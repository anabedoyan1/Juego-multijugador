using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RailNode : MonoBehaviour
{
    [SerializeField] private RailNode[] adjacentRailNodes = null;

    private void Awake()
    {
        foreach (RailNode node in this.adjacentRailNodes) {
            if (node == null) {
                continue;
            }

            RailManager.AddRailroad((this, node));
        }
    }

    private void Update()
    {
        if(RailManager.localPlayerRailedObject == null) {
            return;
        }

        if (Vector3.Distance(this.transform.position, RailManager.localPlayerRailedObject.transform.position) 
            < RailManager.localPlayerRailedObject.GetDistanceToCurrentNode()) {
            RailManager.localPlayerRailedObject.SetNearestRailNode(this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(this.adjacentRailNodes == null) {
            return;
        }

        foreach (RailNode node in this.adjacentRailNodes) {
            if(node == null) {
                continue;
            }

            Handles.DrawDottedLine(this.transform.position, node.transform.position, 3f);
        }
    }
#endif
}
