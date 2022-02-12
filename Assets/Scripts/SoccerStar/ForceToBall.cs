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

        ///public SoccerTestSettings TestSetting ;
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


        private new Rigidbody rigidbody;

        private ServerManager server;

        private TempPlayerControll playerControll;
        private AimCircle aim;

        public Vector3 GetVlocity;
        public float GetSpeed;
        private Vector3 GetDirectionForce;
        private Vector3 PositionBeforForce;
        //// private Vector3 LastPos;
        private float Y_Pos_Refrence = 0.0f;

        //private float StepPower;
        // private Vector3 DirectionMove;
        private Vector3 LastPosition;
        public Vector3 LastVelocity;
        private Vector3 LastRotation;
        private RaycastHit hit, hitwall;
        private Ray ray;
        public LayerMask hitwall_layer;
        public float MaxDisFromWall = 100f;
        [SerializeField] private Vector3 hitpointballtomarbl;
        [SerializeField] private Vector3 hitpointballtowall;
        #region MonoBehaviour Function
        public void Start()
        {
            server = FindObjectOfType<ServerManager>();
            //// TestSetting = FindObjectOfType<SoccerTestSettings>();
            rigidbody = GetComponent<Rigidbody>();



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
            // TestSetting.OnChangeSetting += TestSetting_OnChangeSetting;
            //SetYPositionRefrence();
            /* if(server.Turn)
             {
                 SelectEffectEnable(true);
             }
             else
             {
                 SelectEffectEnableForOpponent(true);
             }*/
            LastPosition = this.transform.position;
            LastRotation = this.transform.eulerAngles;
        }




        void LateUpdate()
        {

            
        }

        private void FixedUpdate()
        {
            // GetVlocity = rigidbody.velocity;

            GetVlocity = (this.transform.position - LastPosition) / Time.deltaTime;
            ///LastVelocity = GetVlocity; 
            GetSpeed = GetVlocity.magnitude;
            if (IsRotatingMarble)
                RotateMarble();
            if (IsRotateBall)
                RotateBall();
            FixOverflowMovment();

            WallHit();
            LastPosition = this.transform.position;
            LastRotation = this.transform.eulerAngles;
        }


        void OnCollisionEnter(Collision collision)
        {
            var tag_collider = collision.collider.tag;

            if (MarbleType == Marble_Type.Marble)
            {
                if (tag_collider == "wall")
                {
                    // BounceMarble(collision);
                    IsRotatingMarble = true;

                }
                if (tag_collider == "ball")
                {
                   // collision.gameObject.GetComponent<ForceToBall>().hitpointMarbletobal = collision.contacts[0].point;
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
                    ///  
                    /////  transform.LookAt(collision.contacts[0].point);
                   hitpointballtowall = collision.contacts[0].point;
                    IsRotateBall = true;
                   BounceBall(collision);

                }
               if (tag_collider == "marble")
                {
                     hitpointballtomarbl = collision.contacts[0].point;
                    

                }
            }
        }
        void OnCollisionStay(Collision collision)
        {
         //   Debug.Log("sSssSsSs");
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
            // TestSetting.OnChangeSetting -= TestSetting_OnChangeSetting;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var reflect = Vector3.Reflect(ray.direction, hit.normal);
            Gizmos.DrawRay(transform.position, ray.direction);
            Gizmos.DrawRay(hit.point, reflect);
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
        /* private void TestSetting_OnChangeSetting(float MassMarble, float ForceMarble, float DragMarble, float AngularDragMarble, float AccelerationMarbleAfterhit, float MassBall, float DragBall, float AngularDragBall, float speedtheshold)
         {
             if (MarbleType == Marble_Type.Marble)
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
         }*/

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




        private void WallHit()
        {


            if (MarbleType == Marble_Type.Marble)
            {
                if (rigidbody.SweepTest(GetVlocity, out hitwall, 1000))
                {

                    var dis = Vector3.Distance(transform.position, hitwall.point);
                    if (hitwall.collider.tag == "wall")
                    {
                        if (dis < MaxDisFromWall)
                        {
                            BounceMarble();
                            // Debug.Log("WallHit");
                        }
                    }
                    // Debug.Log("swap test :" + hitwall.collider.name);
                }
            }

            Debug.DrawRay(transform.position, GetVlocity, Color.yellow);
        }
        public void Move(Vector3 dir, float pow)
        {
            server.IsGoal = -1;
            server.EnablerRingEffect = false;
            var d_n = new Vector3(dir.x, 0, dir.z).normalized;
            var P_F = SoftFloat.Soft((PowerForce) * pow);
            GetDirectionForce = d_n * (float)P_F;
            PositionBeforForce = this.transform.position;
            rigidbody.AddForce(d_n * (float)P_F, forceMode);
            if (server.InRecordMode == false)

                StartCoroutine(server.SendDataMarblesMovement());
            else
                server.StarCheckMovment();



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
                        SelectEffect.enabled = Active; //Debug.Log("sdasd");
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
        private void FixOverflowMovment()
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

        }
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

        private void BounceBall(Collision collision)
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
            }*/
            // Debug.Log("Wall" + reflect2 * collision.relativeVelocity.magnitude);
            //Debug.Log(distance + "wallvelocity:" + GetVlocity);

        }
        private void BounceMarble()
        {

            var normal = hitwall.normal;
            
            
            
            var reflect2 = Vector3.Reflect(GetVlocity, normal).normalized;

            rigidbody.velocity = reflect2 * GetVlocity.magnitude;
            Debug.Log("BounceMarble");
            /*  if (GetSpeed > ThresholdSleep)
              {
                  rigidbody.velocity = reflect2 * collision.relativeVelocity.magnitude;
                  Debug.Log("normal");
              }*/
            // Debug.Log("Wall" + reflect2 * collision.relativeVelocity.magnitude);
            //Debug.Log(distance + "wallvelocity:" + GetVlocity);

        }
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

        private bool EqeulPosition(Vector3 a, Vector3 b)
        {
            bool eqeul = false;
            var x = a.x - b.x;
            var z = a.z - b.z;
            if (x == 0.0f && z == 0.0f)
            {
                eqeul = true;
            }
            //  LastPosition = this.transform.position;
            return eqeul;

        }
        private bool EqeulRotation(Vector3 a, Vector3 b)
        {
            bool eqeul = false;
            var x = a.x - b.x;
            var y = a.y - b.y;
            var z = a.z - b.z;
            if (x == 0.0f && y == 0.0f && z == 0.0f)
            {
                eqeul = true;
            }
            //// LastRotation = this.transform.eulerAngles;
            return eqeul;
        }
        public bool CheckMoveBall()
        {
            var move = false;
            var domove = EqeulPosition(transform.position, LastPosition);
            var dorotate = EqeulRotation(transform.eulerAngles, LastPosition);

            if (domove == false && dorotate == false)
            {

                move = true;
            }

            return move;
        }

        private void PhysicFreeze(bool enable)
        {
            if (enable)

            {

                this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                this.rigidbody.isKinematic = true;
                this.GetComponent<Collider>().enabled = false;


                Debug.Log("PhysicFreeze");
            }

            else
            {
                this.GetComponent<Collider>().enabled = true;
                this.rigidbody.isKinematic = false;
                this.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                Debug.Log("Physic UnFreeze");

            }
        }
    }
}