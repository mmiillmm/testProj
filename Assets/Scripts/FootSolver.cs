using UnityEngine;

public class FootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private FootSolver otherFoot;
    private float stepDistance = 1f; // Reduced distance for more frequent stepping
    private float stepHeight = 1.0f;   // Higher arc for more realistic lift
    private float stepLength = 1.5f;   // Increased length for longer strides
    private float footSpacing = 0.5f;
    private float speed = 1.0f;
    [SerializeField] private Transform body;
    public Vector3 footOffset = Vector3.zero;

    private Vector3 oldPosition, newPosition, currentPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    private float lerp;

    void Start()
    {
        // Set initial positions and normals
        currentPosition = body.position + (body.right * footSpacing) + footOffset;
        oldPosition = newPosition = currentPosition;
        oldNormal = currentNormal = newNormal = transform.up;
        lerp = 1;
    }

    void Update()
    {
        // Set the foot position and orientation
        transform.position = currentPosition;
        transform.up = currentNormal;

        // Raycast to find the ground position for the foot
        Vector3 rayOrigin = body.position + (body.right * footSpacing);
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            // Trigger a new step if the foot is far enough and the other foot is not moving
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;

                // Determine step direction based on body's local z-axis position
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;

                // Set new target position and normal for the step
                newPosition = hit.point + (body.forward * stepLength * direction) + footOffset + (body.right * footSpacing);
                newPosition.y = Mathf.Max(newPosition.y, hit.point.y); // Ensure foot stays above ground

                newNormal = hit.normal;
            }
        }

        // Perform step interpolation if the foot is moving
        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight; // Higher arc for more realistic lift
            currentPosition = tempPos;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            // Update old position and normal after completing the step
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    public bool isMoving()
    {
        return lerp < 1;
    }
}