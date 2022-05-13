using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private InputAction movement;
    private InputAction jump;
    private InputAction toggleSprint;

    //Movement + Jump
    private CharacterController controller;
    public float speed = 8f;
    public float gravity = -9.81f * 5;
    public float jumpHeight = 1.8f;

    //Gravity
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;

    private bool IsGrounded => Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    private static bool IsCursorLocked => Cursor.lockState == CursorLockMode.Locked;

    private bool IsSprinting { get; set; } = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        controller = gameObject.GetComponent<CharacterController>();
        Debug.Assert(controller != null);
    }

    private void OnEnable()
    {
        //position
        movement = playerInputActions.Player.Movement;
        movement.Enable();

        //position
        jump = playerInputActions.Player.Jump;
        jump.performed += DoJump;
        jump.Enable();

        //interaction
        toggleSprint = playerInputActions.Player.Sprint;
        toggleSprint.performed += ToggleSprint;
        toggleSprint.Enable();
    }

    private void ToggleSprint(InputAction.CallbackContext obj)
    {
        IsSprinting = !IsSprinting;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded && IsCursorLocked)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        //Horizontal Movement
        if (IsCursorLocked)
        {
            var runFactor = IsSprinting ? 1.5f : 1.0f;
            Vector2 moveValue = movement.ReadValue<Vector2>();
            Vector3 move = transform.right * moveValue.x + transform.forward * moveValue.y;
            controller.Move(move * speed * runFactor * Time.deltaTime);
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDisable()
    {
        movement.Disable();
        jump.Disable();
    }
}
