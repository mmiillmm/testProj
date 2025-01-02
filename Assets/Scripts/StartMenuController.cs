using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public CanvasGroup startMenuCanvasGroup; // Reference to CanvasGroup for fading
    public Transform cameraTarget;          // Target position for the camera
    public float cameraMoveSpeed = 2f;      // Speed of the camera movement
    public float fadeDuration = 1f;         // Duration of the fade
    public Camera mainCamera;               // Reference to the camera to move

    private bool shouldMoveCamera = false;  // Tracks if the camera should move
    private bool isFading = false;          // Tracks if the menu is fading
    private Vector3 velocity = Vector3.zero; // Used for SmoothDamp

    private void Update()
    {
        if (isFading)
        {
            startMenuCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
            if (startMenuCanvasGroup.alpha <= 0)
            {
                startMenuCanvasGroup.gameObject.SetActive(false);
                isFading = false;
                shouldMoveCamera = true;
            }
        }

        if (shouldMoveCamera)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(
                mainCamera.transform.position,
                cameraTarget.position,
                ref velocity,
                cameraMoveSpeed
            );

            if (Vector3.Distance(mainCamera.transform.position, cameraTarget.position) < 0.01f)
            {
                shouldMoveCamera = false;
                Debug.Log("Camera reached the target position!");
            }
        }
    }

    public void StartGame()
    {
        Debug.Log("Boom! Button clicked. Starting game...");
        isFading = true; // Start the fade-out process
    }
}
