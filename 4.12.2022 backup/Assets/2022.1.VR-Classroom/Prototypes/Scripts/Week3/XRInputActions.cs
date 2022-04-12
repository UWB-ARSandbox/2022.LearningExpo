using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRInputActions : MonoBehaviour
{
    private XRIDefaultInputActions _xria;

    // Start is called before the first frame update
    private void OnEnable()
    {
        _xria.XRIHead.Position.performed += ctx => transform.position = _xria.XRIHead.Position.ReadValue<Vector3>();
        _xria.XRIHead.Position.Enable();
        _xria.XRIHead.Rotation.performed += ctx => transform.rotation = _xria.XRIHead.Rotation.ReadValue<Quaternion>();
        _xria.XRIHead.Rotation.Enable();
        _xria.XRIHead.Enable();

        _xria.XRILeftHandInteraction.Enable();
        _xria.XRIRightHandInteraction.Enable();
    }

    private void Awake()
    {
        _xria = new XRIDefaultInputActions();
    }

    private void OnDisable()
    {
        _xria.XRIHead.Rotation.Disable();
        _xria.XRIHead.Position.Disable();
        _xria.Disable();
    }
}
