using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController6 : MonoBehaviour
{
    MouseLook6 mouseLook;

    private PlayerInputActions6 playerInputActions6;
    private InputAction movement;

    //Movement + Jump
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 5;
    public float jumpHeight = 3f;

    //Gravity
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    //MouseLook
    Vector2 mouseInput;

    private void Awake() {
        playerInputActions6 = new PlayerInputActions6();
        controller = gameObject.GetComponent<CharacterController>();
        Debug.Assert(controller != null);

        mouseLook = gameObject.GetComponentInChildren<MouseLook6>();
    }

    private void OnEnable() {
        movement = playerInputActions6.Player.Movement;
        movement.Enable();

        playerInputActions6.Player.Jump.performed += DoJump;
        playerInputActions6.Player.Enable();

        /*playerInputActions6.Player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerInputActions6.Player.MouseX.Enable();
        playerInputActions6.Player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        playerInputActions6.Player.MouseY.Enable();*/

        playerInputActions6.Player.Rotation.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        playerInputActions6.Player.Rotation.Enable();
    }

    private void DoJump(InputAction.CallbackContext obj) {
        //Debug.Log("Jump!!");
        if (isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void OnDisable() {
        movement.Disable();
        playerInputActions6.Player.Jump.Disable();
        /*playerInputActions6.Player.MouseX.Disable();
        playerInputActions6.Player.MouseY.Disable();*/
        playerInputActions6.Player.Rotation.Disable();
    }

    private void FixedUpdate() {
        //Ground Check for Gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2;
        }

        //Horizontal Movement
        //Debug.Log("Movement Values " + movement.ReadValue<Vector2>());
        Vector2 moveValue = movement.ReadValue<Vector2>();
        Vector3 move = transform.right * moveValue.x + transform.forward * moveValue.y;
        controller.Move(move * speed * Time.deltaTime);

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        mouseLook.ReceiveInput(mouseInput);

    }
}
