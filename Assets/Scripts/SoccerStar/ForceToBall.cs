using System;
//sing System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using Diaco.SoccerStar.Server;
using Sirenix.OdinInspector;

using Diaco.SoccerStar.CustomTypes;

namespace Diaco.SoccerStar.Marble
{


    public class ForceToBall : MonoBehaviour
    {
        public float TEST_Force = 0.0f;
        // public SoccerTestSettings TestSetting ;
        public int ID;
        public enum Marble_Type { Marble, Ball };
        public Marble_Type MarbleType;
        public ForceMode forceMode;

        [Range(110, 1000000000)]
        public float PowerForce;
        // public float AccelerationBallAfterHit = 3.0f;
        public SelectEffectController SelectEffect;
        /// public float SelectEffectRotateSenciviti = 1.5f;

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


        public new Rigidbody rigidbody;

        private ServerManager server;

        private TempPlayerControll playerControll;
        private AimCircle aim;

        public Vector3 GetVlocity;
        public float GetSpeed;

        //// private Vector3 LastPos;
        private float Y_Pos_Refrence = 0.0f;

        //private float StepPower;
        // private Vector3 DirectionMove;
        private Vector3 LastPosition;
        private Vector3 LastPositionInFrame20;
        public Vector3 LastVelocity;
        private Vector3 LastRotation;
        private RaycastHit hitwall;

        //  public LayerMask hitwall_layer;
        public float MaxDisFromWall = 5.00f;
        [SerializeField] private Vector3 hitpointballtomarbl;
        [SerializeField] private Vector3 hitpointballtowall;
        public float bouncepower = 1;
        #region MonoBehaviour Function
        public void Start()
        {
            server = FindObjectOfType<ServerManager>();

            rigidbody = GetComponent<Rigidbody>();
            if (server.Info.userName == server.gameData.playerOne.userName)
                PowerForce = Mathf.Clamp(server.gameData.playerOne.force, 140, 220);
            else
                PowerForce = Mathf.Clamp(server.gameData.playerTwo.force, 140, 220);
            ///TestSetting = FindObjectOfType<SoccerTestSettings>();
            ///  TestSetting.OnChangeSetting += TestSetting_OnChangeSetting;

            if (MarbleType == Marble_Type.Marble)
            {
                server.OnChangeTurn += Server_OnChangeTurn;
                // server.EnableRingMarbleForOpponent += Server_EnableRingMarbleForOpponent;
                playerControll = FindObjectOfType<TempPlayerControll>();
                aim = FindObjectOfType<AimCircle>();
                // playerControll.EnableSelectRingEffect += PlayerControll_EnableSelectRingEffect;
                aim.EnableRingEffectOwner += PlayerControll_EnableRingEffectOwner;
                aim.EnableRingEffectOpponent += PlayerControll_EnableRingEffectOpponent;
                playerControll.OnShoot += PlayerControll_OnShoot;

            }
            server.OnPhysicFreeze += Server_OnPhysicFreeze;

            LastPosition = this.transform.position;
            LastPositionInFrame20 = LastPosition;
            LastRotation = this.transform.eulerAngles;
        }

        int frame = 0;
        private void FixedUpdate()
        {


            GetVlocity = rigidbody.velocity;

            GetSpeed = GetVlocity.magnitude;


            if (IsRotatingMarble)
                RotateMarble();
            if (IsRotateBall)
                RotateBall();




           
            
            if (frame >= 20 && GetSpeed<0.1f )
            {
                LastPositionInFrame20 = LastPosition;
                CheckMoveWithDistanceFromLastPosition();
                frame = 0;

            }
            LastPosition = this.transform.position;
            LastRotation = this.transform.eulerAngles;
            frame++;
            // WallHit();
        }

        private void LateUpdate()
        {

        }
        /// <summary>
        /// ******************** تغییرررررر کرده یادت باشه 
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter(Collision collision)
        {
            var tag_collider = collision.collider.tag;

            if (MarbleType == Marble_Type.Marble)
            {

                if (tag_collider == "wall")
                {

                    IsRotatingMarble = true;
                    Debug.Log("rotateMarble On Wall");

                }
                if (tag_collider == "ball")
                {

                    collision.gameObject.GetComponent<ForceToBall>().IsRotateBall = true;

                }


                if (tag_collider == "marble")
                {


                    IsRotatingMarble = true;
                }
            }
            else if (MarbleType == Marble_Type.Ball)
            {
                if (tag_collider == "wall")
                {

                    IsRotateBall = true;


                }

            }
        }

        public void OnDestroy()
        {

            if (MarbleType == Marble_Type.Marble)
            {
                aim.EnableRingEffectOwner -= PlayerControll_EnableRingEffectOwner;
                aim.EnableRingEffectOpponent -= PlayerControll_EnableRingEffectOpponent;
                playerControll.OnShoot -= PlayerControll_OnShoot;
                server.OnChangeTurn -= Server_OnChangeTurn;
            }
            server.OnPhysicFreeze -= Server_OnPhysicFreeze;
            /// TestSetting.OnChangeSetting -= TestSetting_OnChangeSetting;
        }

