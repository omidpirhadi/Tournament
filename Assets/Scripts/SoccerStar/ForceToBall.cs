using System;
//sing System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using Diaco.SoccerStar.Server;
using Sirenix.OdinInspector;

namespace Diaco.SoccerStar.Marble
{

    
    public class ForceToBall : MonoBehaviour
    {

        public SoccerTestSettings TestSetting ;
        public enum Marble_Type { Marble, Ball };

        public Marble_Type MarbleType;
        //public bool InRecordMode = false;
        public int ID;

        public ForceMode forceMode;

        [Range(110, 1000000000)]
        public float PowerForce;
        public float AccelerationBallAfterHit = 3.0f;
        public GameObject SelectEffect;
        public float SelectEffectRotateSenciviti = 1.5f;

        public SpriteRenderer Flagrenderer;
        public Transform Flag;

        public float StepRotateMarble = 0.1f;
        public bool IsRotatingMarble = false;
        public float StepRotateBall;
        public bool IsRotateBall = false;

        public float DurationStep = 0.1f;
        public Ease EaseTypeRotate;

         public float ThresholdSleep = 0.09f;
        public bool InMove = false;


        private new Rigidbody rigidbody;

        private ServerManager server;

        private TempPlayerControll playerControll;

       
        private Vector3 VlocityBall;
        
       //// private Vector3 LastPos;
       //[SerializeField] private float Y_Pos_Refrence = 0.0f;
        
        //private float StepPower;
       // private Vector3 DirectionMove;

        private RaycastHit hit;
        private Ray ray;
        public void Start()
        {
            server = FindObjectOfType<ServerManager>();
            TestSetting = FindObjectOfType<SoccerTestSettings>();
            rigidbody = GetComponent<Rigidbody>();
            
           

            if (MarbleType == Marble_Type.Marble)
            {
                server.OnChangeTurn += Server_OnChangeTurn;
               // server.EnableRingMarbleForOpponent += Server_EnableRingMarbleForOpponent;
                playerControll = FindObjectOfType<TempPlayerControll>();
                // playerControll.EnableSelectRingEffect += PlayerControll_EnableSelectRingEffect;
                playerControll.EnableRingEffectOwner += PlayerControll_EnableRingEffectOwner;
                playerControll.EnableRingEffectOpponent += PlayerControll_EnableRingEffectOpponent;
                playerControll.OnShoot += PlayerControll_OnShoot;

            }
            server.OnPhysicFreeze += Server_OnPhysicFreeze;
            TestSetting.OnChangeSetting += TestSetting_OnChangeSetting;
            SetYPositionRefrence();
           /* if(server.Turn)
            {
                SelectEffectEnable(true);
            }
            else
            {
                SelectEffectEnableForOpponent(true);
            }*/
        }

        private void PlayerControll_EnableRingEffectOpponent(bool obj)
        {
            SelectEffectEnableForOpponent(obj);
        }

        private void PlayerControll_EnableRingEffectOwner(bool obj)
        {
            SelectEffectEnable(obj);
        }

        void Update()
        {
          
       
        }
        void LateUpdate()
        {

          FixOverflowMovment();
        }
       
        private void FixedUpdate()
        {
            VlocityBall = rigidbody.velocity;
            if (IsRotatingMarble)
                RotateMarble();
            if (IsRotateBall)
                RotateBall();
           
        }


        void OnCollisionEnter(Collision collision)
        {
            var tag_collider = collision.collider.tag;
            if (MarbleType == Marble_Type.Marble)
            {

                if (tag_collider == "ball")
                {
                  ///  collision.gameObject.transform.LookAt(this.transform);
                    collision.gameObject.GetComponent<ForceToBall>().IsRotateBall = true;
                }
                if (tag_collider == "wall" || tag_collider == "marble" )
                    IsRotatingMarble = true;
            }
            else if (MarbleType == Marble_Type.Ball)
            {
                if (tag_collider == "wall")
                {

                  /////  transform.LookAt(collision.contacts[0].point);
                    IsRotateBall = true;
                    
                }
            }
        }

