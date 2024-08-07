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
    }

    public string GivesInstructionText()
    {
        return instruction;
    }
}
