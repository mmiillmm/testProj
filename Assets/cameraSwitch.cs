using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{
    public Transform camera;              
    public Vector3 newPos;       
    public float moveSpeed = 2f;
    private Vector3 initPos;
    private bool isMoving = false;

    void Start()
    {
        initPos = camera.position;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = false;
        }
    }
    void Update()
    {
        if (isMoving)
        {
            camera.position = Vector3.Lerp(camera.position, newPos, Time.deltaTime * moveSpeed);
        }
    }
}