        public void OnDrawGizmos()
        {

        }

        #endregion
        #region CallBackFunctions
        private void PlayerControll_EnableRingEffectOpponent(bool obj)
        {
            SelectEffectEnableForOpponent(obj);

        }

        private void PlayerControll_EnableRingEffectOwner(bool obj)
        {
            SelectEffectEnable(obj);
        }
        private void TestSetting_OnChangeSetting(float MassMarble, float ForceMarble, float DragMarble, float AngularDragMarble, float MassBall, float DragBall, float AngularDragBall, float speedtheshold, float bouncewwall)
        {
            if (MarbleType == Marble_Type.Marble)
            {
                this.rigidbody.mass = MassMarble;
                this.PowerForce = ForceMarble;
                this.rigidbody.drag = DragMarble;
                this.rigidbody.angularDrag = AngularDragMarble;
                //  this.AccelerationBallAfterHit = AccelerationMarbleAfterhit;
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
            this.bouncepower = bouncewwall;
            //// Debug.Log("AAAAAAAAA3");
        }

        private void Server_OnPhysicFreeze(bool obj)
        {
            PhysicFreeze(obj);
        }
        private void Server_OnChangeTurn(bool obj)
        {
            // SelectEffectEnable(obj);
            // Debug.Log("TURN SelectEffectEnable");
        }
        private void PlayerControll_OnShoot(int marbleID, Vector3 dir, float pow)
        {
            if (marbleID == ID)
            {
                // DirectionMove = dir; 
                // StepPower = pow;
                Move(dir, pow);

            }
        }
        #endregion




        /*private void WallHit()
        {



            if (rigidbody.SweepTest(GetVlocity, out hitwall, 10))
            {

                var dis = Vector3.Distance(transform.position, hitwall.point);
                // Debug.Log(dis);

                if (hitwall.collider.tag == "wall")
                {
                    if (dis < MaxDisFromWall)
                    {


                        LastVelocity = GetVlocity;
                        // GetComponent<SoundManagerMarble>().PlaySound()
                        // Debug.Log("WallHit");
                    }
                }
                // Debug.Log("swap test :" + hitwall.collider.name);
            }


            Debug.DrawRay(transform.position, GetVlocity, Color.yellow);
        }*/
        public void Move(Vector3 dir, float pow)
        {
            server.IsGoal = -1;
            server.EnablerRingEffect = false;
            var d_n = new Vector3(dir.x, 0, dir.z).normalized;
            var P_F = SoftFloat.Soft((PowerForce) * pow);
            //  GetDirectionForce = d_n * (float)P_F;
            // PositionBeforForce = this.transform.position;
            
            rigidbody.AddForce(d_n * (float)P_F, forceMode);
            if (server.InRecordMode == false)
            {
                StartCoroutine(server.SendDataMarblesMovement());
                // InMove = true;
            }

            else
            {
                server.StartCheckMovmentInRecordMode();
            }


        }
        public void SelectEffectEnable(bool Active)
        {
            //  Debug.Log("r4");
            if (MarbleType == Marble_Type.Marble)
            {
                if (server.EnablerRingEffect && server.InRecordMode == false)
                {
                    ///  Debug.Log("r5");
                    if (CheckOwnerMarble())
                    {
                        SelectEffect.enabled = Active;

                        ///  Debug.Log("SelectEffectOwner");
                    }

                }
                else
                {
                    // Debug.Log("r5-2");
                    if (CheckOwnerMarble())
                    {
                        SelectEffect.enabled = Active;

                        //  Debug.Log("SelectEffectOwner");
                    }
                }

            }
        }
        public void SelectEffectEnableForOpponent(bool Active)
        {
            // Debug.Log("aaaszx");
            if (MarbleType == Marble_Type.Marble)
            {
                if (server.EnablerRingEffect)
                {
                    //Debug.Log("34343");
                    if (!CheckOwnerMarble())
                    {
                        SelectEffect.enabled = Active;
                    }
                }
                else if (server.InRecordMode)
                {
                    if (!CheckOwnerMarble())
                    {
                        SelectEffect.enabled = Active;
                    }
                }
            }
        }
        /* public void SetYPositionRefrence()
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

              });
         }*/
        /* private void FixOverflowMovment()
         {
             if (Y_Pos_Refrence > 0.0f)
             {
                 var conflict_Y = Mathf.Abs(transform.position.y - Y_Pos_Refrence);
                 if (conflict_Y > 0.1f)
                 {

                     transform.position = new Vector3(transform.position.x, Y_Pos_Refrence, transform.position.z);

                     /// Debug.Log("Fix Y Ball");
                 }



             }
             if (CheckMoveBall() == true && InMove == false)
             {

                 InMove = true;

             }
             if (GetVlocity.magnitude < ThresholdSleep && GetVlocity.magnitude > 0.001f && InMove == true)
             {

                 rigidbody.velocity = Vector3.zero;
                 rigidbody.angularVelocity = Vector3.zero;


                 IsRotatingMarble = false;
                 IsRotateBall = false;
                 InMove = false;
                 // Debug.Log(VlocityBall.magnitude + ":::Fix Move Ball");

             }

         }*/
        /*private bool CheckBallMove()
        {
            var move = false;

            if (rigidbody.velocity.magnitude >ThresholdSleep || rigidbody.angularVelocity.magnitude >ThresholdSleep)
            {

                move = true;
            }




            return move;
        }*/

        public bool CheckOwnerMarble()
        {
            bool c = false;
            if (ID >= server.MinRangMarblesId && ID <= server.MaxRangMarblesId)
            {
                c = true;
            }
            return c;
        }

        public void SetSkinMarble(Sprite skin)
        {

            Flagrenderer.sprite = skin;
        }

        /* private void BounceBall(Collision collision)
         {


             var dir = hitpointballtowall-hitpointballtomarbl;
             var normal = collision.contacts[0].normal;

             var reflect2 = Vector3.Reflect(GetVlocity, normal).normalized;
              var reflect3 = Vector3.Reflect(dir, normal).normalized;


             if (GetVlocity.magnitude <= 4.0f)
             {
                rigidbody.velocity = reflect3 * 5;
                /// rigidbody.AddForce(reflect3 * 2000, ForceMode.Force);
                 Debug.Log("zero");
             }
            else
             {
                 rigidbody.velocity = reflect2 * collision.relativeVelocity.magnitude;
                 Debug.Log("normal");
             }
           /*  if (GetSpeed > ThresholdSleep)
             {
                 rigidbody.velocity = reflect2 * collision.relativeVelocity.magnitude;
                 Debug.Log("normal");
             }
             // Debug.Log("Wall" + reflect2 * collision.relativeVelocity.magnitude);
             //Debug.Log(distance + "wallvelocity:" + GetVlocity);

         }*/
        /*private void BounceMarble(Collision collision)
        {

            var normal = collision.contacts[0].normal;
            var reflect2 = Vector3.Reflect(LastVelocity, normal).normalized;
            rigidbody.velocity = (reflect2 * LastVelocity.magnitude) * bouncepower;
            Debug.Log(collision.impulse.magnitude + "  BounceMarble:  " + reflect2 + "Last :  " + LastVelocity.magnitude);


        }*/
        private void RotateMarble()
        {
            //var speed = rigidbody.velocity.magnitude;
            var speed = Vector3.Distance(transform.position, LastPosition) / Time.deltaTime;
            /// Debug.Log(speed);
            if (speed > 0.0f)
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
            Vector3 velocity = (transform.position - LastPosition) / Time.deltaTime;
            Vector3 movement = velocity * Time.deltaTime;
            Vector3 aix = Vector3.Cross(normal, movement).normalized;

            float distance = movement.magnitude;
            float angle = distance * (180 / Mathf.PI) / StepRotateBall;
            /// Debug.Log(speed);
            transform.DOLocalRotateQuaternion(Quaternion.Euler(aix * angle) * transform.localRotation, DurationStep).SetEase(EaseTypeRotate);
            /// rigidbody.AddTorque((aix * angle)*AccelerationBallAfterHit,ForceMode.Impulse) ;
            //transform.localRotation =;

            // IsRotateBall = true;

        }

        public void RotateMarbleFromServer(bool Do)
        {
            IsRotatingMarble = Do;
        }
        public void RotateBallFromServer(bool Do)
        {

            IsRotateBall = Do;
        }

        public float SensivityCheckMovment = 0.001f;
        public void CheckMoveWithDistanceFromLastPosition()
        {


            var dis = Vector3.Distance(transform.position, LastPositionInFrame20);
            if (dis <= SensivityCheckMovment)
            {
                StopMovment();
            }
            else
            {
                InMove = true;

            }


        }
        public bool OnlyCheckMove()
        {


            var dis = Vector3.Distance(transform.position, LastPositionInFrame20);
            if (dis <= SensivityCheckMovment)
            {
               // StopMovment();
                //Debug.Log("Stoped" );
                return false;
            }
            else
            {
                InMove = true;
                // Debug.Log("Moving");
                return true;
            }

        }
        public  void StopMovment()
        {
            //if (InMove)
            // {
            InMove = false;
            IsRotatingMarble = false;
            IsRotateBall = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            GetVlocity = Vector3.zero;
            GetSpeed = 0;
            rigidbody.Sleep();

           // Debug.Log("stoped:" + gameObject.name);
            // }
        }
        private void PhysicFreeze(bool enable)
        {
            if (enable)

            {

                this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                this.rigidbody.isKinematic = true;
                this.GetComponent<Collider>().enabled = false;


             //   Debug.Log("PhysicFreeze");
            }

            else
            {
                this.GetComponent<Collider>().enabled = true;
                this.rigidbody.isKinematic = false;
                this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
              //  Debug.Log("Physic UnFreeze");

            }
        }


        public void SetMovmentData(Vec_Soccer p, Vec_Soccer v)
        {
            Vector3 pos = VectorHelper.ToVector3WithReversX(p);
            Vector3 velocity = VectorHelper.ToVector3WithReversX(v);
            this.rigidbody.velocity = velocity;
            this.rigidbody.position = pos;
        }
       /* public void MarbleStop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.Sleep();
        }*/
    }
}