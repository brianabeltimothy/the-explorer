using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    public bool isOpen = false;
    private bool isAnimating = false; // Flag to track if an animation is in progress
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        // Prevent interaction if an animation is currently in progress
        if (isAnimating) return;

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        isAnimating = true;
        animator.ResetTrigger("Open");
        animator.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        isOpen = false;
        isAnimating = true;
        animator.ResetTrigger("Close");
        animator.SetTrigger("Close");
    }

    public void OnAnimationComplete()
    {
        isAnimating = false;
    }
}
