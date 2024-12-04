using UnityEngine;

public class FootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private FootSolver otherFoot;
    [SerializeField] private Transform body;

    private float stepDistance = 1f; // Distance before a new step is triggered
    private float stepHeight = 1.0f; // Arc height of the step
    private float stepLength = 1.5f; // Length of the step forward
    private float footSpacing = 0.5f; // Lateral spacing of the foot
    private float speed = 5.0f; // Speed of the step interpolation

    public Vector3 footOffset = Vector3.zero; // Offset for foot position adjustments
    private Vector3 oldPosition, newPosition, currentPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    private float lerp;

    void Start()
    {
        // Initialize foot positions and normals
        currentPosition = body.position + (body.right * footSpacing) + footOffset;
        oldPosition = newPosition = currentPosition;
        oldNormal = currentNormal = newNormal = transform.up;
        lerp = 1;
    }

    void Update()
    {
        // Update foot position and orientation
        transform.position = currentPosition;
        transform.up = currentNormal;

        // Cast a ray to detect the terrain below
        Vector3 rayOrigin = body.position + (body.right * footSpacing);
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, terrainLayer)) // Adjusted raycast length
        {
            // Trigger a step if the distance from the current position is large enough
            if (Vector3.Distance(currentPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;

                // Calculate the target position for the step
                Vector3 direction = (body.forward).normalized;
                newPosition = hit.point + direction * stepLength + footOffset + (body.right * footSpacing);

                // Ensure the foot stays above ground
                newPosition.y = Mathf.Max(newPosition.y, hit.point.y);
                newNormal = hit.normal;
            }
        }

        // Handle step interpolation
        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight; // Add arc for step height
            currentPosition = tempPos;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            // Reset old position and normal after completing the step
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    public bool isMoving() => lerp < 1;
}
