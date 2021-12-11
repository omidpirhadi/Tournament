using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Diaco.EightBall.Pockets
{
   
    public class Pockets : MonoBehaviour
    {
        public int PocketID = 0;
        [ColorUsage(true)]
        public Color PocketNormalColor;
        [ColorUsage(true)]
        public Color PocketSelectColor;
        public GameObject PrefabFakeBall;
        public Material[] Skins;
        public float timeDestory = 3.0f;
        private Basket basket;
        private SpriteRenderer PocketRenderer;
        private Diaco.EightBall.Server.BilliardServer Server;
       // private Diaco.EightBall.CueControllers.HitBallController CueBall;
        private void Start()
        {

            Server = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
            basket = FindObjectOfType<Basket>();

            PocketRenderer = GetComponentInChildren<SpriteRenderer>();
            Server.EnableBoarderPocket += Server_EnableBoarderPocket;

        }

        private void Server_EnableBoarderPocket(bool show)
        {
            ShowPocketBoarder(show);
        }

        private void OnTriggerEnter(Collider Ball)
        {
            if (Ball.tag == "ball" && Ball.GetComponent<Diaco.EightBall.CueControllers.Ball>())
            {
                var id = Ball.GetComponent<Diaco.EightBall.CueControllers.Ball>().ID;
                // Ball.GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = false;
                Ball.GetComponent<Rigidbody>().velocity = new Vector3(0.001f, 0.001f, 0.001f);

                if (Server.InRecordMode == false)
                {
                    if (Server.FirstPocketCall == 0)
                        Server.FirstPocketCall = PocketID;

                    Server.DisableAllSharInBiliboard(id);

                    Server.AddBallToBasket(id);

                    basket.AddToQueue(id);
                    StartCoroutine(basket.ExtractBall());

                    Handler_OnPocket(id);

                    Server.DeletedBallCount++;
                }
                SpwanFakeBall(id, Ball.GetComponent<Rigidbody>());
                /// Destroy(Ball.gameObject, timeDestory);
                // Destroy(Ball.GetComponent<ShodowFake>().shadow.gameObject); 

            }
            else if (Ball.tag == "whiteball" && Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>())
            {

                var id = Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ID;
                
                /// Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().EnableYFix = false;
                // Ball.GetComponent<Rigidbody>().velocity = new Vector3(0.001f, 0.001f, 0.001f);
                ///   Ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.001f, 0.001f, 0.001f);
                Ball.GetComponent<Rigidbody>().velocity = new Vector3(0.001f, 0.0f, 0.0f);
                Ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.00f, 0.001f, 0.0f);
                Ball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                Ball.GetComponent<Rigidbody>().isKinematic = true;
                Ball.transform.DOScale(0.0f, 0.0001f);

                if (Server.InRecordMode == false)
                {
                    
                    SpwanFakeBall(id, Ball.GetComponent<Rigidbody>());
                    Handler_OnPocket(id);
                }

                // Debug.Log("kkkk");
            }
        }
        private void OnMouseDown()
        {
            PocketRenderer.DOColor(PocketSelectColor, 0.5f).OnComplete(() =>
            {
                PocketRenderer.DOColor(PocketNormalColor, 0.5f);
            }).OnComplete(() => { Server.PocketSelected = PocketID; });
           
            Debug.Log("Pocket" + this.name);
        }

        private void SpwanFakeBall(int id, Rigidbody originBall)
        {

            Vector3 pos = originBall.transform.position;
            Quaternion rotate = originBall.transform.rotation;

            if (id != 0)
                Destroy(originBall.gameObject);



            var fakeball = Instantiate(PrefabFakeBall, pos, rotate);
            fakeball.GetComponent<MeshRenderer>().material = Skins[id];
            var rigidbodyfake = fakeball.GetComponent<Rigidbody>();
            rigidbodyfake.velocity = originBall.velocity;
            Destroy(fakeball, timeDestory);

        }

        private void ShowPocketBoarder(bool show)
        {

            PocketRenderer.enabled = show;
        }
        public event Action<int> OnPocket;
        public virtual void  Handler_OnPocket(int id)
        {
            if (OnPocket != null)
            {
                OnPocket(id);
            }
        }
    }
}