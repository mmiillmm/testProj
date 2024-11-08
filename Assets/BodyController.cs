using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float bodyHeightOffset = 1.5f; // Distance from ground
    [SerializeField] private float maxTiltAngle = 30f;      // Maximum tilt angle
    [SerializeField] private float adjustmentSpeed = 5.0f;  // Speed of height and tilt adjustment

    private Vector3 targetPosition;
    private Quaternion initialRotation;     // Store initial rotation
    private Quaternion targetRotation;

    void Start()
    {
        // Capture the initial rotation of the body to use as a base
        initialRotation = transform.rotation;
    }

    void Update()
    {
        AdjustHeightAndTilt();
    }

    private void AdjustHeightAndTilt()
    {
        // Raycast downward from the body's position to find terrain
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f, terrainLayer))
        {
            // Set target position to the hit point plus the height offset
            targetPosition = hit.point + Vector3.up * bodyHeightOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * adjustmentSpeed);

            // Calculate slope rotation relative to the initial rotation
            Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * initialRotation;
            targetRotation = Quaternion.RotateTowards(transform.rotation, slopeRotation, maxTiltAngle);

            // Smoothly adjust the body's rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * adjustmentSpeed);
        }
    }
}
