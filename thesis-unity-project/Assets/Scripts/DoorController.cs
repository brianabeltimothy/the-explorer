using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    private bool open = false;
    private bool isAnimating = false; // Flag to track if an animation is in progress
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        // Prevent interaction if an animation is currently in progress
        if (isAnimating) return;

        if (open)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        open = true;
        isAnimating = true; // Set the flag to true when starting an animation
        animator.ResetTrigger("Open");
        animator.SetTrigger("Open");
    }

    private void CloseDoor()
    {
        open = false;
        isAnimating = true; // Set the flag to true when starting an animation
        animator.ResetTrigger("Close");
        animator.SetTrigger("Close");
    }

    // This method will be called as an animation event at the end of each animation
    public void OnAnimationComplete()
    {
        Debug.Log("animation done");
        isAnimating = false; // Reset the flag when the animation is complete
    }
}
