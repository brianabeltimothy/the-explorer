using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyController : MonoBehaviour, IInteractable
{
    private Player player;
    private AudioSource audioSource;

    private void Awake() 
    {
        player = FindAnyObjectByType<Player>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        audioSource.Play();
        player.hasDoorKey = true;
        Destroy(gameObject);
    }
}
