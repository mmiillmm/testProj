using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float moveSpeed = 5f;
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
}
