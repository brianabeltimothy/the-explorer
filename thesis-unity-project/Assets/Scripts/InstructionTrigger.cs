using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    [SerializeField] private string InstructionText;
    [SerializeField] private bool destroyAfterDisplay = false;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisplayInstructionText());
        }
    }

    private IEnumerator DisplayInstructionText()
    {
        UIManager.Instance.ChangeInstructionText("");
        UIManager.Instance.ChangeInstructionText(InstructionText);
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ChangeInstructionText("");

        if(destroyAfterDisplay)
        {
            Destroy(gameObject);
        }
    }
}
