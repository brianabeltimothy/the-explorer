using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    public bool isOpen = false;
    private bool isMoving = false;
    private Animator animator;
    private AudioSource audioSource;
    private string instruction = "[E] <br> Open";

    [SerializeField] private float xTarget; 
    [SerializeField] private float yTarget; 
    [SerializeField] private float zTarget; 

    private void Awake() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (isMoving) return;

        if (isOpen)
        {
            CloseDoor();
            instruction = "[E] <br> Open";
        }
        else
        {
            OpenDoor();
            instruction = "[E] <br> Close";
        }
    }

    public string GivesInstructionText()
    {
        return instruction;
    }

    public void OpenDoor()
    {
        isOpen = true;
        StartCoroutine(OpenDoorCoroutine());
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.8f, 1.0f);
        audioSource.Play();
    }

    public void CloseDoor()
    {
        isOpen = false;
        StartCoroutine(CloseDoorCoroutine());
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.8f, 1.0f);
        audioSource.Play();
    }

    IEnumerator OpenDoorCoroutine()
    {
        isMoving = true;
        Vector3 startPosition = this.transform.position;
        Vector3 targetPosition = new Vector3 (this.transform.position.x + xTarget, this.transform.position.y + yTarget, this.transform.position.z + zTarget);
        float elapsedTime = 0f;
        float duration = 0.85f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.position = targetPosition;
        isMoving = false;
    }

    IEnumerator CloseDoorCoroutine()
    {
        isMoving = true;
        Vector3 startPosition = this.transform.position;
        Vector3 targetPosition = new Vector3 (this.transform.position.x - xTarget, this.transform.position.y - yTarget, this.transform.position.z - zTarget);
        float elapsedTime = 0f;
        float duration = 0.85f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.position = targetPosition;
        isMoving = false;
    }
}
