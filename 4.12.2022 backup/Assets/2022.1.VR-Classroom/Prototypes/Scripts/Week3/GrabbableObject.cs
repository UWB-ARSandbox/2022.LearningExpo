using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    [Range(1, 50)]
    public int MaxGrabbableObjects = 20;

    [Range(1.0f, 3.0f)]
    public float GrabRange = 1.5f;

    private static Queue<GrabbableObject> _grabbableObjects;

    private Vector3 _tempScale = Vector3.zero;

    public void Start()
    {
        _tempScale = transform.localScale;
        _grabbableObjects.Enqueue(this);
        if (_grabbableObjects.Count > MaxGrabbableObjects)
        {
            var toDelete = _grabbableObjects.Dequeue();
            Destroy(toDelete.gameObject);
        }
    }

    public void Grab(GameObject gObject)
    {
        transform.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<Rigidbody>().detectCollisions = false;
        transform.parent = gObject.transform;
        transform.localPosition = Vector3.forward * GrabRange;
    }

    public void LetGo()
    {
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().detectCollisions = true;

        transform.parent = null;
        transform.localScale = _tempScale;
    }
}
