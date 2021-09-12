
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Test_soccer : MonoBehaviour
{

    public new Rigidbody rigidbody;
    public float Pow = 2f;
    public ForceMode forceMode;
    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(Vector3.right * Pow, forceMode);
        }
    }

}