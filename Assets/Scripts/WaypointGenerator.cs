using System.Collections.Generic;
using UnityEngine;

public class WaypointGenerator : MonoBehaviour
{
    public GameObject waypointPrefab;
    public GameObject groundPrefab;
    public Transform train;
    public Transform baseGround;

    public float minDistance = 40f;
    public float maxDistance = 80f;

    public List<Transform> waypoints = new List<Transform>();

    private float nextSpawnX;
    private float lastGroundEndX;

    void Start()
    {
        nextSpawnX = train.position.x + Random.Range(minDistance, maxDistance);
        lastGroundEndX = baseGround.position.x + baseGround.localScale.x * 5f;
        SpawnWaypoint();
    }

    void Update()
    {
        if (train.position.x >= nextSpawnX - 10f)
        {
            nextSpawnX = train.position.x + Random.Range(minDistance, maxDistance);
            SpawnWaypoint();
        }
    }

    void SpawnWaypoint()
    {
        Vector3 spawnPosition = train.position + train.right * (nextSpawnX - train.position.x);
        spawnPosition.y = train.position.y;
        spawnPosition.z = train.position.z;

        GameObject waypoint = Instantiate(waypointPrefab, spawnPosition, Quaternion.identity);
        waypoints.Add(waypoint.transform);

        Vector3 groundSize = groundPrefab.GetComponent<Renderer>().bounds.size;
        Vector3 groundPosition = new Vector3(lastGroundEndX + groundSize.x / 2f, baseGround.position.y, baseGround.position.z);

        Instantiate(groundPrefab, groundPosition, Quaternion.identity);

        lastGroundEndX += groundSize.x;
    }
}
