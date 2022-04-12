/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using ASL;

public class XpoPlayer : MonoBehaviour
{
    public float MovementSpeed = 3f;
    public float FallVelocity = 0.3f;
    public float JumpVelocity = 10f;
    public Vector3 RespawnPoint = new Vector3(0, 2, 0);
    //public Text CoinCount;
    //public Text WinText;

    Vector3 velocity = Vector3.zero;
    *//*bool topCollision = false;
    bool bottomCollision = false;
    bool leftCollision = false;
    bool rightCollision = false;
    bool jumpRecharged = true;
    int coinsCollected = 0;
    float floor, leftWall, rightWall; //these need to reset on triggerExit*//*

    private Camera mainCamera;
    private Camera playerCamera;

    ASLObject m_ASLObject;
    UserObject m_UserObject;

    // Start is called before the first frame update
    void Start()
    {
        m_ASLObject = GetComponent<ASLObject>();
        Debug.Assert(m_ASLObject != null);
        //CoinCount = FindObjectOfType<GameManager>().CoinCount;
        //WinText = FindObjectOfType<GameManager>().WinText;
        *//*if (Camera.main.GetComponent<GameManager>() != null) {
            Camera.main.GetComponent<GameManager>().SetUpCamera(this);
        }*//*

        m_UserObject = GetComponent<UserObject>();
        Debug.Assert(m_UserObject != null);

        m_ASLObject._LocallySetFloatCallback(floatFunction);

        *//*m_ASLObject.SendAndSetClaim(() => {
            if (m_UserObject.IsOwner(ASL.GameLiftManager.GetInstance().m_PeerId)) {
                playerCamera = m_ASLObject.GetComponentInChildren<Camera>();
                Camera.main.enabled = false;
                playerCamera.enabled = true;
            }
        });*/

        /*if (m_UserObject.IsOwner(ASL.GameLiftManager.GetInstance().m_PeerId)) {
            playerCamera = m_ASLObject.GetComponentInChildren<Camera>();
            Camera.main.enabled = false;
            playerCamera.enabled = true;
        }*//*
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void floatFunction(string _id, float[] _f) {
        Debug.Log("player float function");
        if (_f[0] == 1) {
            m_UserObject.SetOwner(_id, new float[2] { _f[0], _f[1] });
        } else if (_f[0] == 2) {
            Debug.Log("floatFunction Called");
            *//*float collisionSide = _f[1];
            bool collisionEnter;
            float collisionPoint = 0;
            if (_f[2] == 0) {
                collisionEnter = false;
            } else {
                collisionEnter = true;
                collisionPoint = _f[3];
            }
            switch (collisionSide) {
                case 0:
                    //top
                    topCollision = collisionEnter;
                    if (topCollision) {
                        jumpRecharged = true;
                        floor = collisionPoint + transform.localScale.y / 2;
                        Debug.Log("Hit ground");
                    }
                    break;
                case 1:
                    //bottom
                    bottomCollision = collisionEnter;
                    break;
                case 2:
                    //left
                    leftCollision = collisionEnter;
                    if (leftCollision) {
                        leftWall = collisionPoint - transform.localScale.x / 2;
                    }
                    break;
                case 3:
                    //right
                    rightCollision = collisionEnter;
                    if (rightCollision) {
                        rightWall = collisionPoint + transform.localScale.x / 2;
                    }
                    break;
                default:
                    break;
            }*//*
        }
    }
}
*/