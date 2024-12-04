using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTextOnEnter : MonoBehaviour
{
    public TMP_Text text;
    public RectTransform background;
    public Vector2 padding = new Vector2(20, 20);
    public float typeSpeed = 0.05f;
    private bool isPlayerInRange = false;
    private string fullText = "";

    private void Start()
    {
        text.enabled = false;
        background.gameObject.SetActive(false);
        fullText = text.text;
        text.text = "";
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
            isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            isPlayerInRange = false;
            text.enabled = false;
            background.gameObject.SetActive(false);
            StopAllCoroutines();
            text.text = "";
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            text.enabled = true;
            background.gameObject.SetActive(true);
            StartCoroutine(TypeText());
        }
    }

    private void UpdateBackgroundSize()
    {
        Vector2 textSize = text.GetRenderedValues(false);
        background.sizeDelta = textSize + padding;
    }

    IEnumerator TypeText()
    {
        text.text = "";
        foreach (char c in fullText)
        {
            text.text += c;
            UpdateBackgroundSize();
            yield return new WaitForSeconds(typeSpeed);
        }
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        text.enabled = false;
        background.gameObject.SetActive(false);
        text.text = "";
    }
}
