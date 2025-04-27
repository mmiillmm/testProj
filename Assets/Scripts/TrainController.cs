using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float speed = 5f;
    public bool isMoving = true;
    public float stopDistance = 2f;

    private WaypointGenerator waypointGenerator;
    private StopWindowUI stopWindowUI;

    void Start()
    {
        waypointGenerator = FindFirstObjectByType<WaypointGenerator>();
        stopWindowUI = FindFirstObjectByType<StopWindowUI>();
    }

    void Update()
    {
        if (!isMoving) return;

        transform.Translate(Vector3.right * speed * Time.deltaTime);

        List<Transform> waypoints = waypointGenerator.waypoints;

        foreach (Transform waypoint in waypoints)
        {
            if (waypoint == null) continue;

            float distance = Vector3.Distance(transform.position, waypoint.position);
            if (distance <= stopDistance)
            {
                StopTrain();
                stopWindowUI.ShowWindow();

                waypoints.Remove(waypoint);
                Destroy(waypoint.gameObject);
                break;
            }
        }
    }

    public void StopTrain()
    {
        isMoving = false;
    }

    public void StartTrain()
    {
        isMoving = true;
    }
}
