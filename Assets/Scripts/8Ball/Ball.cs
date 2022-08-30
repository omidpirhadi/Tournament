using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Diaco.EightBall.CueControllers
{
    public class Ball : MonoBehaviour
    {
       // public BillardTestSetting testSetting;
        public int ID = 0;
        public float ThresholdSleep = 0.09f;
        public Vector3 TargetVelocity;
        public bool HaveTarget = false;
        public bool IsCueBallMove = false;
        public float powerbounce = 1.0f;
        public bool EnableYFix = true;

        private int intergatDestinationBall = 0;
        private new Rigidbody rigidbody;
        // private LineRenderer lineRenderer;
        private CustomLineRenderer2 AimLine;

        private HitBallController cueball;
      //  private Diaco.EightBall.Server.BilliardServer server;
       // [SerializeField] private float SpeedBallCurrent = 0.0f;
        private Vector3 VlocityBallCurrent;
        
        [SerializeField] private float Y_Pos_Refrence;

        [SerializeField] private bool InMove = false;
        //private Ray ray;
       private RaycastHit hit;
       public LayerMask layer;
       [SerializeField] private float MaxAngularvelocity = 150;

        public Vector3 LastPosition;
        private Vector3 LastPositionInFrame20;
        public Vector3 LastRotation;
        public float GetSpeed;
         private int frame;

        void Start()
        {

            rigidbody = GetComponent<Rigidbody>();
            AimLine = GetComponent<CustomLineRenderer2>();
            cueball = FindObjectOfType<HitBallController>();
            cueball.OnHitBall += Ball_OnHitBall;
            cueball.OnFreazeBall += Cueball_OnFreazeBall;
            cueball.OnShotCueBall += Cueball_OnShotCueBall;
            cueball.OnMissTarget += Cueball_OnMissTarget;
          /* testSetting =  FindObjectOfType<BillardTestSetting>();
            if (testSetting)
                testSetting.OnChangeSetting += HitBallController_OnChangeSetting;*/

            //SetYPositionRefrence();
            rigidbody.maxAngularVelocity = MaxAngularvelocity;
        }

        private void Cueball_OnMissTarget()
        {
            ClearDestinationBall();
        }




        #region MonoFunctions
        private void FixedUpdate()
        {
            VlocityBallCurrent = rigidbody.velocity;
            GetSpeed = VlocityBallCurrent.magnitude;
            TravelToTarget();

          

            LastPosition = this.transform.position;
            LastRotation = this.transform.eulerAngles;

            if (frame >= 20 && GetSpeed<0.1f)
            {
                LastPositionInFrame20 = LastPosition;
                CheckMoveWithDistanceFromLastPosition();
                frame = 0;

            }

            frame++;

   
        }
        void LateUpdate()
        {
           /* if (EnableYFix)
                FixOverflowMovment();

           

            DisableFixY();*/
        }


        private void OnDestroy()
        {

            if (cueball != null)
            {
                cueball.OnHitBall -= Ball_OnHitBall;
                cueball.OnFreazeBall -= Cueball_OnFreazeBall;
                cueball.OnShotCueBall -= Cueball_OnShotCueBall;
            }
          /*  if (testSetting)
                testSetting.OnChangeSetting -= HitBallController_OnChangeSetting;*/

        }

        private void OnCollisionEnter(Collision collision)
        {
            if(HaveTarget && intergatDestinationBall == 0)
            {
                intergatDestinationBall = 1;
            }
            else
            {
                HaveTarget = false;
                intergatDestinationBall = 0;
            }
            if (collision.collider.tag == "wall")
                BounceBall(collision);
            
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }

        #endregion
        #region CallBackFunctions
        private void Cueball_OnFreazeBall(bool active)
        {
            if (active == true)
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rigidbody.isKinematic = active;
            }
            else if (active == false)
            {
                rigidbody.isKinematic = active;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            }

            // Debug.Log("Firize");
        }
        private void Ball_OnHitBall(int target, Vector3 point)
        {

            if (target != -1)// we have a target
            {
                if (target == ID) // target = this ball and TurnOn Aim
                {
                    AimLine.enabled = true;
                    SetlineDirection(point);
                    SetDestinationBall(point);
                }
                else if (target != ID) // target != this ball and TurnOff Aim
                {
                    ClearDestinationBall();
                    AimLine.Reset();
                    AimLine.enabled = false;
                    
                    
                }
            }
            else // target  Empty and clear All Aim
            {
                //ClearDestinationBall();
                AimLine.Reset();
                AimLine.enabled = false;
                

            }
        }
        /*private void HitBallController_OnChangeSetting(float arg1, float arg2, float arg3, float arg, float arg4, float arg6, float arg7)
        {
            SetSetting(arg1, arg2, arg3, arg, arg4, arg7);
        }*/
        private void Cueball_OnShotCueBall()
        {
            IsCueBallMove = true;
        }
        #endregion



        private void SetlineDirection(Vector3 pos)
        {

            AimLine.SetPosition(new Vector3(pos.x, 0.06f, pos.z));

        }
        public void SetYPositionRefrence()
        {
            DOVirtual.Float(0, 1, 5.0f, (x) => { }).OnComplete(() =>
             {
                 Y_Pos_Refrence = transform.position.y;
             //    Debug.Log("YDDDdD");
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
                IsCueBallMove = false;
                ClearDestinationBall();
               // Debug.Log("Fix Move Ball");

            }
        }
        private void DisableFixY()
        {
            float minX = -5.35f;
            float maxX = 6.2f;
            float minZ = -2.98f;
            float maxZ = 2.98f;
            if ((LastPosition.x< minX || LastPosition.x > maxX)  || (LastPosition.z < minZ || LastPosition.z > maxZ))
            {
                    EnableYFix = false;
            //    Debug.Log("DisableY" + this.name + "LAST" + LastPosition);

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
                Debug.Log("Move");
            }


        }
        public bool OnlyCheckMove()
        {


            var dis = Vector3.Distance(transform.position, LastPositionInFrame20);
            if (dis <= SensivityCheckMovment)
            {

                return false;
            }
            else
            {
                InMove = true;

                return true;
            }

        }
        public void StopMovment()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            InMove = false;
            rigidbody.Sleep();
            // Debug.Log("Stoped");
        }


        private void BounceBall(Collision collision)
        {
            var normal = collision.contacts[0].normal;
            var reflect2 = Vector3.Reflect(VlocityBallCurrent.normalized, normal).normalized;
            rigidbody.velocity = (reflect2 * collision.relativeVelocity.magnitude)*powerbounce;
          //  Debug.Log("wall");

        }

        private void SetDestinationBall(Vector3 point)
        {
            HaveTarget = true;
            intergatDestinationBall = 0;
            var dir = point - transform.position;
            if (Physics.Raycast(transform.position, dir, out hit, 100, layer))
            {
                TargetVelocity = dir.normalized;
                TargetVelocity.y = 0;
            }
        }
        private void ClearDestinationBall()
        {

            HaveTarget = false;
            intergatDestinationBall = 0;
            TargetVelocity = Vector3.zero;
        }
        private void TravelToTarget()
        {
            if (HaveTarget && IsCueBallMove)
                rigidbody.velocity = TargetVelocity * rigidbody.velocity.magnitude;
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