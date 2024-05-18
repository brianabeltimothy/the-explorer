using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    // [SerializeField] private Material highlightMaterial;

    // [SerializeField] private Material defaultMaterial;
    private Transform selectedObject;
    private InputManager inputManager;
    private Transform interactorSource;
    [SerializeField] private float interactRange;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
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
                }
            }
        }
    }
}