using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueMask : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject mask;
    [SerializeField] private Transform targetTransform;

    private Player player;
    private BoxCollider boxCollider;
    private string instruction = "[E] <br> Interact";
    
    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        boxCollider = GetComponent<BoxCollider>();
        mask.SetActive(false);
    }

    private void Update() 
    {
        if (player.hasMask)
        {
            instruction = "[E] <br> Put mask";
            //game over
        }
    }

    public void Interact()
    {
        if (player.hasMask)
        {
            StartCoroutine(PutMask());
            //game over
        }
        else
        {
            StartCoroutine(DisplayInstructionText());
        }
    }

    private IEnumerator PutMask()
    {
        boxCollider.enabled = false;
        mask.SetActive(true);
        yield return new WaitForSeconds(1f);
        Vector3 startPosition = mask.transform.position;
        Vector3 targetPosition = targetTransform.position;
        float elapsedTime = 0f;
        float duration = 1.5f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            mask.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mask.transform.position = targetPosition;
    }

    public string GivesInstructionText()
    {
        return instruction;
    }

    private IEnumerator DisplayInstructionText()
    {
        UIManager.Instance.ChangeInstructionText("");
        UIManager.Instance.ChangeInstructionText("Something is missing here... What is it?");
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ChangeInstructionText("");
    }
}
