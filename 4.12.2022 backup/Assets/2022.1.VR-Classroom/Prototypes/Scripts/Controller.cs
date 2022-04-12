using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class Controller : MonoBehaviour
{
    public GameObject selected = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
         
            if( Physics.Raycast( ray, out hit, 100 ) )
            {
                selected = hit.transform.gameObject;
            }
        }
        
    }
}
