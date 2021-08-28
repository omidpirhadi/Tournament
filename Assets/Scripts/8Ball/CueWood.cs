using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueWood : MonoBehaviour
{

    public Vector3 DefaultPosition = new Vector3(-0.5f, 0.0f, 0.0f);
    public Vector3 DefaultRotation = new Vector3(0.0f, 0.0f, 0.0f);

    public void SetDefault()
    {
        //transform.localPosition = DefaultPosition;
       /// transform.localEulerAngles = DefaultRotation;
    }
    public void OnEnable()
    {
    
     //   transform.localPosition = DefaultPosition;
      ///  transform.localEulerAngles = DefaultRotation;
      //  Debug.Log("ENABLEEEEEE");
    }
    public void OnDisable()
    {
    
       // transform.localEulerAngles = DefaultRotation;
       // Debug.Log("DIISSSSaLEEEEEE");
    }
}

