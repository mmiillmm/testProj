using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    private Light pointLight;
    private float targetIntensity;
    private float currentIntensity;

    void Start()
    {
        pointLight = GetComponent<Light>();
        currentIntensity = pointLight.intensity;
        SetNewTargetIntensity();
    }

    void Update()
    {
        currentIntensity = Mathf.MoveTowards(currentIntensity, targetIntensity, flickerSpeed * Time.deltaTime);
        pointLight.intensity = currentIntensity;

        if (Mathf.Approximately(currentIntensity, targetIntensity))
        {
            SetNewTargetIntensity();
        }
    }

    void SetNewTargetIntensity()
    {
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }
}
