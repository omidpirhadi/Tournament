using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIcon : MonoBehaviour
{
    public Vector3 DefaultPosition = new Vector3(-0.17f, 0.57f, -0.15f);
    public Vector3 DefaultRotation = new Vector3(90.0f, 0.0f, -20.0f);
    public void SetDefault()
    {
       // transform.localPosition = DefaultPosition;
       /// transform.localEulerAngles = DefaultRotation;
    }
    public void OnEnable()
    {
     
       /// transform.localPosition = DefaultPosition;
       // transform.localEulerAngles = DefaultRotation;

    }
    public void OnDisable()
    {
        
    
       // transform.localPosition = DefaultPosition;
       // transform.localEulerAngles = DefaultRotation;
    }
}
