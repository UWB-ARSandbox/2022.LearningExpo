using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using ASL;

public class XpoPlayer : MonoBehaviour {

    private static readonly float UPDATES_PER_SECOND = 10.0f;
    private static readonly float CALIBRATE_INTERVAL = 60.0f;

    public UnityEngine.XR.Interaction.Toolkit.Inputs.InputActionManager iam;

    ASLObject m_ASLObject;
    //UserObject m_UserObject;
    public GhostPlayer m_GhostPlayer;
    public SendAndPlayAudio audioManager;
    public Text LocalUsername;
    public Renderer cube;
    public Transform[] movingBodyParts;
    public Transform face;

    // Start is called before the first frame update
    void Start() {
        LocalUsername.text = ASL.GameLiftManager.GetInstance().m_Username;

        // m_ASLObject = GetComponent<ASLObject>();
        // Debug.Assert(m_ASLObject != null);

        // m_UserObject = GetComponent<UserObject>();
        // Debug.Assert(m_UserObject != null);

        //m_ASLObject._LocallySetFloatCallback(floatFunction);

        StartCoroutine(DelayedInit());
    }

    IEnumerator DelayedInit() {
        while (m_GhostPlayer == null) {
            Debug.Log("Finding ghost...");
            foreach (GhostPlayer gp in FindObjectsOfType<GhostPlayer>()) {
                if (gp.IsOwner(GameManager.MyID/*m_UserObject.ownerID*/) && gp.ownerID != 0) {
                    Debug.Log("GHOST FOUND - ID:" + gp.ownerID);
                    m_GhostPlayer = gp;
                    audioManager = gp.GetComponent<SendAndPlayAudio>();
                    audioManager.SetupInput();
                    gp.gameObject.SetActive(false);
                    StartCoroutine(NetworkedUpdate());
                    StartCoroutine(InputActionsRecalibrate());
                    if (GameManager.AmTeacher) {
                        ColorHost();
                    }
                    m_GhostPlayer.SendPlayerName(ASL.GameLiftManager.GetInstance().m_Username);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ColorHost() {
        m_GhostPlayer.SendHostColor();
        cube.material.color = Color.yellow;
        /*Debug.Log("Sent Color");
        Color hostColor = Color.yellow;
        gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() => {
            gameObject.GetComponent<ASL.ASLObject>().SendAndSetObjectColor(hostColor, hostColor);
        });*/
        // Color hostColor = Color.yellow;
        // m_GhostPlayer.GetComponent<ASLObject>().SendAndSetClaim(() =>
        // {
        //     m_GhostPlayer.GetComponent<ASLObject>().SendAndSetObjectColor(hostColor, hostColor);
        // });

        // float[] sendColor = new float[1] { 130 };
        // m_ASLObject.SendAndSetClaim(() => {
        //     m_ASLObject.SendFloatArray(sendColor);
        // });
        /*m_GhostPlayer.GetComponent<ASLObject>().SendAndSetClaim(() => {
            m_GhostPlayer.GetComponent<ASLObject>().SendFloatArray(sendColor);
        });*/
    }

    void Update() {
        foreach (GhostPlayer gp in FindObjectsOfType<GhostPlayer>()) {
            Transform minimapUsernameCanvasTransform = gp.transform.Find("MinimapUsername");
            minimapUsernameCanvasTransform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, transform.eulerAngles.z);
            
            Transform worldspaceUsernameCanvasTransform = gp.transform.Find("WorldUsername");
            worldspaceUsernameCanvasTransform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
        if (Camera.main) {
            //set head rotation to be the same as the camera's
            Transform cameraTransform = Camera.main.transform;
            Vector3 adjustedNewRotation = new Vector3(cameraTransform.eulerAngles.x, cameraTransform.transform.eulerAngles.y, cameraTransform.eulerAngles.z);
            face.transform.rotation = Quaternion.Euler(adjustedNewRotation);
        }
        //audioManager.SendAudio(); //commented out for now, so that we can hear videos without having to hear each other in bad quality
    }

    public void SetFaceTexture() {
        m_GhostPlayer.SetFaceTexture();
    }

    // Putting your update in a coroutine allows you to run it at a rate of your choice
    IEnumerator NetworkedUpdate() {
        while (true) {
            if (m_GhostPlayer == null) {
                yield return new WaitForSeconds(0.1f);
            }

            //Debug.Log("Sending Position");
            m_GhostPlayer.SetWorldPosition(transform.position);
            m_GhostPlayer.SetWorldRotation(transform.rotation);

            m_GhostPlayer.SendBodyPositionsAndRotations(movingBodyParts);

            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }

    IEnumerator InputActionsRecalibrate() {
        iam.enabled = false;
        iam.enabled = true;
        yield return new WaitForSeconds(CALIBRATE_INTERVAL);
    }

    /*public void floatFunction(string _id, float[] _f) {
        Debug.LogError("player float function");
        if (_f[0] == 1) {
            m_UserObject.floatFunction(_id, new float[2] { _f[0], _f[1] });
        }
        if (_f[0] == 130) {
            m_UserObject.floatFunction(_id, new float[1] { _f[0] });
        }
    }*/
}
