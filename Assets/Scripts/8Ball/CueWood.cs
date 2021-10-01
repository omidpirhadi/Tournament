using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueWood : MonoBehaviour
{

    // public Vector3 DefaultPosition = new Vector3(-0.5f, 0.0f, 0.0f);
    //  public Vector3 DefaultRotation = new Vector3(0.0f, 0.0f, 0.0f);

    public Diaco.EightBall.CueControllers.HitBallController whiteball;
    public Transform WoodRenderer;
    void  Start()
    {
        whiteball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
        WoodRenderer.position = new Vector3(-8.5f, 1.0f, 0.0f);
    }
    void  OnEnable()
    {
        whiteball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
        this.transform.position = whiteball.transform.position;
        WoodRenderer.localPosition = new Vector3(-8.5f, 1.0f, 0.0f);
    }
     void LateUpdate()
    {
        this.transform.position = whiteball.transform.position;
       // WoodRenderer.localPosition = new Vector3(-8.5f, 1.0f, 0.0f);
    }
}

