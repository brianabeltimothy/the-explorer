using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    private Transform selectedObject;
    private InputManager inputManager;
    private Transform interactorSource;
    [SerializeField] private float interactRange;
    private AudioSource audioSource;

    private void Awake() 
    {
        inputManager = FindObjectOfType<InputManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        interactorSource = this.gameObject.transform;
    }

    void Update()
    {
        if(selectedObject != null)
        {
            UIManager.Instance.DisableInteractText();
            selectedObject = null;
        }

        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(r, out hitInfo, interactRange))
        {
            if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                var selection = hitInfo.transform;
                UIManager.Instance.EnableInteractText();

                selectedObject = selection;

                if(inputManager.Interact)
                {
                    interactObj.Interact();
                    UIManager.Instance.DisableInteractText();
                    ItemController itemController = hitInfo.collider.gameObject.GetComponent<ItemController>();
                    if (itemController != null)
                    {
                        audioSource.Play();
                    }
                }
            }
        }
    }
}