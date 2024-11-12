using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door;               // Reference to the door object
    public Vector3 openPosition;         // Position where the door opens
    public float openSpeed = 2f;         // Speed at which the door opens
    public int unlockCost = 50;          // Cost to unlock the door

    private Vector3 closedPosition;      // Original position of the door
    private bool isOpen = false;         // Whether the door is open or not
    private bool isUnlocked = false;     // Whether the door is unlocked or not
    private PlayerInventory playerInventory; // Reference to player inventory

    void Start()
    {
        closedPosition = door.position;  // Store the original position of the door
        playerInventory = FindFirstObjectByType<PlayerInventory>(); // Assumes there's only one PlayerInventory in the scene
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger area
        if (other.CompareTag("Player") && playerInventory != null)
        {
            if (!isUnlocked && playerInventory.money >= unlockCost)
            {
                playerInventory.SpendMoney(unlockCost);
                isUnlocked = true;
                isOpen = true; // Set the door to open
                Debug.Log("Door unlocked and opened!");
            }
            else if (!isUnlocked)
            {
                Debug.Log("Not enough money to unlock the door.");
            }
            else
            {
                isOpen = true; // If already unlocked, set to open
            }
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
        // Move the door to the target position based on isOpen state
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
