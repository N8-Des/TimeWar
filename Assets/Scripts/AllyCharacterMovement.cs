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
    float pathInterpDist = 2;
    List<Vector3> pathPoints = new List<Vector3>();
    List<int> navLinkPoints = new List<int>();

    BattleManager battleManager;
    NavMeshAgent myAgent;
    Character myCharacter;


    //movement
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
            currentPointIndex = 0;
            isMoving = true;
            pathDisplay.enabled = false;
            myAgent.enabled = false;
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


    //movement
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
                myAgent.enabled = true;

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


            //make path list
            pathPoints.Clear();
            for (int i = 0; i < path.corners.Length; i++)
            {
                pathPoints.Add(path.corners[i]);
            }
            GetLinkedPaths();
        }
        else
        {
            SetValidPath(false);
        }

        //determine if the path crosses any links

        //if our hovered cursor exceeds the limit of our player's movement    
        if (GetTotalDistance(path.corners) > myCharacter.moveDistance)
        {
            SetValidPath(false);
            Vector4 edgePoint = GetMaxDistanceOnPath(pathPoints.ToArray());
            int lastIndex = (int)edgePoint.w;
            pathPoints[lastIndex] = edgePoint;
            pathPoints.RemoveRange(lastIndex + 1, pathPoints.Count - (lastIndex + 1));
        }
        

        //finally, interpolate the path 
        if (pathPoints.Count > 1)
        {
            List<Vector3> interpolatedPoints = PathInterpolation(pathPoints.ToArray());
            pathDisplay.positionCount = interpolatedPoints.Count;
            pathDisplay.SetPositions(interpolatedPoints.ToArray());


        }
        else
        {
            pathDisplay.positionCount = pathPoints.Count;
            pathDisplay.SetPositions(pathPoints.ToArray());
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


    public List<Vector3> PathInterpolation(Vector3[] corners)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < corners.Length - 1; i++)
        {
            if (!navLinkPoints.Contains(i))
            {
                Vector3 first = corners[i];
                Vector3 last = corners[i + 1];

                Vector3 dir = (last - first).normalized;
                float distanceBetweenSegments = Vector3.Distance(first, last);

                for (float dist = 0; dist < distanceBetweenSegments; dist += pathInterpDist)
                {
                    Vector3 interpPoint = first + dir * dist;
                    Vector3 snappedPoint = SnapToGround(interpPoint);
                    //we dont want to snap this point if its supposed to be in the air
                    if (!navLinkPoints.Contains(i - 1))
                    {
                        points.Add(snappedPoint);
                    }
                    else
                    {
                        points.Add(interpPoint);
                    }

                }
            }
        }
        points.Add(SnapToGround(corners[corners.Length - 1]));
        return points;
    }


    Vector3 SnapToGround(Vector3 point)
    {
        RaycastHit hit;
        Vector3 pointAbove = point + Vector3.up * 50;
        if (Physics.Raycast(pointAbove, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        return point;
    }


    void GetLinkedPaths()
    {
        navLinkPoints.Clear();
        foreach(OffMeshLink link in FindObjectsOfType<OffMeshLink>())
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                //Going Up?
                if (CheckIfLinkMeshPoint(path.corners[i], link.startTransform.position) && 
                    CheckIfLinkMeshPoint(path.corners[i + 1], link.endTransform.position))
                {
                    navLinkPoints.Add(i);
                    Vector3 topPoint = new(path.corners[i].x, path.corners[i + 1].y, path.corners[i].z);
                    pathPoints.Insert(i + 1, topPoint);
                }

                //Going Down?
                if (CheckIfLinkMeshPoint(path.corners[i], link.endTransform.position) &&
                    CheckIfLinkMeshPoint(path.corners[i + 1], link.startTransform.position))
                {
                    navLinkPoints.Add(i);
                    Vector3 topPoint = new Vector3(path.corners[i + 1].x, path.corners[i].y, path.corners[i + 1].z);
                    pathPoints.Insert(i + 1, topPoint);
                }
            }
        }
    }

    bool CheckIfLinkMeshPoint(Vector3 pathPoint, Vector3 linkPoint) 
    {
        return Mathf.Approximately(pathPoint.x, linkPoint.x) && Mathf.Approximately(pathPoint.z, linkPoint.z);
    }



    Vector4 GetMaxDistanceOnPath(Vector3[] points)
    {
        Vector4 finalPoint = Vector4.zero;
        float totalDistance = 0;

        for (int i = 0; i < points.Length - 1; i++)
        {
            if (!navLinkPoints.Contains(i))
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
        }
        return finalPoint;
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
