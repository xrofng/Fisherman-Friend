using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    private List<GraphNode> _nodeList = new List<GraphNode>();
    public List<GraphNode> NodeList
    {
        get
        {
            if (_nodeList.Count <= 0)
            {
                foreach (GraphNode node in FindObjectsOfType<GraphNode>())
                {
                    _nodeList.Add(node);
                }
            }
            return _nodeList;
        }
    }

    public GraphNode GetClosestNode(GameObject find, GraphNode closeToFind=null)
    {
        GraphNode nodeCloseToFind = closeToFind == null ? NodeList[0]:closeToFind;

        if (!nodeCloseToFind)
        {

        }
        foreach (GraphNode node in NodeList)
        {
            Vector3 toFind, currentToFind;

            toFind = find.transform.position - node.transform.position;

            currentToFind = find.transform.position - nodeCloseToFind.transform.position;

            if(currentToFind.sqrMagnitude > toFind.sqrMagnitude)
            {
                nodeCloseToFind = node;
            }
        }
        return nodeCloseToFind;
    }

}
