using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float animationBlendSpeed = 9f;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform cam;
    [SerializeField] private float upperLimit = -40f;
    [SerializeField] private float bottomLimit = 70f;
    [SerializeField] private float mouseSensitivity = 5;
    [SerializeField] private GameObject flashlight;
    private CapsuleCollider capsuleCollider;

    private Rigidbody playerRb;
    private InputManager inputManager;
    private Animator animator;
    private bool hasAnimator;
    private bool grounded = true;
    private int xVelHash;
    private int yVelHash;
    private int zVelHash;
    private int jumpHash;
    private int fallingHash;
    private int groundedHash;
    private int crouchHash;
    [SerializeField] private Transform crouchCamTransform;
    private float xRotation; //cam rotation in x axis
    private const float walkSpeedAnim = 2f;
    private const float runSpeedAnim = 4f;
    private Vector2 currentVelocity;
    private float crouchHeight = 1.6f;
    private float standingHeight = 1.79f;
    private Vector3 crouchCenterOffset = new Vector3(0f, 0.76f, 0f);
    private Vector3 standingCenterOffset = new Vector3(0f, 0.89f, 0f);
    [SerializeField] private bool flashlightIsOn = false;

    public bool canMove = true;

    private void Awake() 
    {
        playerRb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        hasAnimator = TryGetComponent<Animator>(out animator);
        capsuleCollider = GetComponent<CapsuleCollider>();

        xVelHash = Animator.StringToHash("X_Velocity");
        yVelHash = Animator.StringToHash("Y_Velocity");
        zVelHash = Animator.StringToHash("Z_Velocity");
        jumpHash = Animator.StringToHash("Jump");
        fallingHash = Animator.StringToHash("Falling");
        groundedHash = Animator.StringToHash("Grounded");
        crouchHash = Animator.StringToHash("Crouch");
    }
    
    private void Start() {
        // initialCameraPosition = cameraRoot.position;
        // crouchedCameraPosition = new Vector3(cameraRoot.position.x, cameraRoot.position.y - 0.6f, cameraRoot.position.z);
    }

    private void Update()
    {
        HandleFlashlight();
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            Move();
            HandleCrouch();
        }
    }

    private void LateUpdate() 
    {
        CameraMovement();
    }
    
    private void HandleCrouch()
    {
        animator.SetBool(crouchHash , inputManager.Crouch);

        if (inputManager.Crouch)
        {
            capsuleCollider.height = crouchHeight;
            capsuleCollider.center = crouchCenterOffset;
        }
        else
        {
            capsuleCollider.height = standingHeight;
            capsuleCollider.center = standingCenterOffset;
        }
    }

    private void Move()
    {
        if (!hasAnimator) return;

        float targetSpeed = inputManager.Run ? runSpeedAnim : walkSpeedAnim;
        if (inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

        Vector3 movement = new Vector3(inputManager.Move.x, 0f, inputManager.Move.y);
        movement = transform.TransformDirection(movement) * targetSpeed * Time.deltaTime * 0.8f;

        Vector3 newPosition = transform.position + movement;

        playerRb.MovePosition(newPosition);

        // Update animator parameters
        animator.SetFloat(xVelHash, inputManager.Move.x * targetSpeed);
        animator.SetFloat(yVelHash, inputManager.Move.y * targetSpeed);
    }

    private void CameraMovement()
    {
        if(!hasAnimator) return;

        float mouseX = inputManager.Look.x * mouseSensitivity * Time.smoothDeltaTime;
        float mouseY = inputManager.Look.y * mouseSensitivity * Time.smoothDeltaTime;
        
        cam.position = cameraRoot.position;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

        cam.localRotation  = Quaternion.Euler(xRotation, 0f, 0f);
        playerRb.MoveRotation(playerRb.rotation * Quaternion.Euler(0f, mouseX, 0f));
        this.transform.Rotate(Vector3.up, mouseX);
    }

    IEnumerator MoveCamera(Transform transform, Vector3 targetPosition, float duration)
    {
        float time = 0.0f;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            float t = time / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    public void SetIdleAnimation()
    {
        if (!hasAnimator) return;

        // Set animator parameters to idle values
        animator.SetFloat(xVelHash, 0f);
        animator.SetFloat(yVelHash, 0f);
    }

    private void HandleFlashlight()
    {
        if(inputManager.Flashlight)
        {
            if(!flashlightIsOn)
            {
                TurnOnFlashlight();
            }
            else{
                TurnOffFlashlight();
            }
        }
    }

    private void TurnOnFlashlight()
    {  
        flashlight.SetActive(true);
        flashlightIsOn = true;
    }

    private void TurnOffFlashlight()
    {  
        flashlight.SetActive(false);
        flashlightIsOn = false;
    }
}
