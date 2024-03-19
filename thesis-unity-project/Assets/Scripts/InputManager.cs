using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public Vector2 Move {get; private set;}
    public Vector2 Look {get; private set;}
    public bool Run {get; private set;}
    public bool Jump {get; private set;}
    public bool Crouch {get; private set;}
    public bool Inventory {get; private set;}
    public bool Interact {get; private set;}

    private InputActionMap currentMap;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction inventoryAction;
    private InputAction interactAction;

    private void Start() {
        currentMap = playerInput.currentActionMap;
        moveAction = currentMap.FindAction("Move");
        lookAction = currentMap.FindAction("Look");
        runAction = currentMap.FindAction("Run");
        jumpAction = currentMap.FindAction("Jump");
        crouchAction = currentMap.FindAction("Crouch");
        inventoryAction = currentMap.FindAction("Inventory");
        interactAction = currentMap.FindAction("Interact");

        moveAction.performed += onMove;
        lookAction.performed += onLook;
        runAction.performed += onRun;
        jumpAction.performed += onJump;
        crouchAction.performed += onCrouch;
        inventoryAction.performed += onInventory;
        interactAction.performed += onInteract;

        moveAction.canceled += onMove;
        lookAction.canceled += onLook;
        runAction.canceled += onRun;
        jumpAction.canceled += onJump;
        crouchAction.canceled += onCrouch;
        inventoryAction.canceled += onInventory;
        interactAction.canceled += onInteract;
    }

    private void onMove(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }
    private void onLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }
    private void onRun(InputAction.CallbackContext context)
    {
        Run = context.ReadValueAsButton();
    }
    private void onJump(InputAction.CallbackContext context)
    {
        Jump = context.ReadValueAsButton();
    }
    private void onCrouch(InputAction.CallbackContext context)
    {
        Crouch = context.ReadValueAsButton();
    }
    private void onInventory(InputAction.CallbackContext context)
    {
        Inventory = context.ReadValueAsButton();
    }
    private void onInteract(InputAction.CallbackContext context)
    {
        Interact = context.ReadValueAsButton();
    }

    private void LateUpdate()
    {
        Inventory = false;
    }

    private void OnEnable() {
        currentMap.Enable();
    }

    private void OnDisable() {
        currentMap.Disable();
    }
}
