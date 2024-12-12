using UnityEngine;

public class RunTest : MonoBehaviour
{
    public float moveSpeed = 10f;
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
}
