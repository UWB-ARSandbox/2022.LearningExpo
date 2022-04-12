using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using ASL;

public class XpoPlayer6 : MonoBehaviour {

    private static readonly float UPDATES_PER_SECOND = 10.0f;

    ASLObject m_ASLObject;
    UserObject6 m_UserObject;
    public GhostPlayer6 m_GhostPlayer;

    // Start is called before the first frame update
    void Start() {
        m_ASLObject = GetComponent<ASLObject>();
        Debug.Assert(m_ASLObject != null);

        m_UserObject = GetComponent<UserObject6>();
        Debug.Assert(m_UserObject != null);

        m_ASLObject._LocallySetFloatCallback(floatFunction);

        StartCoroutine(DelayedInit());
    }

    /*// Update is called once per frame
    private void Update() {

    }*/
    IEnumerator DelayedInit() {
        while (m_GhostPlayer == null) {
            Debug.Log("Finding ghost...");
            foreach (GhostPlayer6 gp in FindObjectsOfType<GhostPlayer6>()) {
                if (gp.IsOwner(m_UserObject.ownerID) && gp.ownerID != 0) {
                    Debug.Log("GHOST FOUND - ID:" + gp.ownerID);
                    m_GhostPlayer = gp;
                    gp.gameObject.SetActive(false);
                    StartCoroutine(NetworkedUpdate());
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Putting your update in a coroutine allows you to run it at a rate of your choice
    IEnumerator NetworkedUpdate() {
        while (true) {
            if (m_GhostPlayer == null) {
                yield return new WaitForSeconds(0.1f);
            }

            Debug.Log("Sending Position");
            m_GhostPlayer.SetWorldPosition(transform.position);
            m_GhostPlayer.SetWorldRotation(transform.rotation);

            yield return new WaitForSeconds(1 / UPDATES_PER_SECOND);
        }
    }

    public void floatFunction(string _id, float[] _f) {
        Debug.Log("player float function");
        if (_f[0] == 1) {
            m_UserObject.SetOwner(_id, new float[2] { _f[0], _f[1] });
        }
    }
}
