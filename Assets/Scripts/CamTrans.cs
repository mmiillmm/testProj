using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    public Camera mainCamera;
    public Transform gameplayPosition;
    public float transitionSpeed = 3.0f;
    private bool isTransitioning = false;

    public void OnPlayButtonClicked()
    {
        Debug.Log("megy");
        isTransitioning = true;
    }

    private void Update()
    {
        if (isTransitioning)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, gameplayPosition.position, Time.deltaTime * transitionSpeed);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, gameplayPosition.rotation, Time.deltaTime * transitionSpeed);

            if (Vector3.Distance(mainCamera.transform.position, gameplayPosition.position) < 0.1f)
            {
                isTransitioning = false;
            }
        }
    }
}
