using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float bodyHeightOffset = 1.5f;
    [SerializeField] private float adjustmentSpeed = 5.0f;

    private Vector3 targetPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        AdjustHeightAndTilt();
    }

    private void AdjustHeightAndTilt()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f, terrainLayer))
        {
            targetPosition = hit.point + Vector3.up * bodyHeightOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * adjustmentSpeed);

            Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * initialRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, slopeRotation, Time.deltaTime * adjustmentSpeed);
        }
    }
}
