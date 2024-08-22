using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyController : MonoBehaviour, IInteractable
{
    private Player player;
    private string instruction = "[E] <br> Collect";

    private void Awake() 
    {
        player = FindAnyObjectByType<Player>();
    }

    public void Interact()
    {
        player.hasDoorKey = true;
        Destroy(gameObject);
        StartCoroutine(DisplayInstructionText());
    }

    public string GivesInstructionText()
    {
        return instruction;
    }

    private IEnumerator DisplayInstructionText()
    {
        UIManager.Instance.ChangeInstructionText("");
        UIManager.Instance.ChangeInstructionText("The key of Tyet. This should unlock the door.");
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ChangeInstructionText("");
    }
}
