using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform cam;
    [SerializeField] private float upperLimit = -40f;
    [SerializeField] private float bottomLimit = 70f;
    [SerializeField] private float mouseSensitivity = 5;
    [SerializeField] private GameObject flashlight;
    
    [Header("Step Sound Manager")]
    [SerializeField] private StepsSoundManager stepsSoundManager;
    [SerializeField] private float walkStepSoundInterval = 0.5f;
    [SerializeField] private float runStepSoundInterval = 0.3f; //0.485
    private float stepTimer;
    
    public bool canMove = true;

    private CapsuleCollider capsuleCollider;
    private Rigidbody playerRb;
    private InputManager inputManager;
    private Animator animator;
    private bool hasAnimator;
    private int xVelHash;
    private int yVelHash;
    private int zVelHash;
    private int jumpHash;
    private int fallingHash;
    private int groundedHash;
    private int crouchHash;
    private float xRotation; 
    private const float walkSpeedAnim = 2f;
    private const float runSpeedAnim = 4f;
    private float crouchHeight = 1.6f;
    private float standingHeight = 1.79f;
    private Vector3 crouchCenterOffset = new Vector3(0f, 0.76f, 0f);
    private Vector3 standingCenterOffset = new Vector3(0f, 0.89f, 0f);
    private bool flashlightIsOn = false;

    private void Awake() 
    {
        playerRb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        hasAnimator = TryGetComponent<Animator>(out animator);
        capsuleCollider = GetComponent<CapsuleCollider>();
        stepsSoundManager = GetComponent<StepsSoundManager>();
    }
    
    private void Start() {
        xVelHash = Animator.StringToHash("X_Velocity");
        yVelHash = Animator.StringToHash("Y_Velocity");
        zVelHash = Animator.StringToHash("Z_Velocity");
        jumpHash = Animator.StringToHash("Jump");
        fallingHash = Animator.StringToHash("Falling");
        groundedHash = Animator.StringToHash("Grounded");
        crouchHash = Animator.StringToHash("Crouch");
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

        bool isDiagonal = Mathf.Abs(inputManager.Move.x) > 0.1f && Mathf.Abs(inputManager.Move.y) > 0.1f;

        if (!inputManager.Run && isDiagonal)
        {
            targetSpeed = 2.268f;
        }
        else if (inputManager.Move == Vector2.zero)
        {
            targetSpeed = 0.1f;
        }

        Vector3 movement = new Vector3(inputManager.Move.x, 0f, inputManager.Move.y);
        movement = transform.TransformDirection(movement) * targetSpeed * Time.deltaTime * 0.8f;

        Vector3 newPosition = transform.position + movement;

        playerRb.MovePosition(newPosition);

        float currentXVel = animator.GetFloat(xVelHash);
        float currentYVel = animator.GetFloat(yVelHash);

        float smoothTime = 0.15f;
        float newXVel = Mathf.Lerp(currentXVel, inputManager.Move.x * targetSpeed, Time.deltaTime / smoothTime);
        float newYVel = Mathf.Lerp(currentYVel, inputManager.Move.y * targetSpeed, Time.deltaTime / smoothTime);

        animator.SetFloat(xVelHash, newXVel);
        animator.SetFloat(yVelHash, newYVel);

        // Handle footstep sounds based on movement
        float interval = inputManager.Run ? runStepSoundInterval : walkStepSoundInterval;
        if (inputManager.Move != Vector2.zero)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                stepsSoundManager.PlayStepSound();
                stepTimer = interval;
            }
        }
        else
        {
            stepTimer = interval;
        }
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
