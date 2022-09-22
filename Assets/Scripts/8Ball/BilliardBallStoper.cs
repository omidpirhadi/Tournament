using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BilliardBallStoper : MonoBehaviour
{
    private void OnTriggerExit(Collider Ball)
    {

        if (Ball.tag == "ball" && Ball.GetComponent<Diaco.EightBall.CueControllers.Ball>())
        {
            /* 

             
             rigi.AddForce(-Ball.transform.up * 50);


           */
            var rigi = Ball.GetComponent<Rigidbody>();
            rigi.velocity = Vector3.zero;
            rigi.angularVelocity = Vector3.zero;
            rigi.Sleep();
            Debug.Log(".......................STOPER");
        }

        
    }
}
