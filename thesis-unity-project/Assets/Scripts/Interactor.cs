using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactRange;
    private InputManager inputManager;
    private Transform interactorSource;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        interactorSource = this.gameObject.transform;
    }

    void Update()
    {
        if(inputManager.Interact)
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if(Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
