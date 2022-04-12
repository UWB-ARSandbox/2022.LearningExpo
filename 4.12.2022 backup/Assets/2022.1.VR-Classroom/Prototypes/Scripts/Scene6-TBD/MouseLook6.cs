using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MouseLook6 : MonoBehaviour
{
    public float mouseSensitivity = 8f;
    public Transform playerBody;

    private float xRotation = 0f;
    private float mouseX = 0, mouseY = 0;
    private float VRmultiplier = 20;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (XRSettings.isDeviceActive) {
            mouseX = mouseX * VRmultiplier;
        }
        playerBody.Rotate(Vector3.up, mouseX);
    }

    public void ReceiveInput (Vector2 mouseInput) {
        mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;
    }
}
