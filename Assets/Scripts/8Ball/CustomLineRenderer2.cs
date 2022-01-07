using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
public class CustomLineRenderer2 : MonoBehaviour
{
    [SerializeField] Line Fill,Stroke;

    void OnEnable()
    {
        Fill.enabled = true;
        Stroke.enabled = true;
    }
    void OnDisable()
    {
        Fill.enabled = false;
        Stroke.enabled = false;
    }
    public void Reset()
    {
        Fill.Start = Vector3.zero;
        Stroke.Start = Vector3.zero;
        Fill.End = Vector3.zero;
        Stroke.End = Vector3.zero;
    }
    public void SetPosition(Vector3 EndPos)
    {
        
        var pos = Fill.transform.InverseTransformPoint(EndPos);
        Fill.Start = Vector3.zero;
        Stroke.Start = Vector3.zero;
        Fill.End = pos;
        Stroke.End = pos;
    }
}
