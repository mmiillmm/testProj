using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButton : MonoBehaviour
{
    public Camera mainCamera;
    public Transform targetPosition;
    public float cameraMoveSpeed = 2f;
    public CanvasGroup menuCanvasGroup;

    public AudioSource introAudio;
    public AudioSource mainAudio;
    public float audioFadeDuration = 2f;

    private bool isGameStarted = false;

    public MonoBehaviour nextScript;
    public float slowDownDuration = 1f;
    public float shakeIntensity = 0.5f;
    public float shakeSpeed = 10f;

    public void OnStartButtonClicked()
    {
        if (isGameStarted) return;

        isGameStarted = true;

        StartCoroutine(MoveCamera());
        StartCoroutine(FadeOutMenu());
        StartCoroutine(CrossfadeAudio());
    }

    private IEnumerator MoveCamera()
    {
        Vector3 initialPosition = mainCamera.transform.position;
        float totalDistance = Vector3.Distance(initialPosition, targetPosition.position);

        while (Vector3.Distance(mainCamera.transform.position, targetPosition.position) > 0.1f)
        {
            Vector3 target = Vector3.MoveTowards(mainCamera.transform.position, targetPosition.position, cameraMoveSpeed * Time.deltaTime);
            mainCamera.transform.position = target;
            ApplyCameraShake();
            yield return null;
        }

        mainCamera.transform.position = targetPosition.position;
        yield return StartCoroutine(SlowDownAnimation());

        ActivateNextScript();
    }

    private IEnumerator SlowDownAnimation()
    {
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 endPosition = targetPosition.position;
        float timer = 0f;

        while (timer < slowDownDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, timer / slowDownDuration);
            Vector3 interpolatedPosition = Vector3.Lerp(startPosition, endPosition, t);
            mainCamera.transform.position = interpolatedPosition;
            ApplyCameraShake();
            yield return null;
        }

        mainCamera.transform.position = endPosition;
    }

    private void ApplyCameraShake()
    {
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
        float shakeY = Mathf.Cos(Time.time * shakeSpeed) * shakeIntensity;
        mainCamera.transform.rotation = Quaternion.Euler(new Vector3(shakeY, shakeX, 0f));
    }

    private void ActivateNextScript()
    {
        if (nextScript != null)
        {
            nextScript.enabled = true;
        }
    }

    private IEnumerator FadeOutMenu()
    {
        float startAlpha = menuCanvasGroup.alpha;
        float fadeDuration = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            menuCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeDuration);
            yield return null;
        }

        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator CrossfadeAudio()
    {
        float introStartVolume = introAudio.volume;
        float mainStartVolume = mainAudio.volume;

        mainAudio.Play();

        float timer = 0f;

        while (timer < audioFadeDuration)
        {
            timer += Time.deltaTime;
            introAudio.volume = Mathf.Lerp(introStartVolume, 0f, timer / audioFadeDuration);
            mainAudio.volume = Mathf.Lerp(0f, mainStartVolume, timer / audioFadeDuration);

            yield return null;
        }

        introAudio.Stop();
        introAudio.volume = introStartVolume;
    }
}
