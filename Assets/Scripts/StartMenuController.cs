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
        while (Vector3.Distance(mainCamera.transform.position, targetPosition.position) > 0.1f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPosition.position, cameraMoveSpeed * Time.deltaTime);
            yield return null;
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
