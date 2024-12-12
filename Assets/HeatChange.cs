using UnityEngine;
using UnityEngine.UI;

public class HeatChange : MonoBehaviour
{
    [SerializeField] float heat = 0f;
    [SerializeField] Slider slider;
    [SerializeField] bool isHeating = false;
    [SerializeField] float coolingRate = 0.5f;
    [SerializeField] float slowCoolingRate = 0.2f;
    [SerializeField] float fastCoolingRate = 1.0f;
    [SerializeField] Button walkButton;
    [SerializeField] Button runButton;
    [SerializeField] Button stopButton;
    [SerializeField] Color normalColor = Color.green;
    [SerializeField] Color overheatColor = Color.red;
    [SerializeField] Color cooldownColor = Color.blue;
    [SerializeField] Color medheatColor = Color.yellow;
    [SerializeField] float colorChangeSpeed = 0.5f;

    private Image fillImage;

    void Start()
    {
        if (slider != null)
        {
            slider.value = heat;
            fillImage = slider.fillRect.GetComponent<Image>();
            fillImage.color = normalColor;
        }

        if (walkButton != null)
        {
            walkButton.onClick.AddListener(StartWalking);
        }

        if (runButton != null)
        {
            runButton.onClick.AddListener(StartRunning);
        }

        if (stopButton != null)
        {
            stopButton.onClick.AddListener(Stop);
        }
    }

    void Update()
    {
        if (isHeating)
        {
            if (slider != null)
            {
                slider.value += ((Time.deltaTime) * .5f);
            }
        }
        else
        {
            if (slider != null && slider.value > 0)
            {
                slider.value -= coolingRate * Time.deltaTime;
            }
        }

        if (slider != null)
        {
            if (!isHeating)
            {
                fillImage.color = Color.Lerp(fillImage.color, cooldownColor, colorChangeSpeed * Time.deltaTime);
            }
            else if (slider.value >= slider.maxValue * 0.75f)
            {
                fillImage.color = Color.Lerp(fillImage.color, overheatColor, colorChangeSpeed * Time.deltaTime);
            }
            else if (slider.value >= slider.maxValue * 0.50f)
            {
                fillImage.color = Color.Lerp(fillImage.color, medheatColor, colorChangeSpeed * Time.deltaTime);
            }
            else
            {
                fillImage.color = Color.Lerp(fillImage.color, normalColor, colorChangeSpeed * Time.deltaTime);
            }
        }
    }

    public void StartWalking()
    {
        isHeating = false;
        coolingRate = slowCoolingRate;
    }

    public void StartRunning()
    {
        isHeating = true;
    }

    public void Stop()
    {
        isHeating = false;
        coolingRate = fastCoolingRate;
    }
}
