
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Test_soccer : MonoBehaviour
{
    public LineRenderer line;
    public RaycastHit hit;
    public Vector3 point_mouse;

    private void OnMouseDown()
    {
        line.SetPosition(0, transform.position);
    }
    private void OnMouseDrag()
    {
     var ray =  Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(ray.origin);
        if(Physics.Raycast(ray,out hit ,1000))
        {
            var dir = hit.point - transform.position;
            var a = Quaternion.AngleAxis(180, Vector3.up);
            var b = a * dir;
            point_mouse = b + transform.position;
            line.SetPosition(1, point_mouse);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(hit.point, 0.5f);
    }

}