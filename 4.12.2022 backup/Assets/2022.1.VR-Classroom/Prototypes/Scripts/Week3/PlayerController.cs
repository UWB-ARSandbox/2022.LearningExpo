using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    MapToggle mapTogglePC;
    MapToggle mapToggleVR;
    MouseLook mouseLook;

    private PlayerInputActions playerInputActions;
    private InputAction movement;
    private InputAction jump;
    private InputAction rotation;
    private InputAction grab;
    private InputAction switchmap;

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
    bool isGrounded;

    private GrabbableObject selectedObject = null;

    //MouseLook
    Vector2 mouseInput;

    public bool HasObject { 
        get {
            return selectedObject != null;
        } 
    }

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        controller = gameObject.GetComponent<CharacterController>();
        Debug.Assert(controller != null);

        mouseLook = gameObject.GetComponentInChildren<MouseLook>();
    }

    private void Start() {
        foreach (MapToggle mt in FindObjectsOfType<MapToggle>()) {
            if (mt.gameObject.name.Equals("PC Map Canvas")) {
                mapTogglePC = mt;
            } else {
                mapToggleVR = mt;
            }
        }
        StartCoroutine(AttachVRCanvas());
        StartCoroutine(VRCheck());
    }

    IEnumerator AttachVRCanvas() {
        while (FindObjectOfType<MouseLook>() == null || mapToggleVR == null) {
            yield return new WaitForSeconds(0.1f);
        }
        if (gameObject.GetComponent<UserObject>().ownerID == ASL.GameLiftManager.GetInstance().m_PeerId) {
            mapToggleVR.transform.SetParent(FindObjectOfType<MouseLook>().gameObject.transform, false);
        }
    }

    IEnumerator VRCheck() {
        while (true) {
            if (XRSettings.isDeviceActive) {
                mapTogglePC.gameObject.SetActive(false);
                mapToggleVR.gameObject.SetActive(true);
            } else {
                mapTogglePC.gameObject.SetActive(true);
                mapToggleVR.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnEnable() {
        movement = playerInputActions.Player.Movement;
        movement.Enable();

        jump = playerInputActions.Player.Jump;
        jump.performed += DoJump;
        jump.Enable();

        rotation = playerInputActions.Player.Rotation;
        rotation.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        rotation.Enable();

        grab = playerInputActions.Player.Grab;
        grab.performed += TryGrab;
        grab.Enable();

        switchmap = playerInputActions.Player.SwitchMap;
        switchmap.performed += ToggleMap;
        switchmap.Enable();
    }

    private void OnDisable() {
        movement.Disable();
        jump.Disable();
        rotation.Disable();
        grab.Disable();
        switchmap.Disable();
    }

    private void TryGrab(InputAction.CallbackContext obj)
    {
        Debug.Log("TryGrab!");
        //test if we are over the PC UI
        if (EventSystem.current.IsPointerOverGameObject(Mouse.current.deviceId)) return;

        //cast a ray corresponding to the object the mouse is over
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //if we hit the object, we try to select the object and call the GrabbableObject internal functions
            Debug.Log("Hit:" + hit.transform.gameObject.name);
            SelectObject(hit.transform.GetComponent<GrabbableObject>());
        }

    }

    private void SelectObject(GrabbableObject uObject)
    {
        if (uObject == selectedObject) return;
        DeselectObject();
        selectedObject = uObject;
        selectedObject.Grab(gameObject);
    }

    private void DeselectObject()
    {
        if (HasObject)
        {
            selectedObject.LetGo();
            selectedObject = null;
        }
    }

    private void DoJump(InputAction.CallbackContext obj) {
        Debug.Log("Jump!!");
        if (isGrounded) {
            Debug.Log("Actual Jump!!");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ToggleMap(InputAction.CallbackContext obj) {
        mapTogglePC.ToggleMap();
        mapToggleVR.ToggleMap();
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
