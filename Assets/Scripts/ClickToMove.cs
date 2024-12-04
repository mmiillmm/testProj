using UnityEngine;

public class ClickMovement : MonoBehaviour
{
    [SerializeField] private LayerMask mapLayer; // Layer for clickable areas
    [SerializeField] private Transform body;    // Reference to the character's body
    [SerializeField] private float moveSpeed = 3.0f;

    private Vector3 targetPosition;

    void Start()
    {
        // Start with the current body position as the target
        targetPosition = body.position;
    }

    void Update()
    {
        // Detect mouse clicks and set the target position
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, mapLayer))
            {
                targetPosition = hit.point;
                targetPosition.y = body.position.y; // Maintain current height
            }
        }

        // Smoothly move the body toward the target position
        if (Vector3.Distance(body.position, targetPosition) > 0.1f)
        {
            body.position = Vector3.MoveTowards(body.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
