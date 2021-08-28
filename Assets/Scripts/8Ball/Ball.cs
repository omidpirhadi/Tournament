using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Diaco.EightBall.CueControllers
{
    public class Ball : MonoBehaviour
    {
        public int ID = 0;
        public float ThresholdSleep = 0.09f;
        public Vector3 TargetVelocity ;
        public bool HaveTarget = false;
        private new Rigidbody rigidbody;
        private LineRenderer lineRenderer;
        private HitBallController cueball;
        private Diaco.EightBall.Server.BilliardServer server;
        [SerializeField] private float SpeedBallCurrent = 0.0f;
        private Vector3 VlocityBallCurrent;
        [SerializeField]  private float Y_Pos_Refrence;
        
      [SerializeField]  private bool InMove = false;
        private Ray ray;
        private RaycastHit hit;
        public LayerMask layer;
        void Start()
        {

            rigidbody = GetComponent<Rigidbody>();
            lineRenderer = GetComponent<LineRenderer>();
            cueball = FindObjectOfType<HitBallController>();
            cueball.OnHitBall += Ball_OnHitBall;
            cueball.OnFreazeBall += Cueball_OnFreazeBall;
            cueball.OnFristHit += Cueball_OnFristHit;
            server = FindObjectOfType< Diaco.EightBall.Server.BilliardServer>();
            
            //InvokeRepeating("CheckballMove", 0, gamemanager.SendRate.value);//
            SetYPositionRefrence();
        }



        private void FixedUpdate()
        {
             VlocityBallCurrent = rigidbody.velocity;
            SpeedBallCurrent = rigidbody.velocity.magnitude;

            rigidbody.maxAngularVelocity = 150;

           /* if (HaveTarget && SpeedBallCurrent > ThresholdSleep )
            {
                var d = (TargetVelocity - rigidbody.velocity).normalized;
                rigidbody.velocity = d * SpeedBallCurrent;

            }*/
        }
        void LateUpdate()
        {
            FixOverflowMovment();
        }


        private void OnDestroy()
        {

            cueball.OnHitBall -= Ball_OnHitBall;
            cueball.OnFreazeBall -= Cueball_OnFreazeBall;
        }
       
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "wall")
                BounceBall(collision);
        }
        private void OnDrawGizmos()
        {
           Gizmos.color = Color.red;
            
            Gizmos.DrawRay(ray);
            Gizmos.DrawWireSphere(hit.point, 0.4f);
        }



        private void Cueball_OnFreazeBall(bool active)
        {
            rigidbody.isKinematic = active;
            // Debug.Log("Firize");
        }
        private void Cueball_OnFristHit(int id)
        {
            /*if (id != this.ID)
            {
                TargetVelocity = Vector3.zero;
                HaveTarget = false;
            }*/
        }
        private void Ball_OnHitBall(int target, Vector3 dir, Vector3 targetvelocity)
        {

            if (target != -1)
            {
                if (target == ID)
                {
                    lineRenderer.enabled = true;
                    SetlineDirection(dir);
                   /* if (targetvelocity.magnitude > 0.0f)
                    {
                        this.TargetVelocity = targetvelocity;
                        HaveTarget = true;

                    }*/
                }
                else if (target != ID)
                {
                 //  lineRenderer.enabled = false;
                 //  SetlineDirection(new Vector3(0, -10f, 0));
                  ///  this.TargetVelocity = Vector3.zero;
                  ///  HaveTarget = false;
                }
            }
            else
            {
              // lineRenderer.enabled = false;
             //  SetlineDirection(new Vector3(0, -10f, 0));
                // Debug.Log("Nulll");

            }
        }


        private void SetlineDirection(Vector3 pos)
        {
             lineRenderer.SetPosition(0, new Vector3(transform.position.x, 0, transform.position.z));

            lineRenderer.SetPosition(1, (new Vector3(pos.x, 0f, pos.z)));

        }
        public void SetYPositionRefrence()
        {
           DOVirtual.Float(0, 1, 2.0f, (x) => { }).OnComplete(() =>
            {

            }).OnComplete(() => {
                //rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                Y_Pos_Refrence = transform.position.y;
            });
        }
        private void FixOverflowMovment()
        {
           if (Y_Pos_Refrence > 0.0f)
            {
                var conflict_Y = Mathf.Abs(transform.position.y - Y_Pos_Refrence);
                if (conflict_Y > 0.1f)
                {

                    transform.position = new Vector3(transform.position.x, Y_Pos_Refrence, transform.position.z);

                    Debug.Log("Fix Y Ball");
                }

            }
            if (CheckBallMove() == true && InMove == false)
            {
                DOVirtual.Float(0, 1, 1.0f, (x) => { }).OnComplete(() =>
                {

                }).OnComplete(() =>
                {

                    InMove = true;
                });
            }
            if (VlocityBallCurrent.magnitude < ThresholdSleep && InMove == true)
            {

                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                InMove = false;
               
                Debug.Log("Fix Move Ball");

            }
        }
        private bool CheckBallMove()
        {
            var move = false;
            if (rigidbody.velocity.magnitude > ThresholdSleep || rigidbody.angularVelocity.magnitude > ThresholdSleep)
            {

                move = true;
            }

            return move;
        }
        private void BounceBall(Collision collision)
        {



            var normal = collision.contacts[0].normal;
            var reflect2 = Vector3.Reflect(VlocityBallCurrent.normalized, normal).normalized;
            rigidbody.velocity = reflect2 * collision.relativeVelocity.magnitude;
            Debug.Log("wall");

        }
    }
    
}