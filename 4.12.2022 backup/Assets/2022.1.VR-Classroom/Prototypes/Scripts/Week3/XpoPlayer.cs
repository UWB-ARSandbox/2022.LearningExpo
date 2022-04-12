using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using ASL;

public class XpoPlayer : MonoBehaviour {

    private static readonly float UPDATES_PER_SECOND = 10.0f;
    private static readonly float CALIBRATE_INTERVAL = 60.0f;

    public UnityEngine.XR.Interaction.Toolkit.Inputs.InputActionManager iam;

    ASLObject m_ASLObject;
    UserObject m_UserObject;
    public GhostPlayer m_GhostPlayer;
    public Text LocalUsername;

    // Start is called before the first frame update
    void Start() {
        LocalUsername.text = ASL.GameLiftManager.GetInstance().m_Username;

        m_ASLObject = GetComponent<ASLObject>();
        Debug.Assert(m_ASLObject != null);

        m_UserObject = GetComponent<UserObject>();
        Debug.Assert(m_UserObject != null);

        m_ASLObject._LocallySetFloatCallback(floatFunction);

        StartCoroutine(DelayedInit());
    }

    IEnumerator DelayedInit() {
        while (m_GhostPlayer == null) {
            Debug.Log("Finding ghost...");
            foreach (GhostPlayer gp in FindObjectsOfType<GhostPlayer>()) {
                if (gp.IsOwner(m_UserObject.ownerID) && gp.ownerID != 0) {
                    Debug.Log("GHOST FOUND - ID:" + gp.ownerID);
                    m_GhostPlayer = gp;
                    gp.gameObject.SetActive(false);
                    StartCoroutine(NetworkedUpdate());
                    StartCoroutine(InputActionsRecalibrate());
                    if (GameLiftManager.GetInstance().AmLowestPeer()) {
                        ColorHost();
                    }
                    m_GhostPlayer.SendPlayerName(ASL.GameLiftManager.GetInstance().m_Username);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ColorHost() {
        Debug.Log("Sent Color");
        Color hostColor = Color.yellow;
        gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() => {
            gameObject.GetComponent<ASL.ASLObject>().SendAndSetObjectColor(hostColor, hostColor);
        });
        m_GhostPlayer.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            m_GhostPlayer.GetComponent<ASL.ASLObject>().SendAndSetObjectColor(hostColor, hostColor);
        });
    }

    void Update() {
        foreach (GhostPlayer gp in FindObjectsOfType<GhostPlayer>()) {
            Transform usernameCanvasTransform = gp.transform.Find("Username");
            usernameCanvasTransform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, transform.eulerAngles.z);
        }
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

            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }

    IEnumerator InputActionsRecalibrate() {
        iam.enabled = false;
        iam.enabled = true;
        yield return new WaitForSeconds(CALIBRATE_INTERVAL);
    }

    public void floatFunction(string _id, float[] _f) {
        Debug.Log("player float function");
        if (_f[0] == 1) {
            m_UserObject.SetOwner(_id, new float[2] { _f[0], _f[1] });
        }
    }
}
