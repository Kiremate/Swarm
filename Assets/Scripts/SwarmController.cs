using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitController;

public class SwarmController : MonoBehaviour
{

    [SerializeField]
    Camera currentCamera;
    [SerializeField]
    private List<UnitController> units;
    [SerializeField]
    private List<UnitController> selectedUnits;
    [SerializeField]
    private Team swarmTeam;

    [SerializeField]
    public Material selectedMaterial;

    Vector3 select_boxOrigin;
    Vector3 select_boxEnd;
    bool unitSelection = false;
    Rect selectionWindow = new Rect();
    private void Reset()
    {
        GetUnitsFromScene();
    }

    private void GetUnitsFromScene()
    {
        UnitController[] tmp_units = GameObject.FindObjectsOfType<UnitController>();
        foreach (var unit in tmp_units)
        {
            if (unit.CurrentTeam == swarmTeam)
            {
                units.Add(unit);
            }
        }
        units.AddRange(tmp_units);
    }
    
    private void ApplyMaterials()
    {
        foreach (var unit in units)
        {
            if (unit.CurrentTeam == swarmTeam)
            {
                units.Add(unit);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetUnitsFromScene();
        SetSelectedMaterial();
    }

    private void SetSelectedMaterial()
    {
        foreach (var unit in selectedUnits)
        {
            unit.SetSelected();
        }
    }

    private void SelectUnit()
    {
        
    }
    private void DeselectAllUnits()
    {
        foreach (var unit in selectedUnits)
        {
            unit.SetDeselected();
        }
        selectedUnits.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            unitSelection = true;
            RaycastHit hit;
            
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

            Debug.Log("Posicion Inicial " + ray.origin);
            if (Physics.Raycast(ray, out hit))
            {
                UnitController controller;
                hit.transform.TryGetComponent(out controller);
                // Select Origin of the box
                select_boxOrigin = Input.mousePosition;
                if (controller && controller.CurrentTeam == swarmTeam)
                {
                    DeselectAllUnits();
                    selectedUnits.Add(controller);
                    controller.SetSelected();
                }
            }

        }

        if (unitSelection)
        {
            select_boxEnd = Input.mousePosition;
            var xInc = (int)select_boxEnd.x - select_boxOrigin.x;
            var yInc = (int)select_boxEnd.y - select_boxOrigin.y;
            if(xInc < 0)
            {
                selectionWindow.x = select_boxOrigin.x + xInc;
            }

            if(yInc  > 0)
            {
                selectionWindow.y = select_boxOrigin.y + yInc;
            }

            selectionWindow.width = Math.Abs(xInc);
            selectionWindow.height = Math.Abs(yInc);


            SelectInArea();

        }


        if (Input.GetMouseButtonUp(0))
        {
            unitSelection = false;
            RaycastHit hit;

            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

            Debug.Log("Posicion Final ");

            select_boxEnd = Input.mousePosition;

        }

    

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(ray.origin);
                foreach (var unit in selectedUnits)
                {
                    unit.GoTo(hit.point);
                }
            }

           

        }
        
    }
    private void SelectInArea()
    {
        DeselectAllUnits();

        foreach (UnitController unit in units)
        {
            // check if is inside the region
            if(unit.screenPosition.x >= selectionWindow.x && 
                unit.screenPosition.x <= (selectionWindow.x + selectionWindow.width) &&
                    unit.screenPosition.y <= selectionWindow.y &&
                    unit.screenPosition.y >= (selectionWindow.y - selectionWindow.height)){
                unit.SetSelected();
                selectedUnits.Add(unit);
            }
            else
                unit.SetDeselected();

            //if (selectionWindow.Contains(unit.screenPosition, true))
            //    unit.SetSelected();
            //else
            //    unit.SetDeselected();
        }
    }
    private void OnGUI()
    {
        // check this https://forum.unity.com/threads/defining-the-bounds-for-a-multi-gameobject-selection.144850/
        if (unitSelection)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(0, 0, 255, 0.08f));
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(new Rect(
                selectionWindow.x,
                Screen.height - selectionWindow.y,
                selectionWindow.width,
                selectionWindow.height
            ), GUIContent.none);
        }
    }


}
