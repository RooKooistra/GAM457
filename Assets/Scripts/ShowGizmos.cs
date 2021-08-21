using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGizmos : MonoBehaviour
{
    [Tooltip("Colour of solid Gizmo shape")]
    [Range(0, 1)] public float GizmoRedValue;
    [Range(0, 1)] public float GizmoGreenValue;
    [Range(0, 1)] public float GizmoBlueValue;

    [Range(.1f, 10)] public float GizmoSize = 1f;

    [Tooltip("All Gizmos are cubes by default")]
    public bool changeGizmoToSphere = false;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(GizmoRedValue, GizmoGreenValue, GizmoBlueValue, 0.25f);
        if (changeGizmoToSphere)
        {
            Gizmos.DrawSphere(transform.position, GizmoSize);
        }
        else
        {
            Gizmos.DrawCube(transform.position, new Vector3(GizmoSize, GizmoSize, GizmoSize));
        }

        Gizmos.color = new Color(GizmoRedValue, GizmoGreenValue, GizmoBlueValue, 1f);
        if (changeGizmoToSphere)
        {
            Gizmos.DrawWireSphere(transform.position, GizmoSize);
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GizmoSize, GizmoSize, GizmoSize));
        }
    }
}
