using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door; // Reference to the door object
    public Vector3 openPosition; // Position where the door opens
    public float openSpeed = 2f; // Speed at which the door opens
    private Vector3 closedPosition; // Original position of the door
    private bool isOpen = false; // Whether the door is open or not

    void Start()
    {
        // Store the original position of the door
        closedPosition = door.position;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger area
        if (other.CompareTag("Player"))
        {
            isOpen = true; // Set the door to open
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger area
        if (other.CompareTag("Player"))
        {
            isOpen = false; // Set the door to close
        }
    }

    void Update()
    {
        // Open or close the door based on the isOpen flag
        if (isOpen)
        {
            door.position = Vector3.Lerp(door.position, openPosition, Time.deltaTime * openSpeed);
        }
        else
        {
            door.position = Vector3.Lerp(door.position, closedPosition, Time.deltaTime * openSpeed);
        }
    }
}
