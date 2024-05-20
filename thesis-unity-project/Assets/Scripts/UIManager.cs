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

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        DisableInteractText();
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
}
