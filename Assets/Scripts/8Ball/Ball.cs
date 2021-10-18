using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Diaco.EightBall.CueControllers
{
    public class Ball : MonoBehaviour
    {
        public BillardTestSetting testSetting;
        public int ID = 0;
        public float ThresholdSleep = 0.09f;
        public Vector3 TargetVelocity;
        public bool HaveTarget = false;
        public float powerbounce = 1.0f;
        private new Rigidbody rigidbody;
        private LineRenderer lineRenderer;
        private HitBallController cueball;
        private Diaco.EightBall.Server.BilliardServer server;
        [SerializeField] private float SpeedBallCurrent = 0.0f;
        private Vector3 VlocityBallCurrent;
        public bool EnableYFix = true;
        [SerializeField] private float Y_Pos_Refrence;

        [SerializeField] private bool InMove = false;
        private Ray ray;
        private RaycastHit hit;
        public LayerMask layer;
       [SerializeField] private float MaxAngularvelocity = 150;


        
        void Start()
        {

            rigidbody = GetComponent<Rigidbody>();
            lineRenderer = GetComponent<LineRenderer>();
            cueball = FindObjectOfType<HitBallController>();
            cueball.OnHitBall += Ball_OnHitBall;
            cueball.OnFreazeBall += Cueball_OnFreazeBall;
            cueball.OnFristHit += Cueball_OnFristHit;
            server = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
           testSetting =  FindObjectOfType<BillardTestSetting>();
            if (testSetting)
                testSetting.OnChangeSetting += HitBallController_OnChangeSetting;
            //InvokeRepeating("CheckballMove", 0, gamemanager.SendRate.value);//
            SetYPositionRefrence();
        }

        private void HitBallController_OnChangeSetting(float arg1, float arg2, float arg3 ,float arg, float arg4,float arg6,float arg7)
        {
            SetSetting(arg1, arg2, arg3,arg, arg4,arg7);
        }

        private void FixedUpdate()
        {
            VlocityBallCurrent = rigidbody.velocity;
            SpeedBallCurrent = rigidbody.velocity.magnitude;

            rigidbody.maxAngularVelocity = MaxAngularvelocity;

            /* if (HaveTarget && SpeedBallCurrent > ThresholdSleep )
             {
                 var d = (TargetVelocity - rigidbody.velocity).normalized;
                 rigidbody.velocity = d * SpeedBallCurrent;

             }*/
          
        }
        void LateUpdate()
        {
            if (EnableYFix)
                FixOverflowMovment();
        }


        private void OnDestroy()
        {

            if (cueball != null)
            {
                cueball.OnHitBall -= Ball_OnHitBall;
                cueball.OnFreazeBall -= Cueball_OnFreazeBall;
            }
            if (testSetting)
                testSetting.OnChangeSetting -= HitBallController_OnChangeSetting;

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
            if(active == true)
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rigidbody.isKinematic = active;
            }
            else if(active == false)
            {
                rigidbody.isKinematic = active;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                
            }
            
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
        private void Ball_OnHitBall(int target, Vector3 dir)
        {

            if (target != -1)
            {
                if (target == ID)
                {
                    lineRenderer.enabled = true;
                    SetlineDirection(dir);
    
                }
                else if (target != ID)
                {
                 //   lineRenderer.enabled = false;
                 //   SetlineDirection(new Vector3(0, -10f, 0));
      
                }
            }
            else
            {
              //  lineRenderer.enabled = false;
              //  SetlineDirection(new Vector3(0, -10f, 0));
              

            }
        }


        private void SetlineDirection(Vector3 pos)
        {
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, 0.06f, transform.position.z));

            lineRenderer.SetPosition(1, (new Vector3(pos.x, 0.06f, pos.z)));

        }
        public void SetYPositionRefrence()
        {
            DOVirtual.Float(0, 1, 2.0f, (x) => { }).OnComplete(() =>
             {

             }).OnComplete(() =>
             {
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

                  //  Debug.Log("Fix Y Ball");
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

               // Debug.Log("Fix Move Ball");

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
            rigidbody.velocity = (reflect2 * collision.relativeVelocity.magnitude)*powerbounce;
          //  Debug.Log("wall");

        }

        private void  SetSetting(float PowerCue, float Drag, float AngularDrag, float MaxAngular,float SpeedThershold,float powbounce)
        {
            MaxAngularvelocity = MaxAngular;
            rigidbody.drag = Drag;
            rigidbody.angularDrag = AngularDrag;
            this.ThresholdSleep = SpeedThershold;
            this.powerbounce = powbounce;

        }
    }

}