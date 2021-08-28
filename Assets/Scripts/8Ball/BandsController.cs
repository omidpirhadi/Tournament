
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.EightBall.Band
{
    public class BandsController : MonoBehaviour
    {
       private  Server.BilliardServer Server;

        void OnEnable()
        {
            Server = FindObjectOfType<Server.BilliardServer>();
        }
        private void OnCollisionEnter(Collision ball)
        {
            if(ball.collider)
            {
                if (ball.collider.GetComponent<AddressBall>())
                {
                    var id = ball.collider.GetComponent<AddressBall>().IDPost;
                    Server.FillImpactToWallList(id);

                }
            }
        }
        public event Action<int> OnBandHit;
        protected void handler_onBandHit(int IdBall)
        {
            if(OnBandHit != null)
            {
                OnBandHit(IdBall);
            }
        }
    }
}