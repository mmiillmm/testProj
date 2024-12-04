using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private PathManager pathManager;
    [SerializeField] private float moveSpeed = 5f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        waypoints = pathManager.GetWaypoints();
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position; // Start at the first waypoint
        }

    }

    private void Update()
    {
        if (waypoints.Length == 0 || currentWaypointIndex >= waypoints.Length) return;

        // Move towards the current waypoint
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the model has reached the current waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    private void OnMouseDown()
    {
        // Move to the clicked waypoint
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Waypoint"))
            {
                // Find the closest waypoint in the path array
                currentWaypointIndex = System.Array.IndexOf(waypoints, hit.transform);
            }
        }
    }

}
