using UnityEngine;
using UnityEngine.UI;

public class KeepRawImageEnabled : MonoBehaviour
{
    private RawImage rawImage;

    private void Awake()
    {
        // Get the RawImage component on the GameObject
        rawImage = GetComponent<RawImage>();

        // Log an error if the RawImage is missing
        if (rawImage == null)
        {
            Debug.LogError("RawImage component is missing on this GameObject.");
        }
    }

    private void Update()
    {
        // Ensure the RawImage is always enabled
        if (rawImage != null && !rawImage.enabled)
        {
            rawImage.enabled = true;
            Debug.Log("RawImage was disabled and has been re-enabled.");
        }
    }
}
