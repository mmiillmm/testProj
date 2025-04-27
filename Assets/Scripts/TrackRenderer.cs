using UnityEngine;

public class TrackVisualizer : MonoBehaviour
{
    public Transform[] trackWaypoints;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = trackWaypoints.Length;

        for (int i = 0; i < trackWaypoints.Length; i++)
        {
            lineRenderer.SetPosition(i, trackWaypoints[i].position);
        }
    }
}
