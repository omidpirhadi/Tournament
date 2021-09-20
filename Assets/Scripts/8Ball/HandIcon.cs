using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIcon : MonoBehaviour
{
    public float YPosition = 0.64f;
    public float XOffsetPosition = 0.37f;
    
    public Diaco.EightBall.CueControllers.HitBallController whiteball;
    void Start()
    {
        whiteball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
    }
    void LateUpdate()
    {
        this.transform.position = new Vector3(whiteball.transform.position.x + XOffsetPosition, YPosition, whiteball.transform.position.z);

    }
}
