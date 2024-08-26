using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject allKeys;
    [SerializeField] private Transform targetKeyTransform;
    private string instruction = "[E] <br> Interact";
    private AudioSource audioSource;

    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if(InventoryManager.Instance.KeyItems.Count == 3)
        {
            StartCoroutine(PutKey());
        }
        else
        {
            StartCoroutine(DisplayInstructionText());
        }
    }

    public string GivesInstructionText()
    {
        return instruction;
    }

    private IEnumerator DisplayInstructionText()
    {
        UIManager.Instance.ChangeInstructionText("");
        UIManager.Instance.ChangeInstructionText("This looks like a gate. I need to find three relics to open it.");
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ChangeInstructionText("");
    }

    private IEnumerator PutKey()
    {
        allKeys.SetActive(true);
        yield return new WaitForSeconds(1f);
        Vector3 startPosition = allKeys.transform.position;
        Vector3 targetPosition = targetKeyTransform.position;
        float elapsedTime = 0f;
        float duration = 1.5f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            allKeys.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        allKeys.transform.position = targetPosition;
        
        yield return new WaitForSeconds(1f);

        // open the gate
        audioSource.Play();
        startPosition = this.transform.position;
        targetPosition = new Vector3(this.transform.position.x, this.transform.position.y + 3, this.transform.position.z);
        elapsedTime = 0f;
        duration = 2f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = targetPosition;
        audioSource.Stop();
    }
}
