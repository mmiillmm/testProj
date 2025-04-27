using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public Transform[] trackWaypoints; // Array of waypoints along the track
    public float speed = 10f; // Speed at which the train moves

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (currentWaypointIndex < trackWaypoints.Length)
        {
            // Move towards the next waypoint
            Transform targetWaypoint = trackWaypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            // When the train reaches the current waypoint, move to the next one
            if (transform.position == targetWaypoint.position)
            {
                currentWaypointIndex++;
            }
        }
    }
}
