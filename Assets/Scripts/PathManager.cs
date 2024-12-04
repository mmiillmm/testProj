using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; // Array of waypoints defining the path
    [SerializeField] private LineRenderer lineRenderer;

    private void Start()
    {
        // Draw the path using LineRenderer
        if (lineRenderer != null && waypoints.Length > 1)
        {
            lineRenderer.positionCount = waypoints.Length;
            for (int i = 0; i < waypoints.Length; i++)
            {
                lineRenderer.SetPosition(i, waypoints[i].position);
            }
        }
    }

    public Transform[] GetWaypoints() => waypoints;
}
