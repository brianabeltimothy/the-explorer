using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingChestController : MonoBehaviour, IInteractable
{
    private MummyAI mummyAIScript;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform initialTransform;
    [SerializeField] private float duration = 1.0f;

    [SerializeField] private BoxCollider boxCollider; 

    public bool isOpen = false;
    private bool isAnimating = false;
    private Animator animator;
    private bool isMoving = false;

    private Player player;
    private PlayerController playerController;

    private void Awake() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        player = playerTransform.GetComponent<Player>();
        playerController = playerTransform.GetComponent<PlayerController>();
    }

    public void Interact()
    {
        if (!player.isHiding)
        {
            Hide();
        }
        else
        {
            Out();
        }
    }

    public void Hide()
    {
        playerController.SetIdleAnimation();
        if (!isMoving)
        {
            StartCoroutine(HideCoroutine(targetTransform.position, duration));
            player.isHiding = true;
            playerController.canMove = false;
        }
    }

    public void Out()
    {
        if (!isMoving)
        {
            StartCoroutine(OutCoroutine(duration));
            player.isHiding = false;
            playerController.canMove = true;
        }
    }

    private IEnumerator HideCoroutine(Vector3 targetPosition, float duration)
    {
        boxCollider.enabled = false;
        OpenDoor();
        yield return new WaitUntil(() => !isAnimating);
        yield return StartCoroutine(MoveToPosition(targetPosition, duration));
        CloseDoor();
        boxCollider.enabled = true;
    }

    private IEnumerator OutCoroutine(float duration)
    {
        boxCollider.enabled = false;
        OpenDoor();
        yield return new WaitUntil(() => !isAnimating);
        yield return StartCoroutine(MoveUpAndOut(duration));
        CloseDoor();
        boxCollider.enabled = true;
    }

    private IEnumerator MoveUpAndOut(float duration)
    {
        isMoving = true;
        Vector3 startPosition = playerTransform.position;
        Vector3 upPosition = new Vector3(startPosition.x, startPosition.y + 1, startPosition.z);
        float halfDuration = duration / 2f;
        float elapsedTime = 0f;

        while (elapsedTime < halfDuration)
        {
            float t = elapsedTime / halfDuration;
            playerTransform.position = Vector3.Lerp(startPosition, upPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerTransform.position = upPosition;

        elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            float t = elapsedTime / halfDuration;
            playerTransform.position = Vector3.Lerp(upPosition, initialTransform.position, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.position = initialTransform.position;
        isMoving = false;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        isMoving = true;
        Vector3 startPosition = playerTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.position = targetPosition;
        isMoving = false;
    }

    private void OpenDoor()
    {
        isOpen = true;
        isAnimating = true;
        animator.ResetTrigger("Open");
        animator.SetTrigger("Open");
    }

    private void CloseDoor()
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
