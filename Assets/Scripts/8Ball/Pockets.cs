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
        public bool Selectable = false;
        [ColorUsage(true)]
        public Color PocketNormalColor;
        [ColorUsage(true)]
        public Color PocketSelectColor;
        public GameObject PrefabFakeBall;
        public Material[] Skins;
        public float timeDestory = 3.0f;
        private Basket basket;
        [SerializeField] private SpriteRenderer PocketRenderer;
        private Diaco.EightBall.Server.BilliardServer Server;
        // private Diaco.EightBall.CueControllers.HitBallController CueBall;
        private void Start()
        {

            Server = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
            basket = FindObjectOfType<Basket>();

            PocketRenderer = GetComponentInChildren<SpriteRenderer>();
            Server.EnableBoarderPocket += Server_EnableBoarderPocket;

        }



        private void OnTriggerEnter(Collider Ball)
        {
            if (Ball.tag == "ball" && Ball.GetComponent<Diaco.EightBall.CueControllers.Ball>())
            {
                Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                var id = Ball.GetComponent<Diaco.EightBall.CueControllers.Ball>().ID;
                Ball.GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = false;

                Ball.GetComponent<Rigidbody>().AddForce(Vector3.down * 50);
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
                //SpwanFakeBall(id, Ball.GetComponent<Rigidbody>());
                Destroy(Ball.gameObject, timeDestory);
                // Destroy(Ball.GetComponent<ShodowFake>().shadow.gameObject); 

            }
            else if (Ball.tag == "whiteball" && Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>())
            {

                Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                var id = Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ID;

                /// Ball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().EnableYFix = false;
                // Ball.GetComponent<Rigidbody>().velocity = new Vector3(0.001f, 0.001f, 0.001f);
                ///   Ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.001f, 0.001f, 0.001f);

                Ball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                Ball.GetComponent<Rigidbody>().isKinematic = true;
                Ball.transform.DOScale(0.0f, 0.0001f);

                if (Server.InRecordMode == false)
                {

                    // SpwanFakeBall(id, Ball.GetComponent<Rigidbody>());
                    Handler_OnPocket(id);
                }

                // Debug.Log("kkkk");
            }
        }
        private void OnMouseDown()
        {
            if (Selectable)
            {
                PocketRenderer.DOColor(PocketSelectColor, 0.5f).OnComplete(() =>
                {
                    PocketRenderer.DOColor(PocketNormalColor, 0.5f);
                }).OnComplete(() =>
                {

                    PocketRenderer.color = PocketNormalColor;
                    Server.Emit_CallPocket(PocketID);
                });

                Debug.Log("Pocket" + this.name);
            }
        }
        private void Server_EnableBoarderPocket(bool show, int id)
        {
            ShowPocketBoarder(show, id);
        }


        /* private void SpwanFakeBall(int id, Rigidbody originBall)
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

         }*/

        private void ShowPocketBoarder(bool show, int id)
        {
            if (id == 0)///all Enable
            {
                PocketRenderer.enabled = show;
                Selectable = show;
            }
            else if (id > 0)//Show only this ID
            {
                if (id == this.PocketID)
                {
                    PocketRenderer.enabled = show;
                    Selectable = false;
                }
                else
                {
                    PocketRenderer.enabled = !show;
                    Selectable = false;
                }
            }
        }


        public event Action<int> OnPocket;
        public virtual void Handler_OnPocket(int id)
        {
            if (OnPocket != null)
            {
                OnPocket(id);
            }
        }
    }
}