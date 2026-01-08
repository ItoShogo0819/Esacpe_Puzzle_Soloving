using UnityEngine;

public class TargetGizmo : MonoBehaviour
{
    public Color GizmoColor = Color.green;
    public float GizmoSize = 0.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, GizmoSize);
    }
}
