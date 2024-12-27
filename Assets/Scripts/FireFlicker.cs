using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;
    public float minDelay = 0.1f;
    public float maxDelay = 0.5f;

    private Light pointLight;
    private float targetIntensity;
    private float currentIntensity;

    void Start()
    {
        pointLight = GetComponent<Light>();
        currentIntensity = pointLight.intensity;
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            while (!Mathf.Approximately(currentIntensity, targetIntensity))
            {
                currentIntensity = Mathf.MoveTowards(currentIntensity, targetIntensity, flickerSpeed * Time.deltaTime);
                pointLight.intensity = currentIntensity;
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
