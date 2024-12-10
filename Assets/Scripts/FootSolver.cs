using UnityEngine;

public class FootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private FootSolver otherFoot;
    [SerializeField] private Transform body;

    private float stepDistance = 1f;
    private float stepHeight = 1.0f;
    private float stepLength = 1.5f;
    private float footSpacing = 0.5f;
    private float speed = 5.0f;

    public Vector3 footOffset = Vector3.zero;
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

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, terrainLayer))
        {
            if (Vector3.Distance(currentPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;
                Vector3 direction = (body.forward).normalized;
                newPosition = hit.point + direction * stepLength + footOffset + (body.right * footSpacing);

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

    public bool isMoving() => lerp < 1;
}
