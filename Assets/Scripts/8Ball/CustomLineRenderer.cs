using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
public class CustomLineRenderer : MonoBehaviour
{
    [SerializeField] Line Fill, Stroke;
    [SerializeField] Transform StartPosition,EndPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        Fill.Start = StartPosition.localPosition;
        Stroke.Start  = StartPosition.localPosition;
        Fill.End = EndPosition.localPosition;
        Stroke.End = EndPosition.localPosition;
    }
}
