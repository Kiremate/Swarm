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
            RaycastHit hit;
            
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

            Debug.Log("Posicion Inicial " + ray.origin);
            if (Physics.Raycast(ray, out hit))
            {
                UnitController controller;
                hit.transform.TryGetComponent(out controller);
                // Select Origin of the box
                select_boxOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (controller && controller.CurrentTeam == swarmTeam)
                {
                    DeselectAllUnits();
                    selectedUnits.Add(controller);
                    controller.SetSelected();
                }
            }



        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;

            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

            Debug.Log("Posicion Final ");

            select_boxEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }

        void SelectInArea()
        {

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
}
