using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset; // A kamera és a cél közötti eltérés
    [SerializeField] private float damping; // Simítási sebesség
    [SerializeField] private float minDistance = 1f; // Minimális távolság, amit a kamera megtart
    [SerializeField] private float zoomAggressiveness = 0.75f; // Zoom agresszivitás mértéke
    [SerializeField] private LayerMask obstacleLayers; // Akadályok rétegei
    [SerializeField] private LayerMask terrainLayers; // Terep rétegei

    public Transform target; // A cél, amelyet a kamera követ

    private Vector3 vel = Vector3.zero; // Sebesség vektor a simítási algoritmushoz

    private void FixedUpdate()
    {
        // Számítsuk ki a cél pozícióját az eltéréssel együtt
        Vector3 targetPosition = target.position + offset;
        Vector3 desiredPosition = targetPosition;

        RaycastHit hit;

        // Ha akadályra találunk a cél pozíciójától a kamera pozíciójáig vezető vonalon
        if (Physics.Raycast(target.position, (transform.position - target.position).normalized, out hit, offset.magnitude, obstacleLayers | terrainLayers))
        {
            // Számítsuk ki a távolságot az akadály és a cél között
            float distance = Vector3.Distance(target.position, hit.point);
            // Számítsuk ki a zoom faktort
            float zoomFactor = Mathf.Clamp01(distance / offset.magnitude) * zoomAggressiveness;
            // Állítsuk be a kívánt pozíciót, figyelembe véve a minimális távolságot és a zoom faktort
            desiredPosition = target.position + (offset.normalized * Mathf.Max(minDistance, distance * zoomFactor));
        }

        // Simítsuk a kamera pozícióját a kívánt pozícióra
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref vel, damping);
    }
}
