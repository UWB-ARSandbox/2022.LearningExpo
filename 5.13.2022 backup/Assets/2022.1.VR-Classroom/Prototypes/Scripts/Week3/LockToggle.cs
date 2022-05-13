using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ASL;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class LockToggle : MonoBehaviour
{
    public bool locked = true;
    public bool locallyUnlocked = false;
    public Collider boothCollider;
    public Renderer boothRenderer;
    public float defaultLockedTransparency;
    public bool startUnlocked;
    public LockToggle Pair;
    ASLObject m_ASLObject;

    void Start()
    {
        m_ASLObject = GetComponent<ASLObject>();
        m_ASLObject._LocallySetFloatCallback(floatFunction);

        defaultLockedTransparency = boothRenderer.material.color.a;
        
        ToggleCanvasGraphicRaycaster(false);
        if (startUnlocked || !boothRenderer.enabled || !boothRenderer.gameObject.activeSelf) {
            Unlock();
        }
        
        //onSelectEnter.AddListener(floatFunction);
    }

    // public void IClickableClicked() 
    // { //Andy
    //     if (GameManager.AmTeacher) {
    //         float[] toggleBooth = new float[1] { 100 };
    //         m_ASLObject.SendAndSetClaim(() =>
    //         {
    //             m_ASLObject.SendFloatArray(toggleBooth);
    //         });
    //     }
    // }

    
    public void floatFunction(string _id, float[] _f) 
    {
        int opcode = (int)_f[0];
        switch (opcode) {
            case 100: //unlock
                Unlock();
                break;
            case 101: //lock
                Lock();
                break;
            case 102: //force lock
                Lock(true);
                break;
        }
        
        //TODO for connor: will probably need to rework this if we still want it
        // if (_f[0] == 100)
        // {
        //     if (Pair != null && Pair.locked != locked)
        //     {
        //         Pair.TriggerOnClick();
        //     }
        // }
    }

    public void Lock(bool forceLock = false)
    {
        if (!locallyUnlocked || forceLock)
        {
            locked = true;
            boothRenderer.enabled = true;
            boothCollider.enabled = true;
            ChangeAlpha(defaultLockedTransparency);
            ToggleCanvasGraphicRaycaster(false);
        }
    }

    public void Unlock(bool locallyUnlock = false)
    {
        if (locallyUnlock) {
            locallyUnlocked = true;
        }
        locked = false;
        boothRenderer.enabled = false;
        boothCollider.enabled = false;
        ChangeAlpha(0f);
        ToggleCanvasGraphicRaycaster(true);
    }

    private void ToggleCanvasGraphicRaycaster(bool setActive)
    //Helps to ensure that the graphic raycaster of the start button, if it exists (i.e. it's an assessment booth),
    //is not a raycast target, and thus cannot be pressed when the booth is locked
    {
        Transform lectern = transform.Find("../../Lectern");
        if (lectern != null) {
            lectern.Find("LecternCanvas").GetComponent<GraphicRaycaster>().enabled = setActive;
            lectern.Find("LecternCanvas").GetComponent<TrackedDeviceGraphicRaycaster>().enabled = setActive;
        }
    }

    public void ChangeAlpha(float alpha) 
    {
        Color oldColor = boothRenderer.material.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        boothRenderer.material.color = newColor;
    }

    // public void TriggerOnClick() {
    //     if (GameManager.AmTeacher)
    //     {
    //         float[] toggleBooth = new float[1] { 100 };
    //         m_ASLObject.SendAndSetClaim(() =>
    //         {
    //             m_ASLObject.SendFloatArray(toggleBooth);
    //         });
    //     }
    // }

    #region sending
    public void UnlockBooth() {
        ChangeBoothStatus(100f);
    }
    public void LockBooth() {
        ChangeBoothStatus(101f);
    }
    public void ForceLockBooth() {
        ChangeBoothStatus(102f);
    }
    public void ChangeBoothStatus(float code) {
        if (GameManager.AmTeacher)
        {
            float[] boothStatus = new float[1] { code };
            m_ASLObject.SendAndSetClaim(() =>
            {
                m_ASLObject.SendFloatArray(boothStatus);
            });
        }
    }
    #endregion
}
