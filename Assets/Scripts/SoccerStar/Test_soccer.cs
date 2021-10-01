
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Test_soccer : MonoBehaviour
{

    ScrollRect Scroll;
    public void Start()
    {
        Scroll = GetComponent<ScrollRect>();

        Scroll.onValueChanged.AddListener(call => {


            Debug.Log(call);
        });
       
    }

}