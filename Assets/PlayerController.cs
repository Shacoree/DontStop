using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
public class PlayerController : MonoBehaviour
{
    
    private CharacterController playerController;
    private Camera playerCamera;
    private Transform playerTransform;
    [SerializeField]
    private InputActionReference movement, pointerPosition, jump;

    private Vector2 lastMousePos;

    private float rotationX, rotationY;

    private Vector3 playerVelocity;

    private Animator playerAnimator;



    [Header("Physics")]
    public float gravity = 9.81f;
    public float jumpImpulse = 6.0f;
    public float maxGroundSpeed = 5.0f;
    public float groundAcceleration = 12.0f;
    public float airAcceleration = 40.0f;
    public float maxAirSpeed = 1.5f;
    public float groundDeceleration = 3.0f;
    public float friction = 5.0f;
    public float airDeceleration = 2.0f;


    private Vector3 currentRotation;
    private Vector3 smoothCameraVelocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        playerCamera = GetComponentInChildren<Camera>();
        playerController = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponentInChildren<Animator>();
        lastMousePos = pointerPosition.action.ReadValue<Vector2>();
        rotationX = 0.0f;
        rotationY = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mouseDelta = GetMouseDeltaInput();

        
        rotationX = Mathf.Clamp(rotationX + mouseDelta.y, -90f, 90f);
        rotationY -= mouseDelta.x;


        
        playerTransform.localRotation = Quaternion.Euler(0, rotationY, 0);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

    }
    void FixedUpdate()
    {
        Vector2 movementInput = movement.action.ReadValue<Vector2>();

        if (movementInput.y >= 1.0f)
        {
            playerAnimator.SetBool("Running", true);
        }
        else
        {
            playerAnimator.SetBool("Running", false);
        }
        

        Vector3 wishDir = playerController.transform.forward * movementInput.y + playerController.transform.right * movementInput.x;
        if (wishDir.sqrMagnitude > 1f)
            wishDir.Normalize();


        if (playerController.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -1f;

        // Jump input
        if (jump.action.ReadValue<float>() == 1 && playerController.isGrounded)
        {
            playerVelocity.y = jumpImpulse;
        }

        float deltaTime = Time.deltaTime;

        // Apply friction
        if (playerController.isGrounded)
        {
            float speed = new Vector3(playerVelocity.x, 0, playerVelocity.z).magnitude;
            float control = Mathf.Max(speed, groundDeceleration);
            float drop = control * friction * deltaTime;
            float newSpeed = Mathf.Max(0, speed - drop);
            if (speed > 0)
            {
                float scale = newSpeed / speed;
                playerVelocity.x *= scale;
                playerVelocity.z *= scale;
            }
        }

        // Acceleration
        float wishSpeed = playerController.isGrounded ? maxGroundSpeed : maxAirSpeed;
        float accel = playerController.isGrounded ? groundAcceleration : airAcceleration;

        Vector3 wishVel = wishDir * wishSpeed;
        Vector3 velXZ = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        Vector3 accelVel = Accelerate(velXZ, wishDir, wishSpeed, accel, deltaTime);
        playerVelocity.x = accelVel.x;
        playerVelocity.z = accelVel.z;

        // Gravity
        if (!playerController.isGrounded)
        {
            playerVelocity.y -= gravity * deltaTime;
        }

        playerController.Move(playerVelocity * Time.fixedDeltaTime);
    }
    Vector3 Accelerate(Vector3 vel, Vector3 wishdir, float wishspeed, float accel, float dt)
    {
        float curspeed = Vector3.Dot(vel, wishdir);
        float addspeed = wishspeed - curspeed;
        if (addspeed <= 0) return vel;

        float accelspeed = accel * wishspeed * dt;
        if (accelspeed > addspeed)
            accelspeed = addspeed;

        vel += accelspeed * wishdir;
        return vel;
    }
    private Vector2 GetMouseDeltaInput()
    {
        Vector2 mousePos = pointerPosition.action.ReadValue<Vector2>();
        Vector2 delta = lastMousePos - mousePos;
        lastMousePos = mousePos;

        return delta;
    }    
}
