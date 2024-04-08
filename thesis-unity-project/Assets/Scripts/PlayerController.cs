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
    [SerializeField] private bool heabob = true;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float distanceToGround = 0.8f;
    [SerializeField] private LayerMask groundCheck;
    [SerializeField] private float airResistance = 0.8f;
    [SerializeField] private GameObject flashlight;
    private CapsuleCollider capsuleCollider;

    private Rigidbody playerRb;
    private InputManager inputManager;
    private Animator animator;
    private bool hasAnimator;
    private bool grounded;
    private int xVelHash;
    private int yVelHash;
    private int zVelHash;
    private int jumpHash;
    private int fallingHash;
    private int groundedHash;
    private int crouchHash;
    private float xRotation; //cam rotation in x axis
    private const float walkSpeedAnim = 2f;
    private const float runSpeedAnim = 6f;
    private Vector2 currentVelocity;
    private float crouchHeight = 1.6f;
    private float standingHeight = 1.79f;
    private Vector3 crouchCenterOffset = new Vector3(0f, 0.76f, 0f);
    private Vector3 standingCenterOffset = new Vector3(0f, 0.89f, 0f);
    [SerializeField] private bool flashlightIsOn = false;

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

    private void Update()
    {
        HandleFlashlight();
    }

    private void FixedUpdate()
    {
        SampleGround();
        Move();
        HandleJump();
        HandleCrouch();
    }

    private void LateUpdate() 
    {
        CameraMovement();
    }

    private void Move()
    {
        if(!hasAnimator) return;

        float targetSpeed = inputManager.Run ? runSpeedAnim : walkSpeedAnim;
        if(inputManager.Crouch) targetSpeed = 1.5f;
        if(inputManager.Move == Vector2.zero) targetSpeed = .1f;

        if (grounded)
        {
            Vector3 targetVelocity = new Vector3(targetSpeed * inputManager.Move.x, 0, targetSpeed * inputManager.Move.y);

            currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetVelocity.x, animationBlendSpeed * Time.deltaTime);
            currentVelocity.y = Mathf.Lerp(currentVelocity.y, targetVelocity.z, animationBlendSpeed * Time.deltaTime);

            playerRb.AddForce(transform.right * targetSpeed * inputManager.Move.x, ForceMode.VelocityChange);
            playerRb.AddForce(transform.forward * targetSpeed * inputManager.Move.y, ForceMode.VelocityChange);
        }
        else
        {
            playerRb.AddForce(transform.TransformVector(new Vector3(currentVelocity.x * airResistance, 0, currentVelocity.y * airResistance)), ForceMode.VelocityChange);
        }

        animator.SetFloat(xVelHash, currentVelocity.x);
        animator.SetFloat(yVelHash, currentVelocity.y);
    }

    private void CameraMovement()
    {
        if(!hasAnimator) return;

        float mouseX = inputManager.Look.x * mouseSensitivity * Time.smoothDeltaTime;
        float mouseY = inputManager.Look.y * mouseSensitivity * Time.smoothDeltaTime;
        
        //for headbob can be disable or not
        if(heabob) cam.position = cameraRoot.position;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

        cam.localRotation  = Quaternion.Euler(xRotation, 0f, 0f);
        playerRb.MoveRotation(playerRb.rotation * Quaternion.Euler(0f, mouseX, 0f));
        this.transform.Rotate(Vector3.up, mouseX);
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

    private void HandleFlashlight()
    {
        if(inputManager.Flashlight)
        {
            Debug.Log("F is pressed");
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

    private void HandleJump()
    {
        if(!hasAnimator) return;
        if(!inputManager.Jump) return;
        if(!grounded) return;
        animator.SetTrigger(jumpHash);
    }

    private void JumpAddForce()
    {
        playerRb.AddForce(playerRb.velocity.y * Vector3.up, ForceMode.VelocityChange);
        playerRb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        animator.ResetTrigger(jumpHash);
    }

    private void SampleGround()
    {
        if(!hasAnimator) return;

        RaycastHit hitInfo;
        if(Physics.Raycast(playerRb.worldCenterOfMass, Vector3.down, out hitInfo, distanceToGround + 0.1f, groundCheck))
        {
            grounded = true;
            Debug.DrawRay(transform.position, hitInfo.point, Color.green);
            SetAnimationGrounding();
            return;
        }
        // Raycast did not hit anything or falling
        grounded = false;
        animator.SetFloat(zVelHash, playerRb.velocity.y);
        SetAnimationGrounding();
        return;
    }

    private void SetAnimationGrounding()
    {
        animator.SetBool(fallingHash, !grounded);
        animator.SetBool(groundedHash, grounded);
    }
}
