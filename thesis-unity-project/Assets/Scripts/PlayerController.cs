using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float animationBlendSpeed = 8.9f;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform cam;
    [SerializeField] private float upperLimit = -40f;
    [SerializeField] private float bottomLimit = 70f;
    [SerializeField] private float mouseSensitivity = 21f;
    private Rigidbody playerRb;
    private InputManager inputManager;
    private Animator animator;
    private bool hasAnimator;
    private int xVelHash;
    private int yVelHash;
    private float xRotation; //cam rotation in x axis
    private const float walkSpeedAnim = 1.5f;
    private const float runSpeedAnim = 3f;
    private Vector2 currentVelocity;

    private void Awake() {
        playerRb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        hasAnimator = TryGetComponent<Animator>(out animator);

        xVelHash = Animator.StringToHash("X_Velocity");
        yVelHash = Animator.StringToHash("Y_Velocity");
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
        CameraMovement();
    }

    private void Move()
    {
        if(!hasAnimator) return;

        float targetSpeed = inputManager.Run ? runSpeedAnim : walkSpeedAnim;
        if(inputManager.Move == Vector2.zero) targetSpeed = .1f;

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetSpeed * inputManager.Move.x, animationBlendSpeed * Time.deltaTime);
        currentVelocity.y = Mathf.Lerp(currentVelocity.y, targetSpeed * inputManager.Move.y, animationBlendSpeed * Time.deltaTime);
  
        playerRb.AddForce(transform.right * targetSpeed * inputManager.Move.x, ForceMode.VelocityChange);
        playerRb.AddForce(transform.forward * targetSpeed * inputManager.Move.y, ForceMode.VelocityChange);

        animator.SetFloat(xVelHash, currentVelocity.x);
        animator.SetFloat(yVelHash, currentVelocity.y);
    }

    private void CameraMovement()
    {
        if(!hasAnimator) return;

        float mouseX = inputManager.Look.x * mouseSensitivity * Time.deltaTime;
        float mouseY = inputManager.Look.y * mouseSensitivity * Time.deltaTime;
        
        //for headbob can be disable or not
        cam.position = cameraRoot.position;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

        cam.localRotation  = Quaternion.Euler(xRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up, mouseX * mouseSensitivity * Time.deltaTime);
    }
}
