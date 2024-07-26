using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour, IInteractable
{
    private Animator animator;
    private BoxCollider boxCollider;
    private AudioSource audioSource;

    private void Awake() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        OpenChest();
    }

    private void OpenChest()
    {
        animator.SetTrigger("Open");
        boxCollider.enabled = false;
    }

    private void PlaySound()
    {
        audioSource.Play();
    }
}
