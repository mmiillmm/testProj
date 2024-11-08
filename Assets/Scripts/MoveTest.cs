using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float moveSpeed = 5f; // Set the speed of movement

    void Update()
    {
        // Move the object along the X axis at the specified speed
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
}
