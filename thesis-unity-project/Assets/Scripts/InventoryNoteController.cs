using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryNoteController : MonoBehaviour
{
    [SerializeField] private Image noteImage;
    [SerializeField] private TMP_Text noteText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    
    [SerializeField] public List<Item> notes = new List<Item>();

    
    private int currentNoteIndex = 0;
    private Item currentNote;

    private void OnEnable() {
        this.notes = InventoryManager.Instance.notes;

        if(notes.Count == 0)
        {
            nextButton.SetActive(false);
            previousButton.SetActive(false);
        }
        else if(notes.Count == 1)
        {
            ShowPage();
        }
        else
        {
            nextButton.SetActive(true);
            previousButton.SetActive(true);
        }
    }

    public void NextPage()
    {
        currentNoteIndex ++;
        
        if(currentNoteIndex > notes.Count - 1)
        {
            currentNoteIndex = 0;
        }

        SetCurrentNote();
    }

    public void PreviousPage()
    {
        currentNoteIndex--;

        if(currentNoteIndex < 0)
        {
            currentNoteIndex = notes.Count - 1;
        }

        SetCurrentNote();
    }

    public void ShowPage()
    {
        noteImage.sprite = notes[currentNoteIndex].icon;
        noteText.text = notes[currentNoteIndex].description;
    }

    public void SetCurrentNote()
    {
        if (notes[currentNoteIndex] != null)
        {
            currentNote = notes[currentNoteIndex];
            ShowPage();           
        }
        else
        {
            Debug.LogWarning("currentNote is null.");
        }
    }
}
