using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    public Camera mainCamera;
    public Transform targetPosition;
    public Transform gamePosition;
    public float moveSpeed = 3f;
    public float rotationSpeed = 3f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool inSettings = false;

    void Start()
    {
        originalPosition = mainCamera.transform.position;
        originalRotation = mainCamera.transform.rotation;
    }

    public void MoveToSettings()
    {
        if (!inSettings)
        {
            StopAllCoroutines();
            StartCoroutine(MoveCamera(targetPosition.position, targetPosition.rotation));
            inSettings = true;
        }
    }

    public void MoveBackToGame()
    {
        if (inSettings)
        {
            StopAllCoroutines();
            StartCoroutine(MoveCamera(originalPosition, originalRotation));
            inSettings = false;
        }
    }

    public void StartGame()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamera(gamePosition.position, gamePosition.rotation));
    }

    public void CloseGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator MoveCamera(Vector3 destination, Quaternion targetRotation)
    {
        while (Vector3.Distance(mainCamera.transform.position, destination) > 0.1f ||
               Quaternion.Angle(mainCamera.transform.rotation, targetRotation) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        mainCamera.transform.position = destination;
        mainCamera.transform.rotation = targetRotation;
    }
}
