using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AllyCharacterMovement : MonoBehaviour
{

    public float moveSpeed = 16f;

    Vector3 hoverPositionNav = Vector3.zero;
    LineRenderer pathDisplay;
    Vector3 hoverPositionTrue = Vector3.zero;
    GameObject positionIndicator;
    NavMeshPath path;

    BattleManager battleManager;
    NavMeshAgent myAgent;
    Character myCharacter;

    //movement
    List<Vector3> pathPoints = new List<Vector3>();
    int currentPointIndex = 0;

    bool isMoving = false;
    bool hasValidPath = false;



    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myCharacter = GetComponent<Character>();

        //get values from Battle Manager
        battleManager = FindObjectOfType<BattleManager>();


        //create line
        pathDisplay = gameObject.AddComponent<LineRenderer>();
        pathDisplay.widthMultiplier = 0.15f;
        pathDisplay.colorGradient = battleManager.pathValidGradient;
        pathDisplay.material = battleManager.lineMat;

        //create line end indicator
        /*positionIndicator = Resources.Load<GameObject>("RadiusSelector");
        positionIndicator = Instantiate(positionIndicator);
        positionIndicator.GetComponentInChildren<MeshRenderer>().material.color = new Color32(0, 163, 255, 255);
        positionIndicator.transform.localScale = Vector3.one * 1.5f;
        */

        //make nav mesh path
        path = new NavMeshPath();
    }


    

    private void ClickPosition()
    {
        if (hasValidPath)
        {
            pathPoints.Clear();
            for (int i = 0; i < path.corners.Length; i++)
            {
                pathPoints.Add(path.corners[i]);
            }
            pathPoints.Add(hoverPositionTrue);
            currentPointIndex = 0;
            isMoving = true;
            pathDisplay.enabled = false;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            FollowPath();
        }
        else
        {
            GetPath();
            if (Input.GetMouseButtonDown(0))
            {
                ClickPosition();
            }
        }
    }


    void FollowPath()
    {
        Vector3 targetPoint = pathPoints[currentPointIndex];


        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            currentPointIndex++;

            // Stop if we've reached the end of the path
            if (currentPointIndex >= pathPoints.Count)
            {
                isMoving = false;
                pathDisplay.positionCount = 0;
                pathDisplay.enabled = true;

            }
        }
    }




    void GetPath()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, battleManager.groundLayerMask) && !isMoving
            && NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, myAgent.radius, NavMesh.AllAreas))
        {
            SetValidPath(true);
            hoverPositionTrue = hit.point;
            //do navigation calculation
            if (Vector3.Distance(hit.point, hoverPositionNav) > 1)
            {
                hoverPositionNav = hit.point;
                NavMesh.CalculatePath(transform.position, hoverPositionNav, NavMesh.AllAreas, path);
            }

            //update line renderer
            pathDisplay.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
            {
                pathDisplay.SetPosition(i, path.corners[i]);
            }
        }
        else
        {
            SetValidPath(false);
        }

        //if our hovered cursor exceeds the limit of our player's movement
        if (GetTotalDistance(path.corners) > myCharacter.moveDistance)
        {
            SetValidPath(false);
            Vector4 final = GetMaxDistanceOnPath(path.corners);
            InsertPosition((int)final.w + 1, final);
        }
    }


    void SetValidPath(bool isValid)
    {
        hasValidPath = isValid;
        if (isValid)
        {
            pathDisplay.colorGradient = battleManager.pathValidGradient;
        }
    }


    void SetPathGradient()
    {
        float amtOver = GetTotalDistance(path.corners) - myCharacter.moveDistance;
        if (amtOver > 0)
        {
            float amtOverPercent = amtOver / GetTotalDistance(path.corners);
            AddColorKey(battleManager.pathInvalidColor, Mathf.Abs( 1 - amtOverPercent));

        }
    }

    #region line visuals
    void AddColorKey(Color color, float time)
    {
        GradientColorKey[] existingKeys = battleManager.pathValidGradient.colorKeys;

        GradientColorKey[] newKeys = new GradientColorKey[existingKeys.Length + 2];

        for (int i = 0; i < existingKeys.Length; i++)
        {
            newKeys[i] = existingKeys[i];
        }
        newKeys[newKeys.Length - 1] = new GradientColorKey(newKeys[0].color, time);
        newKeys[newKeys.Length - 3] = new GradientColorKey(color, 1);
        newKeys[newKeys.Length - 2] = new GradientColorKey(color, time + 0.01f);
        Gradient gradient = new Gradient();
        gradient.colorKeys = newKeys;
        pathDisplay.colorGradient = gradient;

    }

    Vector4 GetMaxDistanceOnPath(Vector3[] points)
    {
        Vector4 finalPoint = Vector4.zero;
        float totalDistance = 0;

        for (int i = 0; i < points.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(points[i], points[i + 1]);
            if (totalDistance > myCharacter.moveDistance)
            {
                //get the amount of distance the player has left.
                totalDistance -= Vector3.Distance(points[i], points[i + 1]);
                float extra = myCharacter.moveDistance - totalDistance;
                Vector3 direction = points[i + 1] - points[i];
                direction = Vector3.Normalize(direction);
                finalPoint = points[i] + (direction * extra);
                finalPoint.w = i;
                break;
            }
        }
        return finalPoint;
    }

    void InsertPosition(int index, Vector3 pos)
    {
        pathDisplay.positionCount = index + 1;

        pathDisplay.SetPosition(index, pos);
    }


    float GetTotalDistance (Vector3[] points)
    {
        float totalDistance = 0;

        for (int i = 0; i < points.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(points[i], points[i+1]);
        }

        return totalDistance;
    }



    #endregion
}
