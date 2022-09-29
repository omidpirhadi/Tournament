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
            Ball.GetComponent<ShodowFake>().shadow.gameObject.SetActive(false);
            var rigi = Ball.GetComponent<Rigidbody>();
              rigi.isKinematic = true;
            ////  rigi.velocity = Vector3.zero;
            /// rigi.angularVelocity = Vector3.zero;
            // rigi.Sleep();
            DOVirtual.Float(0.33f, 0, 0.05f, (x) => {
                Ball.transform.localScale = new Vector3(x, x, x);
            });
              Debug.Log(".......................STOPER");
        }

        
    }
}
