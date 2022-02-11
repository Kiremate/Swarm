using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public enum Team { BLUE, RED, YELLOW, GREEN }

    [SerializeField]
    private Team currentTeam;
    [SerializeField]
    private Material currentMaterial;
    [SerializeField]
    private Material selectedMaterial;
    private Renderer render;

    private NavMeshAgent agent;

    public Vector3 screenPosition;

    public Team CurrentTeam
    {
        get => currentTeam; protected
set => currentTeam = value;
    }

    public Material CurrentMaterial { get => currentMaterial; set => currentMaterial = value; }
    public Material SelectedMaterial { get => selectedMaterial; set => selectedMaterial = value; }


    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out agent);
        gameObject.TryGetComponent(out render);
    }

    // Update is called once per frame
    void Update()
    {
        this.screenPosition = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void SetSelected()
    {
        render.material = selectedMaterial;
    }
    public void SetDeselected()
    {
        render.material = currentMaterial;
    }

    public void GoTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

}
