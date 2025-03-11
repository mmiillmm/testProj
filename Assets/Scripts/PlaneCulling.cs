using UnityEngine;
using System.Collections.Generic;

public class PlaneSpawner : MonoBehaviour
{
    public GameObject planePrefab;
    public Transform player;
    public float planeWidth = 10f;
    public float spawnThreshold = 5f;
    public float removeDistance = 15f;

    private Queue<GameObject> spawnedPlanes = new Queue<GameObject>();
    private GameObject lastPlane;

    void Start()
    {
        lastPlane = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);
        spawnedPlanes.Enqueue(lastPlane);
    }

    void Update()
    {
        if (player.position.x >= lastPlane.transform.position.x - (planeWidth / 2) + spawnThreshold)
        {
            SpawnNewPlane();
        }

        if (spawnedPlanes.Count > 0 && player.position.x >= spawnedPlanes.Peek().transform.position.x + removeDistance)
        {
            Destroy(spawnedPlanes.Dequeue());
        }
    }

    void SpawnNewPlane()
    {
        Vector3 spawnPos = lastPlane.transform.position + new Vector3(planeWidth, 0, 0);
        lastPlane = Instantiate(planePrefab, spawnPos, Quaternion.identity);
        spawnedPlanes.Enqueue(lastPlane);
    }
}
