using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject interactTextObject;
    [SerializeField] private TMP_Text interactText;

    [SerializeField] private GameObject blackScreen;
    [SerializeField] private Image blackScreenImage;
    [SerializeField] private GameObject youDieScreen;

    private void Awake() {
        Instance = this;
        blackScreenImage = blackScreen.GetComponent<Image>();
    }

    private void Start()
    {
        DisableInteractText();
        blackScreen.SetActive(false);
    }

    public void ChangeInteractText(string text)
    {
        interactText.text = text;
    }

    public void EnableInteractText()
    {
        interactText.enabled = true;
    }

    public void DisableInteractText()
    {
        interactText.enabled = false;
    }

    public IEnumerator ShowGameOverScreen()
    {
        blackScreen.SetActive(true);
        yield return FadeToAlpha(blackScreenImage, 1f, 2f);
        youDieScreen.SetActive(true);
        yield return FadeToAlpha(blackScreenImage, 0f, 2f);
        blackScreen.SetActive(false);
    }

    IEnumerator FadeToAlpha(Image img, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = img.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (elapsedTime < duration)
        {
            img.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        img.color = targetColor;
    }
}
