using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Diaco.EightBall.Pockets
{
    public delegate void Pocket(int ID);
    public class Pockets : MonoBehaviour
    {
        public Basket basket;
        public event Pocket OnPocket;
        private Diaco.EightBall.Server.BilliardServer Server;
       // private Diaco.EightBall.CueControllers.HitBallController CueBall;
        private void Start()
        {
            Server = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
            basket = FindObjectOfType<Basket>();
         

        }
        private void OnTriggerEnter(Collider Ball)
        {
            if(Ball.tag == "ball" && Ball.GetComponent< Diaco.EightBall.CueControllers.Ball>())
            {
                var id = Ball.GetComponent<Diaco.EightBall.CueControllers.Ball>().ID;
               // Ball.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(Ball.gameObject,0.1f);
                if (Server.InRecordMode == false)
                {
                    Server.DisableAllSharInBiliboard(id);

                    Server.BallInBasket.Add(id);

                    basket.AddToQueue(id);
                    StartCoroutine(basket.ExtractBall());

                    Handler_OnPocket(id);

                    Server.DeletedBallCount++;
                }
            }
            else if(Ball.tag == "whiteball" && Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>())
            {
                if (Server.InRecordMode == false)
                {
                    var id = Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ID;
                    Ball.GetComponent<Rigidbody>().isKinematic = true;
                    Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                    Ball.transform.DOScale(0.0f, 0.1f);
                    Handler_OnPocket(id);
                }
                else
                {
                    var id = Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ID;
                    Ball.GetComponent<Rigidbody>().Sleep();
                    Ball.transform.DOScale(0.0f, 0.1f);
                }
                Debug.Log("kkkk");
            }
        }
        public virtual void  Handler_OnPocket(int id)
        {
            if (OnPocket != null)
            {
                OnPocket(id);
            }
        }
    }
}