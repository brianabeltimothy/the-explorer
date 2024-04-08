using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;

    [SerializeField] private Material defaultMaterial;
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
            var selectionRenderer = selectedObject.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            selectedObject = null;
        }

        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(r, out hitInfo, interactRange))
        {
            if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                var selection = hitInfo.transform;
                var selectionRenderer = selection.GetComponent<Renderer>();
                if(selectionRenderer != null){
                    defaultMaterial = selectionRenderer.material;
                    selectionRenderer.material = highlightMaterial;
                }

                selectedObject = selection;

                if(inputManager.Interact)
                {
                    interactObj.Interact();
                }
            }
        }
    }
}