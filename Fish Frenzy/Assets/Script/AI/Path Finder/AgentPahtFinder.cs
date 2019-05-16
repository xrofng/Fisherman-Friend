using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentPahtFinder : MonoBehaviour
{
    public Agent agent;

    public bool pathSmooth = true;
    public bool printLog;

    public bool useSelfNodeList = false;
    public List<GraphNode> selfNodeList = new List<GraphNode>();
    public List<GraphNode> NodeList
    {
        get
        {
            if (useSelfNodeList)
            {
                return selfNodeList;
            }
            return FFGameManager.Instance.PathManager.NodeList;
        }
    }

    private PathManager _pathManager;
    public PathManager PathManager
    {
        get
        {
            if (!_pathManager)
            {
                _pathManager = FFGameManager.Instance.PathManager;
            }
            return _pathManager;
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    [Header("Agent&Target")]
    public float reachingRange = 0.5f;
    private bool agentSeeking = false;
    private GameObject target;

    private GraphNode closeToAgent;
    private GraphNode currentCloseToAgent;
    private GraphNode closeToTarget;

    private bool isPathReady = false;
    private bool seekingToTarget;

    private Dictionary<GraphNode, bool> nodeVisited = new Dictionary<GraphNode, bool>();
    private Dictionary<GraphNode, GraphNode> reachBy = new Dictionary<GraphNode, GraphNode>();

    public Color pathColor = Color.white;

    public void StartPathFinding()
    {
        if (PathFinderCondition())
        {
            Debug.Log("No agent and target assign");
            return;
        }

        closeToAgent = NodeList[0];
        closeToTarget = NodeList[0];


        // find closest
        closeToAgent = PathManager.GetClosestNode(agent.gameObject);
        closeToTarget = PathManager.GetClosestNode(target.gameObject);
        currentCloseToAgent = closeToAgent;
       
        if (closeToAgent == closeToTarget)
        {
            return;
        }

        A_STAR();

    }

    // Update is called once per frame
    void Update()
    {
        if (seekingToTarget)
        {
            AgentSeekToTarget(closeToTarget);
        }
    }

    public void AddNode(GraphNode node)
    {
        if (!NodeList.Contains(node))
        {
            NodeList.Add(node);
        }
    }

    public void RemoveNode(GraphNode node)
    {
        if (NodeList.Contains(node))
        {
            NodeList.Remove(node);
        }
    }

    public List<GraphNode> GetNodeList()
    {
        return NodeList;
    }

    void A_STAR()
    {
        List<GraphNode> openSet = new List<GraphNode>();
        openSet.Add(closeToAgent);
        List<GraphNode> closedSet = new List<GraphNode>();

        Dictionary<GraphNode, float> gScore = new Dictionary<GraphNode, float>();
        foreach (GraphNode node in NodeList)
        {
            gScore.Add(node, float.MaxValue);
        }
        gScore[closeToAgent] = 0;

        reachBy.Clear();
        while (openSet.Count > 0)
        {
            float heuristic_score = NodeDistance(closeToTarget, closeToAgent);
            GraphNode current = GetLowestFscore(openSet, gScore, heuristic_score);

            if (current == closeToTarget)
            {
                isPathReady = true;
                return;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (GraphNode neigh in current.neighbors)
            {
                if (neigh && !closedSet.Contains(neigh))
                {
                    float tentative_gScore = gScore[current] + NodeDistance(current, neigh);

                    if (!openSet.Contains(neigh))
                    {
                        openSet.Add(neigh);
                    }
                    if (tentative_gScore < gScore[neigh])
                    {
                        UpdateReachBy(neigh, current);
                        gScore[neigh] = tentative_gScore;
                    }

                }
            }
        }


        if (pathSmooth)
        {
            //PathSmooth();
        }
    }

    private void UpdateReachBy(GraphNode reachToNode, GraphNode reachByNode)
    {
        if (reachBy.ContainsKey(reachToNode))
        {
            reachBy.Add(reachToNode, reachByNode);
        }
        else
        {
            reachBy[reachToNode] = reachByNode;
        }
    }


    GraphNode GetLowestFscore(List<GraphNode> openSet, Dictionary<GraphNode, float> gScore, float hScore)
    {
        float lowest = float.MaxValue;
        GraphNode lowestNode = openSet[0];
        foreach (GraphNode node in openSet)
        {
            if (gScore[node] + hScore < lowest)
            {
                lowestNode = node;
                lowest = gScore[node];
            }
        }
        return lowestNode;
    }

    //void PathSmooth()
    //{
    //    if (NodeList.Count <= 0 || !closeToTarget || !closeToAgent)
    //    {
    //        return;
    //    }

    //    if (!isPathReady)
    //    {
    //        return;
    //    }

    //    GraphNode current, parent;
    //    current = closeToTarget;
    //    parent = closeToTarget.parent;

    //    do
    //    {
    //        bool hitSomething = false;
    //        GraphNode checkingNode = current.parent;
    //        GraphNode smoothNode = current.parent;
    //        do
    //        {
    //            RaycastHit hit;
    //            Vector3 rayDirection = (checkingNode.transform.position - current.transform.position).normalized;
    //            float rayDistance = Vector3.Distance(checkingNode.transform.position, current.transform.position);

    //            if (Physics.Raycast(current.transform.position, rayDirection, out hit, rayDistance))
    //            {
    //                current.parent = smoothNode;
    //                hitSomething = true;
    //            }
    //            else
    //            {
    //                smoothNode = checkingNode;
    //                if (checkingNode == closeToAgent)
    //                {
    //                    hitSomething = true;
    //                    current.parent = smoothNode;
    //                }
    //                else
    //                {
    //                    checkingNode = parent;
    //                    parent = checkingNode.parent;
    //                }
    //            }
    //        } while (!hitSomething);

    //        current = current.parent;
    //        parent = current.parent;

    //    } while (!current.Equals(closeToAgent));

    //}

    float NodeDistance(GraphNode a, GraphNode b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }

    void OnDrawGizmos()
    {
        if (NodeList.Count <= 0)
        {
            return;
        }

        if (!closeToTarget || !closeToAgent)
        {
            return;
        }

        if (isPathReady && reachBy.Count > 0)
        {
            Gizmos.color = pathColor;
            GraphNode current, reachToCurrent = null;
            current = closeToTarget;
            if (reachBy.ContainsKey(current))
            {
                reachToCurrent = reachBy[current];
            }

            do
            {
                float upOffset = 0.5f;
                Gizmos.DrawLine(current.transform.position + (Vector3.up * upOffset), reachToCurrent.transform.position + (Vector3.up * upOffset));
                current = reachToCurrent;
                if (reachBy.ContainsKey(current))
                {
                    reachToCurrent = reachBy[current];
                }
            } while (!current.Equals(closeToAgent));
        }
    }

    GraphNode GetReachBy(GraphNode toNode)
    {
        if (reachBy.ContainsKey(toNode))
        {
            return reachBy[toNode];
        }
        return null;
    }

    void AgentSeekToTarget(GraphNode targetNode)
    {
        if (printLog)
        {
            //Debug.Log(targetNode);
        }
        if (!PathFinderCondition() || !GetReachBy(targetNode))
        {
            return;
        }
        if (reachBy.Count <= 0)
        {
            return;
        }

        if (reachBy[targetNode] == currentCloseToAgent)
        {
            Vector3 targetDir = targetNode.gameObject.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 90.0f * Mathf.Deg2Rad, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            transform.Translate(targetDir.normalized);


            Vector3 agentNoY = agent.transform.position;
            agentNoY.y = 0.0f;
            Vector3 targetNoY = targetNode.transform.position;
            targetNoY.y = 0.0f;

            float dis = Vector3.Distance(agentNoY, targetNoY);

            if (dis < reachingRange)
            {
                currentCloseToAgent = targetNode;
            }
            return;
        }

        AgentSeekToTarget(reachBy[targetNode]);
    }

    public void StartSeeking()
    {
        ResetPath();
        StartPathFinding();

        seekingToTarget = true;
    }

    public void StopSeeking()
    {
        seekingToTarget = false;
    }

    private void ResetPath()
    {
        if (nodeVisited.Count <= 0)
        {
            foreach (GraphNode node in NodeList)
            {
                nodeVisited.Add(node, false);
            }
        }
        else
        {
            foreach (GraphNode node in NodeList)
            {
                nodeVisited[node] = false;
            }
        }
    }

    public void ClearReachBy()
    {
        reachBy.Clear();
    }

    public void DestroyNodeList()
    {
        int iteraCount = NodeList.Count;
        for (int i = 0; i < iteraCount; i++)
        {
            Destroy(NodeList[i].gameObject);
        }
        NodeList.Clear();
    }

    protected virtual bool PathFinderCondition()
    {
        return agent && target;
    }

}
