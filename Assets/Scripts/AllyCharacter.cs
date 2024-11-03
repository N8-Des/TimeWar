using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyCharacter : Character
{
    Vector3 hoverPosition = Vector3.zero;
    LineRenderer pathDisplay;
    public LayerMask groundLayer;
    public Material lineMaterial;

    private void Start()
    {
        //create and initiate things
        pathDisplay = gameObject.AddComponent<LineRenderer>();
        pathDisplay.material = lineMaterial;
    }


    void Update()
    {
        getPath();
    }



    void getPath()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            if (Vector3.Distance(hit.point, hoverPosition) > 2)
            {
                hoverPosition = hit.point;
                NavMeshPath path = new();
                NavMesh.CalculatePath(transform.position, hoverPosition, NavMesh.AllAreas, path);
                pathDisplay.positionCount = path.corners.Length;
                for (int i = 0; i < path.corners.Length; i++)
                {
                    pathDisplay.SetPosition(i, path.corners[i]);
                }
            }
        }
    }
}