        private void OnEnable()
        {

        }
        public void OnDestroy()
        {

            if (MarbleType == Marble_Type.Marble)
            {

              //  playerControll.EnableSelectRingEffect -= PlayerControll_EnableSelectRingEffect;
                playerControll.OnShoot -= PlayerControll_OnShoot;
               // server.EnableRingMarbleForOpponent -= Server_EnableRingMarbleForOpponent;
                server.OnChangeTurn -= Server_OnChangeTurn;

            }
            server.OnPhysicFreeze -= Server_OnPhysicFreeze;
            TestSetting.OnChangeSetting -= TestSetting_OnChangeSetting;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var reflect = Vector3.Reflect(ray.direction, hit.normal);
            Gizmos.DrawRay(transform.position, ray.direction);
            Gizmos.DrawRay(hit.point, reflect);
        }

        private void TestSetting_OnChangeSetting(float MassMarble, float ForceMarble, float DragMarble, float AngularDragMarble, float AccelerationMarbleAfterhit, float MassBall, float DragBall, float AngularDragBall,float speedtheshold)
        {
            if(MarbleType == Marble_Type.Marble)
            {
                this.rigidbody.mass = MassMarble;
                this.PowerForce = ForceMarble;
                this.rigidbody.drag = DragMarble;
                this.rigidbody.angularDrag = AngularDragMarble;
                this.AccelerationBallAfterHit = AccelerationMarbleAfterhit;
               // Debug.Log("AAAAAAAAA1");
            }
            else
            {
                this.rigidbody.mass = MassBall;
                this.rigidbody.drag = DragBall;
                this.rigidbody.angularDrag = AngularDragBall;
               // Debug.Log("AAAAAAAAA2");
            }
            this.ThresholdSleep = speedtheshold;
           //// Debug.Log("AAAAAAAAA3");
        }

        private void Server_OnPhysicFreeze(bool obj)
        {
            PhysicFreeze(obj);
        }


        private void Server_EnableRingMarbleForOpponent( bool enable)
        {
           // SelectEffectEnableForOpponent(enable);
            Debug.Log("SelectEffectEnableForOpponent SelectEffectEnable");
        }

        private void PlayerControll_OnShoot(int marbleID,Vector3 dir, float pow)
        {
            if(marbleID == ID)
            {
               // DirectionMove = dir; 
               // StepPower = pow;
                Move(dir, pow);
                
            }
        }

        private void PlayerControll_EnableSelectRingEffect(bool obj)
        {
          //  SelectEffectEnable(obj);
            //Debug.Log("PlayerControll SelectEffectEnable");
        }

      

        private void Server_OnChangeTurn(bool obj)
        {
           // SelectEffectEnable(obj);
           // Debug.Log("TURN SelectEffectEnable");
        }





        public void Move(Vector3 dir, float pow)
        {


            var d_n = new Vector3(dir.x, 0, dir.z).normalized;
            var P_F = SoftFloat.Soft((PowerForce) * pow);

            rigidbody.AddForce(d_n * (float)P_F, forceMode);
            if (server.InRecordMode == false)

                StartCoroutine(server.SendDataMarblesMovement());
            else
                server.StarCheckMovment();



        }
        

        public void SetYPositionRefrence()
        {
            
           /* DOVirtual.Float(0, 1, 2.0f, (x) => { }).OnComplete(() =>
            {
               PhysicFreeze(false);
            }).OnComplete(() => {
                
                
                if (MarbleType == Marble_Type.Marble)
                {
                   
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationX  | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }
                else
                {
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }
                Y_Pos_Refrence = transform.position.y;
                if (server.Turn == false)
                    PhysicFreeze(true);
                else
                    PhysicFreeze(false);
                    
            });*/
        }
        private void FixOverflowMovment()
        {
           /* if (Y_Pos_Refrence > 0.0f)
            {
                var conflict_Y = Mathf.Abs(transform.position.y - Y_Pos_Refrence);
                if (conflict_Y > 0.1f)
                {

                    transform.position = new Vector3(transform.position.x, Y_Pos_Refrence, transform.position.z);

                    Debug.Log("Fix Y Ball");
                }


               
            }*/
            if (CheckBallMove() == true && InMove == false)
            {
                DOVirtual.Float(0, 1, 1.0f, (x) => { }).OnComplete(() =>
                {

                }).OnComplete(() =>
                {

                    InMove = true;
                    //Debug.Log("BallMove");
                });
            }
            if (VlocityBall.magnitude < ThresholdSleep  && VlocityBall.magnitude >0.001f && InMove == true)
            {

                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                //rigidbody.isKinematic = true;
                InMove = false;
                IsRotatingMarble = false;
                IsRotateBall = false;
                // frFlag = false;
                // Debug.Log(VlocityBall.magnitude + ":::Fix Move Ball");

            }
                
        }
        private bool CheckBallMove()
        {
            var move = false;

            if (rigidbody.velocity.magnitude >ThresholdSleep || rigidbody.angularVelocity.magnitude >ThresholdSleep)
            {

                move = true;
            }




            return move;
        }

