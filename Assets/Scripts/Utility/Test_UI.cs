using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[ExecuteInEditMode]
public class Test_UI : MonoBehaviour
{

    public Transform target;

    void Start()
    {
 
    }

  
    void Update()
    {

        transform.LookAt(target,Vector3.up);

       
    }
    public void CheckPositionContacts()
    {
        Handheld.Vibrate(); 
    }
}
