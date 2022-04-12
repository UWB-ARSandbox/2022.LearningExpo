using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineEndPointController : MonoBehaviour
{
    public Color Color = Color.black;

    [Range(0.0f, 10.0f)]
    public float Scale = 1.0f;

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Renderer>().material.color = Color;
        transform.localScale = new Vector3(Scale, Scale, Scale);
    }
}