        public bool CheckOwnerMarble()
        {
            bool c = false;
            if (ID >= server.MinRangMarblesId && ID <= server.MaxRangMarblesId)
            {
                c = true;
            }
            return c;
        }
        public void SelectEffectEnable(bool Active)
        {
           // Debug.Log("aaaszx");
            if (MarbleType == Marble_Type.Marble)
            {
                //Debug.Log("34343");
                if (CheckOwnerMarble())
                {
                    SelectEffect.SetActive(Active);

                    ///Debug.Log("SelectEffectOwner");
                }

             
               
            }
        }
        public void SelectEffectEnableForOpponent(bool Active)
        {
            // Debug.Log("aaaszx");
            if (MarbleType == Marble_Type.Marble)
            {
                //Debug.Log("34343");
                if (!CheckOwnerMarble())
                {
                    SelectEffect.SetActive(Active); //Debug.Log("sdasd");
                }

            }
        }
        public void SetSkinMarble(Sprite skin)
        {

            Flagrenderer.sprite = skin;
        }
       /* private void SoftPositionAndRotation()
        {
            DOVirtual.Float(0, 1, 0.5f, x =>
            {
              
            }).OnComplete(() => {
                var x_p = (float)SoftFloat.Soft(this.transform.position.x);
                var y_p = this.transform.position.y;
                var z_p = (float)SoftFloat.Soft(this.transform.position.z);

                var x_r = (float)SoftFloat.Soft(this.transform.eulerAngles.x);
                var y_r = (float)SoftFloat.Soft(this.transform.eulerAngles.y);
                var z_r = (float)SoftFloat.Soft(this.transform.eulerAngles.z);
              //  Debug.Log(new Vector3(x_p, y_p, z_p));
               // Debug.Log(new Vector3(x_r, y_r, z_r));
                this.transform.position = new Vector3(x_p, y_p, z_p);
                this.transform.eulerAngles = new Vector3(x_r, y_r, z_r);
            });

        }*/

        private void BounceBall(Collision collision)
        {



            var normal = collision.contacts[0].normal;
            var reflect2 = Vector3.Reflect(VlocityBall, normal).normalized;
            if (reflect2.magnitude > 0.0f)
                rigidbody.velocity = reflect2 * collision.relativeVelocity.magnitude;
            Debug.Log("Wall" + reflect2 * collision.relativeVelocity.magnitude);

        }




        private void RotateMarble()
        {
            var speed = rigidbody.velocity.magnitude;
           /// Debug.Log(speed);
            if (speed>0.0f)
            {

                //Flag.transform.localEulerAngles = new Vector3(90,  StepRotateMarble * speed, 0);
                // Flag.transform.DOLocalRotate(new Vector3(90, (Flag.transform.eulerAngles.y + StepRotateMarble) * speed, 0), DurationStep);

                DOVirtual.Float(0, 1, DurationStep, (x) =>
                {
                    Flag.transform.eulerAngles = new Vector3(90, (Flag.transform.eulerAngles.y) + StepRotateMarble * speed, 0);
                }).SetEase(EaseTypeRotate);
            }
        }
        private void RotateBall()
        {
            Vector3 normal = new Vector3(0, 1, 0);
            Vector3 movement = rigidbody.velocity *Time.fixedDeltaTime;
            Vector3 aix = Vector3.Cross(normal, movement).normalized;

            float distance = movement.magnitude;
            float angle = distance * (180 / Mathf.PI) / StepRotateBall;
            /// Debug.Log(speed);
            transform.DOLocalRotateQuaternion(Quaternion.Euler(aix * angle) * transform.localRotation, DurationStep).SetEase(EaseTypeRotate);
            //transform.localRotation =;

               // IsRotateBall = true;

        }
        private void PhysicFreeze(bool enable)
        {
            if (enable)

            {
                this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                this.rigidbody.isKinematic = true;
                

                Debug.Log("PhysicFreeze");
            }

            else
            {
                this.rigidbody.isKinematic = false;
                this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                Debug.Log("Physic UnFreeze");

            }
        }
    }
}