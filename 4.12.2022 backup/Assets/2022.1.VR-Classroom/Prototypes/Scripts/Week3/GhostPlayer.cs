using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ASL;

public class GhostPlayer : MonoBehaviour
{
    public int ownerID;

    ASLObject m_ASLObject;

    public Text GlobalUsername;

    // Start is called before the first frame update
    void Start()
    {
        m_ASLObject = GetComponent<ASLObject>();
        Debug.Assert(m_ASLObject != null);
        m_ASLObject._LocallySetFloatCallback(FloatReceive);
    }

    public bool IsOwner(int peerID) {
        return peerID == ownerID;
    }

    public void IncrementWorldPosition(Vector3 m_AdditiveMovementAmount) {
        if (ownerID == ASL.GameLiftManager.GetInstance().m_PeerId) {
            m_ASLObject.SendAndSetClaim(() => {
                m_ASLObject.SendAndIncrementWorldPosition(m_AdditiveMovementAmount);
            });
        }
    }

    public void IncrementWorldRotation(Quaternion m_RotationAmount) {
        if (ownerID == ASL.GameLiftManager.GetInstance().m_PeerId) {
            m_ASLObject.SendAndSetClaim(() => {
                m_ASLObject.SendAndIncrementWorldRotation(m_RotationAmount);
            });
        }
    }

    public void IncrementWorldScale(Vector3 m_AdditiveScaleAmount) {
        if (ownerID == ASL.GameLiftManager.GetInstance().m_PeerId) {
            m_ASLObject.SendAndSetClaim(() => {
                m_ASLObject.SendAndIncrementWorldScale(m_AdditiveScaleAmount);
            });
        }
    }

    public void SetWorldPosition(Vector3 worldPosition) {
        if (ownerID == ASL.GameLiftManager.GetInstance().m_PeerId) {
            m_ASLObject.SendAndSetClaim(() => {
                m_ASLObject.SendAndSetWorldPosition(worldPosition);
            });
        }
    }

    public void SetWorldRotation(Quaternion worldRotation) {
        if (ownerID == ASL.GameLiftManager.GetInstance().m_PeerId) {
            m_ASLObject.SendAndSetClaim(() => {
                m_ASLObject.SendAndSetWorldRotation(worldRotation);
            });
        }
    }

    public void SetWorldScale(Vector3 worldScale) {
        if (ownerID == ASL.GameLiftManager.GetInstance().m_PeerId) {
            m_ASLObject.SendAndSetClaim(() => {
                m_ASLObject.SendAndSetWorldScale(worldScale);
            });
        }
    }

    public void FloatReceive(string _id, float[] _f) {
        if (_f[0] == 1) {
            SetOwner(_id, _f);
        }
        if (_f[0] == 100) {
            string username = "";
            for (int i = 1; i < _f.Length; i++) {
                username += (char)(int)_f[i];
            }
            Debug.Log("FloatReceive, Converted Username===============>" + username);
            GlobalUsername.text = username;
        }
    }

    public void SendPlayerName(string _playername) {
        Debug.Log("Player name: " + _playername);
        float[] usernameAsAFloatArray = new float[_playername.Length + 1];
        usernameAsAFloatArray[0] = 100;
        for (int i = 1; i < _playername.Length + 1; i++) {
            usernameAsAFloatArray[i] = (float)(int)_playername[i - 1];
        }

        m_ASLObject.SendAndSetClaim(() =>
        {
            m_ASLObject.SendFloatArray(usernameAsAFloatArray);
        });
    }

    public void SetOwner(string _id, float[] _f) {
        Debug.Log("ghostPlayer float function");
        ownerID = (int)_f[1];
        Debug.Log("-----");
        Debug.Log("GhostMyID: " + ASL.GameLiftManager.GetInstance().m_PeerId);
        Debug.Log("GhostOwnerID: " + ownerID);
    }
}
