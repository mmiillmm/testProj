using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnEnter : MonoBehaviour
{
    public GameObject text;  // The text UI element to display
    private bool isPlayerInRange = false; // Flag to track if the player is inside the trigger area

    private void Start()
    {
        text.SetActive(false);  // Ensure the text is hidden at the start
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            isPlayerInRange = true; // Player entered the trigger area
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            isPlayerInRange = false; // Player exited the trigger area
            text.SetActive(false);   // Hide text if the player leaves the trigger area
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) // Check if player is in range and presses "E"
        {
            text.SetActive(true); // Show the text when "E" is pressed
            StartCoroutine(Wait()); // Start the countdown to hide the text after a delay
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        text.SetActive(false); // Hide the text after the wait time
    }
}
