using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    public Camera mainCamera;
    public Transform gameplayPosition;
    public float transitionSpeed = 3.0f; // Adjust for noticeable movement speed
    private bool isTransitioning = false;

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked");
        isTransitioning = true;
    }

    private void Update()
    {
        if (isTransitioning)
        {
            // Smoothly move the camera to the gameplay position
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, gameplayPosition.position, Time.deltaTime * transitionSpeed);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, gameplayPosition.rotation, Time.deltaTime * transitionSpeed);

            // Debugging log to track camera position
            Debug.Log("Camera position: " + mainCamera.transform.position);

            // Stop transitioning when camera is close to the target position
            if (Vector3.Distance(mainCamera.transform.position, gameplayPosition.position) < 0.1f)
            {
                isTransitioning = false;
            }
        }
    }
}
