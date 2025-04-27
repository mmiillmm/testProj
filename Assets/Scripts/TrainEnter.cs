using UnityEngine;

public class TrainEnter : MonoBehaviour
{
    private bool isNearTrain = false;
    private GameObject train;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private MonoBehaviour playerMovement; // replace with your actual movement script type if needed

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerController>(); // Replace with your movement script's class name
    }

    void Update()
    {
        if (isNearTrain && Input.GetKeyDown(KeyCode.E))
        {
            EnterTrain();
        }
    }

    private void EnterTrain()
    {
        // Hide the player sprite
        spriteRenderer.enabled = false;

        // Disable player movement
        if (playerMovement != null) playerMovement.enabled = false;
        if (rb != null) rb.simulated = false;

        // Enable train control
        train.GetComponent<TrainController>().enabled = true;

        // Optionally move camera if you're using one
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Train"))
        {
            train = other.gameObject;
            isNearTrain = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Train"))
        {
            isNearTrain = false;
            train = null;
        }
    }
}
