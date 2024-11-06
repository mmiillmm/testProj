using UnityEngine;

public class FootSolver : MonoBehaviour
{
    public LayerMask terrainLayer;
    public FootSolver otherFoot;
    public float stepDistance = 1.0f;
    public float stepHeight = 0.5f;
    public float stepLength = 0.5f;
    public float footSpacing; // Set unique spacing for each foot in Inspector
    public float speed = 5.0f;
    public Transform body;
    public Vector3 footOffset;

    private Vector3 oldPosition, newPosition, currentPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    private float lerp;

    void Start()
    {
        // Initialize the position of the foot with spacing relative to the body
        currentPosition = body.position + (body.right * footSpacing) + footOffset;
        oldPosition = newPosition = currentPosition;
        oldNormal = currentNormal = newNormal = transform.up;
        lerp = 1;
    }

    void Update()
    {
        // Update the transform position and orientation
        transform.position = currentPosition;
        transform.up = currentNormal;

        // Raycast to find the ground position for the foot
        Vector3 rayOrigin = body.position + (body.right * footSpacing);
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            // Check if the foot needs to take a step
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;

                // Calculate step direction based on the forward/backward relation to the new hit point
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;

                // Calculate the new target position with foot spacing to ensure feet remain apart
                newPosition = hit.point + (body.forward * stepLength * direction) + footOffset + (body.right * footSpacing);
                newPosition.y = Mathf.Max(newPosition.y, hit.point.y);  // Prevent going underground

                newNormal = hit.normal;
            }
        }

        // Lerp foot position if it's moving
        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = tempPos;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    public bool isMoving()
    {
        return lerp < 1;
    }
}
