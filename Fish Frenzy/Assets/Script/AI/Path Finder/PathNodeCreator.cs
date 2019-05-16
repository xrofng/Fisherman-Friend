using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeCreator : MonoBehaviour
{
    public Vector3 startingPoint;

    public GraphNode originalNode;

    public int levelLimit;

    public float nodeOffset;
    public float nodeRadius = 0.5f;
    public LayerMask obstacleLayer;

    //Cached node
    public Dictionary<Vector3, GraphNode> nodeList = new Dictionary<Vector3, GraphNode>();
    public Dictionary<Vector2, GraphNode> nodeByGrid = new Dictionary<Vector2, GraphNode>();

    private GraphNode tempNode;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        SpawnNode(startingPoint, null, 0);
    }

    Vector3 rayCenterT = Vector3.negativeInfinity;
    float rayDistanceT = float.Epsilon;
    Vector3 rayDirectionT = Vector3.negativeInfinity;
    int levelRay = 3;
    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rayCenterT, rayCenterT + (rayDirectionT * rayDistanceT), Color.black);
    }

    GraphNode SpawnNode(Vector3 pos, GraphNode fromNode, int level)
    {
        if (level >= levelLimit)
        {
            return null;
        }
        if (nodeList.ContainsKey(pos) && nodeList[pos] == null)
        {
            return null;
        }

        if (fromNode)
        {
            RaycastHit hit = HitObstacle(fromNode.transform.position, pos);
            if (hit.collider)
            {
                return null;
            }
        }

        if (nodeList.ContainsKey(pos))
        {
            return nodeList[pos];
        }

        GraphNode currentNode;
        if (nodeList.ContainsKey(pos))
        {
            currentNode = nodeList[pos];
            nodeList[pos].AddNeighbour(fromNode);
        }
        else
        {
            // spawn
            currentNode = Instantiate(originalNode, this.transform) as GraphNode;
            Debug.Log("sdsddw");
            Vector3 gridPosV3 = (pos - startingPoint) / nodeOffset;
            Vector2 gridPosV2 = new Vector2(gridPosV3.x, gridPosV3.z);
            currentNode.gameObject.name = "Node" + gridPosV2;
            currentNode.transform.position = pos;
            currentNode.rayRadius = nodeRadius;
            nodeList.Add(pos, currentNode);
            nodeByGrid.Add(gridPosV2, currentNode);
        }

        Vector3[] neighPos = {
            pos + nodeOffset * Vector3.right,
            pos + nodeOffset * Vector3.left,
            pos + nodeOffset * Vector3.forward,
            pos + nodeOffset * Vector3.back,
        };

        for (int i = 0; i < neighPos.Length; i++)
        {
            GraphNode neiNode = SpawnNode(neighPos[i], currentNode, level + 1);
            if (neiNode)
            {
                currentNode.AddNeighbour(neiNode);
                neiNode.AddNeighbour(currentNode);
            }
        }
        return currentNode;
    }

    RaycastHit HitObstacle(Vector3 a, Vector3 b)
    {
        RaycastHit hit;
        float rayDistance = Vector3.Distance(a, b) + nodeRadius;
        Vector3 rayDirection = (b - a).normalized;
        Vector3 rayCenter = a;

        if (levelRay == 0)
        {
            rayCenterT = rayCenter;
            rayDirectionT = rayDirection;
            rayDistanceT = rayDistance;
        }
        levelRay -= 1;

        Physics.Raycast(rayCenter, rayDirection, out hit, rayDistance, obstacleLayer);
        return hit;
    }

    public GraphNode GetNode(Vector2 gridPos)
    {
        if (nodeByGrid.ContainsKey(gridPos))
        {
            return nodeByGrid[gridPos];
        }
        return null;
    }

    
}