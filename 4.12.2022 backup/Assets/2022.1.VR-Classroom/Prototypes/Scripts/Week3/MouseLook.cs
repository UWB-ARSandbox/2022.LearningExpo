using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class MouseLook : MonoBehaviour {
    public float mouseSensitivity = 8f;
    public Transform playerBody;
    public TrackedPoseDriver tpd;
    public InputActionManager iam;

    private float xRotation = 0f;
    private float mouseX = 0, mouseY = 0;
    private float VRmultiplier = 20;

    public LineRenderer leftLR;
    public LineRenderer rightLR;

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        Debug.Log("ACTIVE?: " + XRSettings.isDeviceActive);
        
        if (XRSettings.isDeviceActive) {
            mouseX = mouseX * VRmultiplier;
            leftLR.gameObject.SetActive(true);
            rightLR.gameObject.SetActive(true);
            tpd.enabled = true;
        } else {
            leftLR.gameObject.SetActive(false);
            rightLR.gameObject.SetActive(false);
            tpd.enabled = false;
        }

        transform.parent.localPosition = new Vector3(0, -0.3f, 0);
        playerBody.Rotate(Vector3.up, mouseX);
    }

    public void ReceiveInput(Vector2 mouseInput) {
        mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;
    }
}
