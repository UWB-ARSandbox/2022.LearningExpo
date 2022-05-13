using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boothLockNameAssigner : MonoBehaviour
{
    int numLocks = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (LockToggle go in FindObjectsOfType<LockToggle>()) {
            go.gameObject.name = "LockToggle" + numLocks.ToString();
            numLocks++;
        }
    }
}