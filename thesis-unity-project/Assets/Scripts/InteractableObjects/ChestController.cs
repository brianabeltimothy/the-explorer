using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour, IInteractable
{
    private Animator animator;
    private BoxCollider boxCollider;

    private void Awake() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact()
    {
        OpenChest();
    }

    private void OpenChest()
    {
        animator.ResetTrigger("Open");
        animator.SetTrigger("Open");
        boxCollider.enabled = false;
    }
}
