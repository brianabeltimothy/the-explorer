using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour, IInteractable
{
    private Player player;
    private string instruction = "[E] <br> Collect";

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Interact()
    {
        player.hasMask = true;
        Destroy(this.gameObject);
        StartCoroutine(DisplayInstructionText());
    }

    public string GivesInstructionText()
    {
        return instruction;
    }

    private IEnumerator DisplayInstructionText()
    {
        UIManager.Instance.ChangeInstructionText("");
        UIManager.Instance.ChangeInstructionText("Here it is. The missing mask that I looked for.");
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ChangeInstructionText("");
    }
}
