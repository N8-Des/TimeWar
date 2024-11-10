using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class FogOfWarManager : MonoBehaviour
{
    public List <Character> allyCharacters = new List <Character> ();
    public LayerMask groundMask;
    public LayerMask revealMask;
    public float maxGridHeight = 50;
    
    public Transform minCorner;
    public Transform maxCorner;
    public float cellSize = 1f;
    public float maxVerticalSeparation = 5f;


    public Dictionary<Vector2, List<GridNode>> gridNodes;
    public DecalProjector projector;
    public GameObject quad;
    Texture2D fogBuffer;

    int cellsX;
    int cellsZ;
    float gridWidth;
    float gridLength;





    public class GridNode
    {
        public Vector3 position;
        public Dictionary<int, bool> playerVisibility;
        public bool isVisible = false;

        public GridNode(Vector3 pos)
        {
            position = pos;
            playerVisibility = new Dictionary<int, bool>();
        }
    }



    private void Start()
    {
        GenerateGrids();       
        InitializeFogTexture();
    }


    void Update()
    {
        HandleVision();
        FinalizeNodeVisibility();
        UpdateFogTexture();
    }


    void HandleVision()
    {

        for (int i = 0; i < allyCharacters.Count; i++) 
        {
            Character ally = allyCharacters[i];
            Vector3 allyPosition = ally.transform.position;
            foreach (var nodeEntry in gridNodes)
            {
                List<GridNode> nodesAtPosition = nodeEntry.Value;

                for(int j = 0; j < nodesAtPosition.Count; j++)
                {
                    GridNode node = nodesAtPosition[j];
                    if (Vector3.Distance(allyPosition, node.position) > ally.visionRadius + cellSize * 2)
                        continue;

                    RaycastHit hit;
                    if (Physics.Raycast(allyPosition + Vector3.up * 2, node.position - allyPosition, out hit, Vector3.Distance(allyPosition, node.position), groundMask))
                    {
                        node.playerVisibility[i] = false;
                    }
                    else
                    {
                        if (Vector3.Distance(allyPosition, node.position) < ally.visionRadius)
                        {
                            node.playerVisibility[i] = true;
                        }
                        else
                        {
                            node.playerVisibility[i] = false;
                        }
                    }
                }

            }

        }
    }



    void GenerateGrids()
    {
        gridNodes = new Dictionary<Vector2, List<GridNode>>();

        Vector3 gridStart = maxCorner.position;
        Vector3 gridEnd = minCorner.position;

        gridWidth = Mathf.Abs(gridEnd.x - gridStart.x);
        gridLength = Mathf.Abs(gridEnd.z - gridStart.z);


        cellsX = Mathf.CeilToInt(gridWidth / cellSize);
        cellsZ = Mathf.CeilToInt(gridLength / cellSize);

        for (int x = 0; x <= cellsX; x++)
        {
            for (int z = 0; z <= cellsZ; z++)
            {
                float posX = Mathf.Lerp(Mathf.Min(gridStart.x, gridEnd.x), Mathf.Max(gridStart.x, gridEnd.x), (float)x / cellsX);
                float posZ = Mathf.Lerp(Mathf.Min(gridStart.z, gridEnd.z), Mathf.Max(gridStart.z, gridEnd.z), (float)z / cellsZ);

                Vector3 cellPosition = new Vector3(posX, gridStart.y, posZ);
                CreateNodeLayers(cellPosition, x, z);
            }
        }
    }


    void UpdateFogTexture()
    {
        Color32[] colors = new Color32[cellsX * cellsZ];

        // Loop through all the grid nodes
        foreach (var nodeEntry in gridNodes)
        {
            Vector2 nodeKey = nodeEntry.Key;
            List<GridNode> nodesAtPos = nodeEntry.Value;

            foreach (GridNode node in nodesAtPos)
            {
                // Calculate the texture coordinates based on node position in world space
                Vector2 nodePositionInTexture = WorldToTextureCoords(node.position);
                int centerX = Mathf.FloorToInt(nodePositionInTexture.x * cellsX);
                int centerY = Mathf.FloorToInt(nodePositionInTexture.y * cellsZ);



                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    for (int offsetY = -1; offsetY <= 1; offsetY++)
                    {
                        int x = Mathf.Clamp(centerX + offsetX, 0, cellsX - 1);
                        int y = Mathf.Clamp(centerY + offsetY, 0, cellsZ - 1);
                        if (node.isVisible)
                        {
                            colors[y * cellsX + x] = new Color32(255, 255, 255, 100);  // Fully visible (white)
                        }
                        else
                        {
                            colors[y * cellsX + x] = new Color32(0, 0, 0, 255);  // Unseen (black)
                        }
                    }
                }


            }
        }

        // Apply the color changes to the texture
        fogBuffer.SetPixels32(colors);
        fogBuffer.Apply();  // Apply the changes
    }




    Vector2 WorldToTextureCoords(Vector3 worldPosition)
    {
        // Normalize world position to the texture space (0 to 1)
        float textureX = (worldPosition.x - minCorner.position.x) / (maxCorner.position.x - minCorner.position.x);
        float textureY = (worldPosition.z - minCorner.position.z) / (maxCorner.position.z - minCorner.position.z);


        return new Vector2(textureX, textureY);
    }

    void CreateNodeLayers(Vector3 startPosition, int gridX, int gridZ)
    {
        Vector2 node = new Vector2(gridX, gridZ);

        if (!gridNodes.ContainsKey(node))
        {
            gridNodes[node] = new List<GridNode>();
        }

        RaycastHit[] hits = Physics.RaycastAll(startPosition + Vector3.up * maxGridHeight, Vector3.down, 100, groundMask);

        //filter hits by height to create layers
        List<float> heights = new List<float>();

        foreach (RaycastHit hit in hits)
        {
            bool isNewLayer = true;
            /*foreach(float height in heights)
            {
                if(Mathf.Abs(height - hit.point.y) < maxVerticalSeparation)
                {
                    isNewLayer = false;
                    break;
                }
            }*/

            if (isNewLayer)
            {
                GridNode newNode = new(hit.point);
                newNode.position = hit.point + Vector3.up;
                gridNodes[node].Add(newNode);
                heights.Add(hit.point.y);
            }
        }
    }

    void FinalizeNodeVisibility()
    {
        foreach (var nodeEntry in gridNodes)
        {
            List<GridNode> nodesAtPos = nodeEntry.Value;

            foreach (GridNode node in nodesAtPos)
            {
                bool isVisibleToAnyPlayer = false;

                // Check if any player sees this node
                foreach (var playerVisibility in node.playerVisibility)
                {
                    if (playerVisibility.Value)
                    {
                        isVisibleToAnyPlayer = true;
                        break;
                    }
                }

                node.isVisible = isVisibleToAnyPlayer;
            }
        }
    }

    void InitializeFogTexture()
    {


        fogBuffer = new Texture2D(cellsX, cellsZ, TextureFormat.RGBAFloat, false);
        fogBuffer.filterMode = FilterMode.Bilinear;

        projector.material.SetTexture("_FogMap", fogBuffer);
        //quad.GetComponent<MeshRenderer>().material.SetTexture("_FogMap", fogBuffer);

    }

    //show the grid points
    //private void OnDrawGizmos()
    //{
    //    if (gridNodes == null)
    //    {
    //        return;
    //    }


    //    foreach (var nodeEntry in gridNodes)
    //    {
    //        Vector2 nodeKey = nodeEntry.Key;
    //        List<GridNode> nodesAtPos = nodeEntry.Value;

    //        foreach (GridNode node in nodesAtPos)
    //        {
    //            //change color based on if its visible or not
    //            Gizmos.color = node.isVisible ? UnityEngine.Color.green : UnityEngine.Color.red;
    //            Gizmos.DrawSphere(node.position, 0.2f);
    //        }
    //    }
    //}
}
