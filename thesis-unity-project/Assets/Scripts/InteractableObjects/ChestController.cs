using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour, IInteractable
{
    private Animator animator;
    private BoxCollider boxCollider;
    private AudioSource audioSource;
    [SerializeField] private GameObject chestLid;
    private Vector3 targetRotation = new Vector3(100, 0, 0); 

    private void Awake() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        Transform secondChildTransform = transform.GetChild(1);
        chestLid = secondChildTransform.gameObject;
    }

    public void Interact()
    {
        OpenChest();
    }

    private void OpenChest()
    {
        // animator.SetTrigger("Open");
        StartCoroutine(OpenChestCoroutine());
        boxCollider.enabled = false;
        audioSource.Play();
    }

    IEnumerator OpenChestCoroutine()
    {
        Quaternion initialRotation = chestLid.transform.rotation;
        Quaternion finalRotation = Quaternion.Euler(targetRotation);
        float elapsedTime = 0f;
        float duration = 0.85f;

        while (elapsedTime < duration)
        {
            chestLid.transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestLid.transform.rotation = finalRotation;
    }
}
