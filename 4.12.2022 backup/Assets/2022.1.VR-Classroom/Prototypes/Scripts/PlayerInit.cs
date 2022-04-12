using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{

    private static bool IsPC
    {
        get
        {
            return (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ASL.GameLiftManager.GetInstance().AmLowestPeer())
        {
            ASL.ASLHelper.InstantiateASLObject("SimpleDemoPrefabs/WorldOriginCloudAnchorObject", Vector3.zero, Quaternion.identity, string.Empty, string.Empty, SpawnWorldOrigin);
        }

        if (IsPC)
        {
            //If on PC, set normal Camera as Main Camera tag, remove AR Camera
            var mainCamera = GameObject.Find("PlayerCameraPCandVR");
            mainCamera.transform.parent = null;
            mainCamera.tag = "MainCamera";
            Destroy(GameObject.Find("AR Camera"));
        }
    }

    private void Update()
    {
        Destroy(gameObject);
    }

    private static void SpawnWorldOrigin(GameObject _worldOriginVisualizationObject)
    {
        _worldOriginVisualizationObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            ASL.ASLHelper.CreateARCoreCloudAnchor(Pose.identity, _worldOriginVisualizationObject.GetComponent<ASL.ASLObject>(), _waitForAllUsersToResolve: false);
        });
    }
}
