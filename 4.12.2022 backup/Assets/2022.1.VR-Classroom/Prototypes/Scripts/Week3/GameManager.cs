using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ASL;

public class GameManager : MonoBehaviour {
    public Vector3 RespawnPoint;

    Dictionary<int, string> players;
    int playerIndex = 0;
    int playerIndex2 = 0;
    List<int> playerIDs = new List<int>();

    // Start is called before the first frame update
    private void Start() {
        players = GameLiftManager.GetInstance().m_Players;
        ASL_PhysicsMasterSingleton.Instance.SetUpPhysicsMaster();

        if (GameLiftManager.GetInstance().AmLowestPeer()) {
            foreach (int playerID in players.Keys) {
                playerIDs.Add(playerID);

                ASL.ASLHelper.InstantiateASLObject("FirstPersonPlayer",
                    new Vector3(RespawnPoint.x, RespawnPoint.y + 1.05f, RespawnPoint.z),
                    Quaternion.identity, "", "", playerSetUp);

                ASL.ASLHelper.InstantiateASLObject("GhostPlayer",
                    new Vector3(RespawnPoint.x, RespawnPoint.y + 1.05f, RespawnPoint.z),
                    Quaternion.identity, "", "", ghostSetUp);

                RespawnPoint.x += 2;
            }
        }
    }

    private static void playerSetUp(GameObject _gameObject) {
        if (ASL_PhysicsMasterSingleton.Instance.IsPhysicsMaster) {
            GameManager _this = FindObjectOfType<GameManager>();
            int playerID = _this.playerIDs[_this.playerIndex];
            _this.playerIndex++;
            float[] m_floatArray = new float[2] { 1, playerID };
            _gameObject.GetComponent<ASLObject>().SendAndSetClaim(() => {
                _gameObject.GetComponent<ASLObject>().SendFloatArray(m_floatArray);
            });
        }
    }

    private static void ghostSetUp(GameObject _gameObject) {
        if (ASL_PhysicsMasterSingleton.Instance.IsPhysicsMaster) {
            GameManager _this = FindObjectOfType<GameManager>();
            int playerID = _this.playerIDs[_this.playerIndex2];
            _this.playerIndex2++;
            float[] m_floatArray = new float[2] { 1, playerID };
            _gameObject.GetComponent<ASLObject>().SendAndSetClaim(() => {
                _gameObject.GetComponent<ASLObject>().SendFloatArray(m_floatArray);
            });
        }
    }
}
