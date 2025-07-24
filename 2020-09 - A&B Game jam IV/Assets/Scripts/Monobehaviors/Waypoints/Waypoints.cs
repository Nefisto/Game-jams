using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Color32 lineColor = Color.white;

    private void OnDrawGizmos()
    {
        if (transform.childCount < 2)
            return;

        Gizmos.color = lineColor;

        var current = transform.GetChild(0);
        Gizmos.DrawWireSphere(current.position, .1f);
        for (var i = 1; i < transform.childCount; i++)
        {
            var target = transform.GetChild(i);

            Gizmos.DrawLine(current.position, target.position);
            Gizmos.DrawWireSphere(target.position, .1f);

            current = target;
        }
    }

    public List<Transform> GetAllNodes()
    {
        if (transform.childCount < 1)
            throw new System.Exception("Trying to get nodes from an empty path");

        var nodes = new List<Transform>();

        foreach (Transform node in transform)
            nodes.Add(node);

        return nodes;
    }
}