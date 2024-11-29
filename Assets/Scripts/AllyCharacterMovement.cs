using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AllyCharacterMovement : MonoBehaviour
{
    public float moveSpeed = 25;

    Vector3 hoverPositionNav = Vector3.zero;

    NavMeshPath path;
    float pathInterpDist = 2;
    List<Vector3> pathPoints = new List<Vector3>();
    List<Vector3> extraPathPoints = new List<Vector3>();
    List<int> navLinkPoints = new List<int>();
    PlayerPathRenderer pathRenderer;

    BattleManager battleManager;
    NavMeshAgent myAgent;
    Character myCharacter;

    //abilities
    bool inAbilitySelect = false;
    Character targetCharacter;
    Vector3 abilityTargetPosition;
    public int selectedAbilityIndex;
    AbilityBase selectedAbility;
    bool validAbility;
    bool needMovementForAbility = false;

    //movement
    int currentPointIndex = 0;
    bool isMoving = false;
    bool hasValidPath = false;
    float currentPathDistance;
    bool isMyTurn = false;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myCharacter = GetComponent<Character>();

        //get values from Battle Manager
        battleManager = FindObjectOfType<BattleManager>();
        //create line (should probably just turn this into a prefab)
        GameObject pathObject = Resources.Load<GameObject>("PathLineRenderer");
        pathObject = Instantiate(pathObject);
        pathObject.transform.SetParent(transform);
        pathObject.transform.localPosition = Vector3.zero;
        pathObject.transform.eulerAngles = new(90, 0, 0);
        pathRenderer = pathObject.GetComponent<PlayerPathRenderer>();


        //create line end indicator
        /*positionIndicator = Resources.Load<GameObject>("RadiusSelector");
        positionIndicator = Instantiate(positionIndicator);
        positionIndicator.GetComponentInChildren<MeshRenderer>().material.color = new Color32(0, 163, 255, 255);
        positionIndicator.transform.localScale = Vector3.one * 1.5f;
        */

        //make nav mesh path
        path = new NavMeshPath();
    }


    public void StartTurn()
    {
        isMyTurn = true;
        pathRenderer.pathDisplay.enabled = true;
        pathRenderer.extraPathDisplay.enabled = true;
        selectedAbility = null;
    }

    public void EndTurn()
    {
        isMyTurn = false;
        pathRenderer.pathDisplay.enabled = false;
        pathRenderer.extraPathDisplay.enabled = false;
    }


    public void StartAbilitySelection(int index)
    {
        selectedAbilityIndex = index;
        inAbilitySelect = true;
        pathRenderer.pathDisplay.enabled = false;
        pathRenderer.extraPathDisplay.enabled = false;
        targetCharacter = null;
        selectedAbility = myCharacter.abilityManager.abilities[selectedAbilityIndex];
    }

    public void EndAbilitySelection()
    {
        inAbilitySelect = false;
        selectedAbility.EndDisplay();
        pathRenderer.pathDisplay.enabled = true;
    }

    private void ClickPosition()
    {
        if (hasValidPath)
        { 
            currentPointIndex = 0;
            isMoving = true;
            pathRenderer.pathDisplay.enabled = false;
            myAgent.enabled = false;
            pathRenderer.extraPathDisplay.enabled = false;

        }
    }


    void Update()
    {
        if (isMyTurn)
        {
            if (isMoving)
            {
                FollowPath();
            }
            else
            {
                if (!inAbilitySelect)
                {
                    GetPath();
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        ClickPosition();
                    }
                }
                else
                {
                    DisplayAbility();

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (validAbility)
                        {
                            if (needMovementForAbility)
                            {
                                ClickPosition();
                                selectedAbility.EndDisplay();
                            }
                            else
                            {
                                selectedAbility.Execute(myCharacter, targetCharacter, abilityTargetPosition);
                                EndAbilitySelection();
                            }
                        }
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        EndAbilitySelection();
                        selectedAbility.EndDisplay();
                    }
                }

            }
        }
    }


    void DisplayAbility()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, battleManager.abilityLayerMask) && !isMoving)
        {
            abilityTargetPosition = hit.point;
            //check if you hit a character
            if (hit.transform.gameObject.GetComponentInParent<Character>() != null)
            {
                targetCharacter = hit.transform.gameObject.GetComponentInParent<Character>();
            }
        }
        if (Vector3.Distance(abilityTargetPosition, transform.position) >= selectedAbility.config.attackRange)
        {
            Vector3 targetPosition = GetMaxAbilityDistance(transform.position, abilityTargetPosition, selectedAbility.config.attackRange);
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);


            if (path.status != NavMeshPathStatus.PathInvalid && GetTotalDistance(path.corners) <= myCharacter.currentMoveDistance)
            {
                selectedAbility.DisplaySpell(targetPosition, hit.point, myCharacter);
                needMovementForAbility = true;
                hasValidPath = true;

                pathPoints.Clear();
                for (int i = 0; i < path.corners.Length; i++)
                {
                    pathPoints.Add(path.corners[i]);
                }
                GetLinkedPaths();

                pathRenderer.pathDisplay.enabled = true;
                SetPathDisplay(false);
                validAbility = selectedAbility.IsValidSpell(targetPosition, (abilityTargetPosition - targetPosition).normalized, targetCharacter);
            }
            else
            {
                validAbility = false;
                hasValidPath = false;
            }

        }
        else
        {
            selectedAbility.DisplaySpell(transform.position, hit.point, myCharacter);
            validAbility = selectedAbility.IsValidSpell(transform.position, (abilityTargetPosition - transform.position).normalized, targetCharacter);
            needMovementForAbility = false;
            pathRenderer.pathDisplay.enabled = false;
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
                pathRenderer.pathDisplay.positionCount = 0;
                pathRenderer.pathDisplay.enabled = true;
                path.ClearCorners();
                myAgent.enabled = true;
                myCharacter.currentMoveDistance -= currentPathDistance;
                if(inAbilitySelect)
                {
                    selectedAbility.Execute(myCharacter, targetCharacter, targetPoint);
                    EndAbilitySelection();
                }
                pathRenderer.extraPathDisplay.positionCount = 0;
                pathRenderer.extraPathDisplay.enabled = true;
                extraPathPoints.Clear();

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

        SetPathDisplay(true);

    }


    void SetValidPath(bool isValid)
    {
        hasValidPath = isValid;
        pathRenderer.SetPathMaterial(isValid);
    }


    void SetPathDisplay(bool isInterpolated)
    {
        //determine if the path crosses any links
        if (myCharacter.currentMoveDistance <= 0)
        {
            SetValidPath(false);
        }
        //if our hovered cursor exceeds the limit of our player's movement    
        else if (GetTotalDistance(path.corners) > myCharacter.currentMoveDistance)
        {
            Vector4 edgePoint = GetMaxDistanceOnPath(pathPoints.ToArray(), myCharacter.currentMoveDistance);
            int lastIndex = (int)edgePoint.w;

            //handle the extra distance in red
            pathRenderer.extraPathDisplay.enabled = true;
            extraPathPoints.Clear();
            extraPathPoints.Add(edgePoint);
            for (int i = lastIndex + 1; i < pathPoints.Count; i++)
            {
                extraPathPoints.Add(pathPoints[i]);
            }

            if (extraPathPoints.Count > 1 && isInterpolated)
            {
                List<Vector3> interpolatedPoints = PathInterpolation(extraPathPoints.ToArray());
                pathRenderer.extraPathDisplay.positionCount = interpolatedPoints.Count;
                for (int i = 0; i < interpolatedPoints.Count; i++)
                {
                    interpolatedPoints[i] += Vector3.up;
                }
                pathRenderer.extraPathDisplay.SetPositions(interpolatedPoints.ToArray());
            }
            else
            {
                pathRenderer.extraPathDisplay.positionCount = extraPathPoints.Count;
                List<Vector3> pathRenderPoints = new(extraPathPoints);
                for (int i = 0; i < pathRenderPoints.Count; i++)
                {
                    pathRenderPoints[i] += Vector3.up;
                }
                pathRenderer.extraPathDisplay.SetPositions(pathRenderPoints.ToArray());
            }
            //remove any points on the blue path that are outside of our movespeed zone
            pathPoints[lastIndex + 1] = edgePoint;
            if (pathPoints.Count > lastIndex + 1)
            {
                for(int i = pathPoints.Count -1; i > lastIndex + 1; i--)
                {
                    pathPoints.RemoveAt(i);
                }
            }
        }
        else
        {
            pathRenderer.extraPathDisplay.enabled = false;
        }


        //finally, interpolate the path 
        if (pathPoints.Count > 1 && isInterpolated)
        {
            List<Vector3> interpolatedPoints = PathInterpolation(pathPoints.ToArray());
            pathRenderer.pathDisplay.positionCount = interpolatedPoints.Count;
            for (int i = 0; i < interpolatedPoints.Count; i++)
            {
                interpolatedPoints[i] += Vector3.up;
            }
            pathRenderer.pathDisplay.SetPositions(interpolatedPoints.ToArray());
        }
        else
        {
            pathRenderer.pathDisplay.positionCount = pathPoints.Count;
            List<Vector3> pathRenderPoints = new(pathPoints);
            for (int i = 0; i < pathRenderPoints.Count; i++)
            {
                pathRenderPoints[i] += Vector3.up;
            }
            pathRenderer.pathDisplay.SetPositions(pathRenderPoints.ToArray());
        }
        currentPathDistance = GetTotalDistance(path.corners);
    }


    #region line visuals
   


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

    Vector3 GetMaxAbilityDistance(Vector3 start, Vector3 end, float maxDistance)
    {
        Vector3 maxPoint = Vector3.zero;
        float totalDistance = Vector3.Distance(start, end);
        float extraDistance = totalDistance - maxDistance;
        Vector3 direction = end - start;
        direction.Normalize();
        maxPoint = start + (direction * extraDistance);
        return maxPoint;
    }

    Vector3 SnapToGround(Vector3 point)
    {
        RaycastHit hit;
        Vector3 pointAbove = point + Vector3.up * 50;
        if (Physics.Raycast(pointAbove, Vector3.down, out hit, Mathf.Infinity, battleManager.groundLayerMask))
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



    Vector4 GetMaxDistanceOnPath(Vector3[] points, float maxDistance)
    {
        Vector4 finalPoint = Vector4.zero;
        float totalDistance = 0;

        for (int i = 0; i < points.Length - 1; i++)
        {
            if (!navLinkPoints.Contains(i))
            {
                totalDistance += Vector3.Distance(points[i], points[i + 1]);
                if (totalDistance > maxDistance)
                {
                    //get the amount of distance the player has left.
                    totalDistance -= Vector3.Distance(points[i], points[i + 1]);
                    float extra = maxDistance - totalDistance;
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
