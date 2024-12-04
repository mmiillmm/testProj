using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; // Waypoints for the path
    [SerializeField] private float lineWidth = 0.1f; // Thickness of the line
    [SerializeField] private Material lineMaterial; // Material for the line

    private LineRenderer lineRenderer;

    void Start()
    {
        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();

        // Configure the LineRenderer properties
        lineRenderer.positionCount = waypoints.Length; // Number of points in the path
        lineRenderer.startWidth = lineWidth;           // Start thickness of the line
        lineRenderer.endWidth = lineWidth;             // End thickness of the line
        lineRenderer.material = lineMaterial;          // Assign material for the line

        // Set the positions for the LineRenderer
        for (int i = 0; i < waypoints.Length; i++)
        {
            lineRenderer.SetPosition(i, waypoints[i].position);
        }
    }

    void OnDrawGizmos()
    {
        // Visualize waypoints in the editor
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
