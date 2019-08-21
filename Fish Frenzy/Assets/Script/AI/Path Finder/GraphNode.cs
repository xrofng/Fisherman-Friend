using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour {

    public List<GraphNode> neighbors = new List<GraphNode>();

    public void AddNeighbour(GraphNode neigh)
    {
        if (!neighbors.Contains(neigh))
        {
            neighbors.Add(neigh);
        }
    }

    public void ClearMissingNeighbour()
    {
        for (var i = neighbors.Count - 1; i > -1; i--)
        {
            if (neighbors[i] == null)
            {
                neighbors.RemoveAt(i);
            }
        }
    }

    [Header("Debug")]
    public bool showRay = true;
    public float rayRadius=0.6f;
    public Color rayColor = Color.green;

    void OnDrawGizmos()
    {
        if (!showRay)
        {
            return;
        }
        Gizmos.color = rayColor;
        Gizmos.DrawWireSphere(transform.position, rayRadius);

        if (neighbors.Count <= 0)
        {
            return;
        }

        Gizmos.color = Color.green;
        foreach (GraphNode node in neighbors)
        {
            if (node)
            {
                Gizmos.DrawLine(transform.position, node.transform.position);
            }
        }
    }
}
