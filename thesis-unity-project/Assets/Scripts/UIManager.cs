using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public bool youDieInProgress = false;
    [SerializeField] private GameObject interactTextObject;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private TMP_Text instructionText;

    [SerializeField] private GameObject blackScreen;
    [SerializeField] private Image blackScreenImage;
    [SerializeField] private GameObject youDieScreen;

    [SerializeField] private MummyAI mummy;
    [SerializeField] private Player player;
    [SerializeField] private PlayerController playerControler;

    private void Awake() {
        Instance = this;
        blackScreenImage = blackScreen.GetComponent<Image>();
    }

    private void Start()
    {
        DisableInteractText();
        blackScreen.SetActive(false);
    }

    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }

    IEnumerator RestartCoroutine()
    {   
        player.currentHits = 0;
        player.cooldownTimer = 0f;
        MenuManager.Instance.menuIsOpen = false;

        Color temp = blackScreenImage.color;
        temp.a = 1f;
        blackScreenImage.color = temp;
        blackScreen.SetActive(true);
        
        playerControler.canMove = false;
        playerControler.canMoveCam = false;

        mummy.ChooseNearestDestinations();
        mummy.currentState = MummyAI.EnemyState.Patrol;
        mummy.chaseTimer = 0;
        
        player.transform.position = player.savePoint.transform.position;

        yield return new WaitForSeconds(1.5f);

        yield return FadeToAlpha(blackScreenImage, 0f, 2f);
        blackScreen.SetActive(false);

        playerControler.canMove = true;
        playerControler.canMoveCam = true;

        youDieInProgress = false;
    }

    public void ChangeInstructionText(string text)
    {
        instructionText.text = text;
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
        youDieInProgress = true;
        blackScreen.SetActive(true);
        yield return FadeToAlpha(blackScreenImage, 1f, 1f);
        youDieScreen.SetActive(true);
        yield return FadeToAlpha(blackScreenImage, 0f, 1f);
        blackScreen.SetActive(false);
        MenuManager.Instance.menuIsOpen = true;
    }

    public IEnumerator BlackScreenFadeInCoroutine(float duration)
    {
        blackScreen.SetActive(true);
        yield return FadeToAlpha(blackScreenImage, 1f, duration);
    }

    public IEnumerator BlackScreenFadeOutCoroutine(float duration)
    {
        yield return FadeToAlpha(blackScreenImage, 0f, duration);
        blackScreen.SetActive(false);
    }

    public IEnumerator FadeToAlpha(Image img, float targetAlpha, float duration)
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
