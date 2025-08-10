using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private InputActionReference movement, pointerPosition, jump, attack, menu;

    

    private CharacterController playerController;
    private Camera playerCamera;
    private Transform playerTransform;
    private Animator playerAnimator;

    public static bool lowFood = false;
    public static bool highFood = false;

    private Vector2 lastMousePos;
    private float rotationX, rotationY;

    private static bool playerStartedMoving;

    public static bool PlayerStartedMoving
    {
        get { return playerStartedMoving; }
        set { playerStartedMoving = value; }
    }

    //Attack
    private float attackCooldownTimer = 1.0f;
    private float attackCooldown = 1.0f;


    private static Vector3 playerVelocity;
    private float playerSensitivity;

    public static float playerDamage = 30.0f;

    public static Vector3 PlayerVelocity
    {
        get { return playerVelocity; }
        set { playerVelocity = value; }
    }

    [Header("Physics")]
    public float gravity = 9.81f;
    public static float jumpImpulse = 6.0f;
    public float maxGroundSpeed = 5.0f;
    public float groundAcceleration = 12.0f;
    public float airAcceleration = 40.0f;
    public float maxAirSpeed = 1.5f;
    public float groundDeceleration = 3.0f;
    public float friction = 5.0f;
    public float airDeceleration = 2.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        playerCamera = GetComponentInChildren<Camera>();
        playerController = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponentInChildren<Animator>();

        lastMousePos = pointerPosition.action.ReadValue<Vector2>();

        playerStartedMoving = false;
        lowFood = false;
        highFood = false;

        playerVelocity = Vector3.zero;

        playerSensitivity = PlayerPrefs.GetFloat(SettingsMenu.prefsSensitivity);

        rotationX = 0.0f;
        rotationY = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerCamera();
    }
    void FixedUpdate()
    {
        HandleAttack();
        HandlePlayerMovement();
        HandleMenuOpen();
    }
    

    private void HandleAttack()
    {
        attackCooldownTimer += Time.fixedDeltaTime;
        attack.action.performed += Attack;
    }
    private void Attack(InputAction.CallbackContext context)
    {
        if (attackCooldownTimer > attackCooldown)
        {
            attackCooldownTimer = 0.0f;
            if (playerAnimator.IsDestroyed() && playerAnimator == null) return;

            playerAnimator.SetTrigger("Axe_Swing");
        }
    }
    
    private void HandlePlayerCamera()
    {
        Vector2 mouseDelta = GetMouseDeltaInput();

        mouseDelta.x *= playerSensitivity;
        mouseDelta.y *= playerSensitivity;

        // Invert Y axis 
        rotationX -= mouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        rotationY += mouseDelta.x;

        // Rotate player body left/right
        playerTransform.localRotation = Quaternion.Euler(0f, rotationY, 0f);

        // Rotate camera up/down
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }    
    private void HandlePlayerMovement()
    {
        float deltaTime = Time.fixedDeltaTime;

        // Get movInput
        Vector2 movementInput = movement.action.ReadValue<Vector2>();

        if(!playerStartedMoving && movementInput != Vector2.zero)
        {
            playerStartedMoving = true;
        }

        // Direction in which player wants to move
        Vector3 wishDir = playerController.transform.forward * movementInput.y + playerController.transform.right * movementInput.x;
        if (wishDir.sqrMagnitude > 1f)
            wishDir.Normalize();

        if(playerController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }    

        // Jump input
        if (jump.action.ReadValue<float>() == 1 && playerController.isGrounded)
        {
            playerVelocity.y = jumpImpulse;
        }



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

        playerController.Move(playerVelocity * deltaTime);
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
        return pointerPosition.action.ReadValue<Vector2>();
    }

    private void HandleMenuOpen()
    {
        menu.action.performed += MenuOpen;
    }    
    private void MenuOpen(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
}
