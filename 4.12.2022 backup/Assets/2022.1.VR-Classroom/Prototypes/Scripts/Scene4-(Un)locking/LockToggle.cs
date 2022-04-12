using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ASL;
using UnityEngine.XR.Interaction.Toolkit;

public class LockToggle : MonoBehaviour
{
    bool locked = true;
    Ray ray;
    RaycastHit hit;
    public Collider lockCollider;
    public Collider boothCollider;
    public Renderer boothRenderer;
    ASLObject m_ASLObject;

    void Start()
    {
        m_ASLObject = GetComponent<ASLObject>();
        m_ASLObject._LocallySetFloatCallback(floatFunction);
        //onSelectEnter.AddListener(floatFunction);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider == lockCollider && ASL.GameLiftManager.GetInstance().AmLowestPeer())
                {
                    float[] toggleBooth = new float[1] {100};
                    m_ASLObject.SendAndSetClaim(() =>
                    {
                        m_ASLObject.SendFloatArray(toggleBooth);
                    });
                }
            }
        }
    }

    public void floatFunction(string _id, float[] _f) 
    {
        if (_f[0] == 100)
        {
            locked = !locked;
            boothCollider.enabled = !boothCollider.enabled;
            if (locked) //could also just completely toggle the active state of the object instead of what's done here
            {
                Color oldColor = boothRenderer.material.color;
                Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
                boothRenderer.material.color = newColor;
            }
            else
            {
                Color oldColor = boothRenderer.material.color;
                Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.0f);
                boothRenderer.material.color = newColor;
            }
        }
    }

    public void TriggerOnClick(){
        float[] toggleBooth = new float[1] {100};
        m_ASLObject.SendAndSetClaim(() =>
        {
            m_ASLObject.SendFloatArray(toggleBooth);
        });
    }
}
