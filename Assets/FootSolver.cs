using UnityEngine;

public class FootSolver : MonoBehaviour
{
    public LayerMask terrainLayer;
    public FootSolver otherFoot;
    public float stepDistance = 1.0f;
    public float stepHeight = 0.5f;
    public float stepLength = 0.5f;
    public float footSpacing;
    public float speed = 5.0f;
    public Transform body;
    public Vector3 footOffset;

    private Vector3 oldPosition, newPosition, currentPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    private float lerp;

    void Start()
    {
        currentPosition = body.position + (body.right * footSpacing) + footOffset;
        oldPosition = newPosition = currentPosition;
        oldNormal = currentNormal = newNormal = transform.up;
        lerp = 1;
    }

    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        Vector3 rayOrigin = body.position + (body.right * footSpacing);
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;

                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;

                newPosition = hit.point + (body.forward * stepLength * direction) + footOffset + (body.right * footSpacing);
                newPosition.y = Mathf.Max(newPosition.y, hit.point.y);

                newNormal = hit.normal;
            }
        }

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
