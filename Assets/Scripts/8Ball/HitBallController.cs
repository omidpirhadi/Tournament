﻿using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using DG.Tweening;


namespace Diaco.EightBall.CueControllers
{
    public class HitBallController : MonoBehaviour
    {
        public BillardTestSetting testSetting;
        public Diaco.EightBall.Server.BilliardServer Server;
        public SoundEffectControll soundeffectControll;
        public CueSpinController CueSpin;
        public EnergyCUEController EnergyCue;
        public AimController AimControllerUI;
        public NavigationUI UI;
        public SpriteRenderer LargeCueBall;
        public SpriteRenderer HandIcon;
        public LayerMask mask_for_move_cue_ball;
        public LayerMask mask_for_move_Aim;
        public LayerMask mask_for_Line_Aim;
        public float MinDistancePointTouchToCueBall = 3;

        public bool LimitedMovePitok = false;
        public enum Type_Ball { White, Color };
        public int ID = 0;

        // public Type_Ball TypeBall;
        public ForceMode Forcemode;
        public float maxanguler;
        public GameObject CUEWood;
        public Transform CueRenderer;
        public float PowerCUE;
        public float PowerSpin;
        //  private float temp_PowerCUE;
        //  private float temp_PowerSpin;
        /// public Vector3 OffsetPositionCueWoodFromCueBall;

        public GameObject GhostBall;
        private float RadiusGhostBall;
        public float RadiusGhostBallScaleFactor = 0.94f;///94%
        public float cueAim = 1;
        public float AimOffset = 3;
        // public Vector3 LastTouchPosition;
        public bool AimSystemShow = true;
        public CustomLineRenderer2 AimLine;
        public float ScaleLineAimGhostBall = 2f;
        public float SensitivityRotate;
        public float PowerBounceOnWall = 1.0f;
        private Vector2 PosCueSpin;
        private new Rigidbody rigidbody;
        //***private LineRenderer lineRenderer;
        private RaycastHit hit, hit2,hit3;
        private Vector3 VlocityBall;

        //public Vector3 LastPosition;
        private int count_imapct;
        private int IntergatioShowAnimation = 0;
        public bool DragIsBusy = false;
        public bool inPlayPos = false;
        public bool TouchWorkInUI = false;
        private float last_value_cue_energy = 0;
        private float timetouch = 0;

        /// private float temp_PowerCUE;
        /// private float temp_PowerSpin;
        private Ray ray_line;
        float prevAngle;
        public bool EnableYFix = true;
        [SerializeField] private bool waitForAim;
        //[SerializeField] private bool waitForAim2;
        public float Y_Pos_Refrence;
        public float ThresholdSleep = 0.09f;
        public bool InMove = false;
        // public bool TEsTaimDir = false;
        public Vector3 LastPosition;
        public Vector3 LastRotation;
        public Vector3 vvv;
        private float powscalefactor;
       [SerializeField] private bool CueBallMoveInPitoke = false;
        void Start()
        {
            // temp_PowerCUE = PowerCUE;
            //  temp_PowerSpin = PowerSpin;
            AimLine = GetComponent<CustomLineRenderer2>();
            rigidbody = GetComponent<Rigidbody>();
            //***lineRenderer = GetComponent<LineRenderer>();

            Server = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
            //  CueSpin = FindObjectOfType<CueSpinController>();
            // EnergyCue= FindObjectOfType<EnergyCUEController>();
            Server.OnTurn += HitBallController_OnTurn;
            // Server.OnPitok += Gamemanager_OnPitok;
            // rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            EnergyCue.OnChangeEnergy += HitBallController_OnChangeEnergy;
            EnergyCue.OnEnergyTouchEnd += HitBallController_OnEnergyTouchEnd;
            EnergyCue.OnBeginChangeEnergy += EnergyCue_OnBeginChangeEnergy;
            AimControllerUI.OnChangeValueAimControll += AimControllerUI_OnChangeValueAimControll;

            CueSpin.OnChangeValueSpin += HitBallController_OnChangeValueSpin;
            //  testSetting = FindObjectOfType<BillardTestSetting>();
            //  testSetting.OnChangeSetting += HitBallController_OnChangeSetting;

            UI.OnUIActive += UI_OnUIActive;
            /* if (Server.Turn)
                 ActiveAimSystem(true);
             else
                 ActiveAimSystem(false);*/
            SetYPositionRefrence();
            //CUEWoodSetPosition(new Vector3(1445.30f, 550.35f, 6.63f));
            //  Debug.Log("cCcCCCcCC");

            LastPosition = this.transform.position;
            LastRotation = this.transform.eulerAngles;
        }



        void LateUpdate()
        {
            RadiusGhostBall = (GetComponent<SphereCollider>().radius * transform.localScale.x) * RadiusGhostBallScaleFactor;
            if (EnableYFix)
                FixOverflowMovment();
        }
        public void FixedUpdate()
        {


           if( rigidbody.SweepTest(Vector3.right, out hit3, 20))
            {
                Debug.Log(hit3.collider.name);
            }
            VlocityBall = rigidbody.velocity;

            if (waitForAim && CheckMoveBall() == false)
            {
                EnergyCue.Show(true);
                AimControllerUI.Show(true);

                if (Server.Pitok > 0)
                {
                    CheckPitok();

                }
                soundeffectControll.PlaySound(1);///play sound change turn
                Handheld.Vibrate();
                waitForAim = false;
            }
            if ((CheckMoveBall() == true && inPlayPos) || DragIsBusy)
            {


                ActiveAimSystem111(false);
                ///  Debug.Log("Move1111");

            }
            else if (!DragIsBusy)
            {

                ActiveAimSystem111(true);

                ///Debug.Log("Stop111" );
            }


            AimSystem();

            TouchOrderControll();
            if (CueBallMoveInPitoke == true && Server.Pitok>0)
                CueBallMoveInPitokTouchController();
            else
                CueRotate();
            //CueBallMoveInPitoke = false;




            LastPosition = this.transform.position;
            LastRotation = this.transform.eulerAngles;
        }
        private void OnCollisionEnter(Collision collision)
        {

            if (collision.collider.tag == "ball" && count_imapct == 0)
            {

                /*var RelativeVelocity = this.rigidbody.velocity - vvv;
                  var Normal = this.rigidbody.position - collision.rigidbody.position;
                  float dot = Vector3.Dot(RelativeVelocity, Normal);
                  dot *= this.rigidbody.mass + collision.rigidbody.mass;
                  Normal *= dot;
                  this.rigidbody.velocity += Normal / this.rigidbody.mass;
                  collision.rigidbody.velocity -= Normal / collision.rigidbody.mass;*/


                // var friction = collision.relativeVelocity.magnitude * collision.collider.material.staticFriction;
                if (vvv.magnitude > 0)
                {
                    vvv.y = 0;

                    collision.rigidbody.velocity = (vvv.normalized) * (collision.relativeVelocity.magnitude);

                   /* collision.rigidbody.AddForceAtPosition((vvv.normalized) * (collision.relativeVelocity.magnitude)
                        , collision.contacts[0].point,
                    ForceMode.Force);
                    collision.rigidbody.velocity = ((vvv.normalized) * (collision.relativeVelocity.magnitude))*collision.rigidbody.mass;*/

                    /*collision.rigidbody.AddForceAtPosition(
                        collision.impulse/Time.fixedDeltaTime, 
                        collision.contacts[0].point,
                        ForceMode.Force);*/

                    //  collision.rigidbody.velocity = (vvv.normalized);
                    // Debug.Log("WhiteToBallXxXXXxX11");
                }

                Server.FirstBallImpact = collision.collider.GetComponent<AddressBall>().IDPost;
                count_imapct++;

            }
            if (collision.collider.tag == "wall")
                BounceBall(collision);

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GhostBall.transform.position, RadiusGhostBall);

        }

        private void UI_OnUIActive(bool obj)
        {
            if (obj == true)
            {
                TouchWorkInUI = true;
            }
            else
            {
                TouchWorkInUI = false;
            }
        }



        private void HitBallController_OnChangeValueSpin(Vector2 pos)
        {
            PosCueSpin = pos;
            //  TouchWorkInUI = false;
        }


        public void HitBallController_OnTurn(bool turn)
        {

            if (turn)
            {
                // ActiveAimSystem(true);
                /// CUEWoodSetPosition(LastTouchPosition);
                //  Debug.Log("TRUEEEEE");
                if (CheckMoveBall() == true)
                {
                    waitForAim = true;
                }
                else
                {
                    EnergyCue.Show(true);
                    AimControllerUI.Show(true);

                    if (Server.Pitok > 0)
                    {
                        CheckPitok();

                    }
                    soundeffectControll.PlaySound(1);///play sound change turn
                    Handheld.Vibrate();
                }
                last_value_cue_energy = 0;
                count_imapct = 0;
            }
            else if (turn == false)
            {
                ///ActiveAimSystem(false);
                EnergyCue.Show(false);
                AimControllerUI.Show(false);
                HandIcon.enabled = false;
                CancelAnimationHand();

                if (LargeCueBall.enabled)
                    LargeCueBall.enabled = false;


                Handler_OnHitBall(-1, Vector3.zero);
            }
            TouchWorkInUI = false;
            // resetpos();

        }

        private void EnergyCue_OnBeginChangeEnergy()
        {
            TouchWorkInUI = true;
        }
        private void HitBallController_OnChangeEnergy(float x)
        {
            if (Server.Turn /*&& Server.Record == false*/)
            {
                TouchWorkInUI = true;
                var unit = Mathf.Abs(last_value_cue_energy - x);
                int navigaion = 0;
                if (x > last_value_cue_energy)
                {

                    //   Debug.Log("big");
                    last_value_cue_energy = x;
                    navigaion = -1;
                }
                else
                {
                    //  Debug.Log("small");
                    last_value_cue_energy = x;
                    navigaion = 1;
                }


                CueRendererMove(navigaion, unit, 0.5f);
                //InvokeRepeating("CheckballMove", 0, gamemanager.SendRate.value); 

            }
        }
        private void force(float step)
        {
           /// TouchWorkInUI = false;
            if (step > 0)
            {
                float t_step = step / 5.0f;
                if (t_step == 1)
                {
                    var rand = UnityEngine.Random.Range(0.0f, PowerCUE * 0.015f);
                    ForceToBall((PowerCUE + rand) * t_step, PowerSpin);
                }
                else
                {
                    ForceToBall(PowerCUE * t_step, PowerSpin);
                }


                soundeffectControll.PlaySound(0);

                Server.Turn = false;
                LimitedMovePitok = false;
                Handler_OnHitBall(-1, Vector3.zero);
                Handler_ResetCueSpin();
                PosCueSpin = Vector2.zero;
                CancelInvoke("HandAnimationOnPitok");
                DragIsBusy = true;
            }
        }
        private void HitBallController_OnEnergyTouchEnd(float step)
        {
            if (Server.Turn /*&& Server.Record == false*/)
            {

                if (Server.GamePlayRule == EightBall.Server._GamePlayRule.classic)
                {
                    force(step);
                }
                else if (Server.GamePlayRule == EightBall.Server._GamePlayRule.quick)
                {
                    if (Server.EightBallEnable && Server.PocketSelected != 0)
                    {
                        force(step);
                    }
                    else if (!Server.EightBallEnable)
                    {
                        force(step);
                    }
                }
                else if (Server.GamePlayRule == EightBall.Server._GamePlayRule.big)
                {
                    if (Server.PlayerShar == Structs.Shar.None)
                    {
                        force(step);
                    }
                    else if (Server.PocketSelected != 0)
                    {
                        force(step);
                    }

                }

                
                // OffsetPositionCueWoodFromCueBall = new Vector3(-0.3f, 0.0f, 0.00f);
            }
            TouchWorkInUI = false;
        }
        private void AimControllerUI_OnChangeValueAimControll(float value)
        {
            // Debug.Log("AimControllerChange");
            CUEWood.transform.Rotate(0f, value, 0f);
            Server.Emit_AimCueBall(new Diaco.EightBall.Structs.AimData { X_position = CUEWood.transform.position.x, Z_position = CUEWood.transform.position.z, YY_rotation = CUEWood.transform.eulerAngles.y, PosCueBall = this.transform.position });
        }
        private void HitBallController_OnChangeSetting(float arg1, float arg2, float arg3, float arg, float arg4, float arg6, float arg7)
        {
            SetSetting(arg1, arg2, arg3, arg, arg4, arg6, arg7);
            Debug.Log("ChengeAccept");
        }

        private void TouchOrderControll()
        {
            if (Input.touchCount > 0 && Server.Turn && TouchWorkInUI == false)
            {

                Touch touch = Input.GetTouch(0);
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit_touch;

                if (Physics.Raycast(ray, out hit_touch, 1000, mask_for_move_Aim))
                {
                    var hitpos = new Vector3(hit_touch.point.x, 00, hit_touch.point.z);
                    var cueball_pos = new Vector3(transform.position.x, 00, transform.position.z);
                    var dist = Vector3.Distance(hitpos, cueball_pos);
                    if (dist < MinDistancePointTouchToCueBall && Server.Pitok>0)
                    {
                        CueBallMoveInPitoke = true;
                    }
                   

                }
            }
        }

        private void CueBallMoveInPitokTouchController()
        {
            if (Server.Pitok > 0 && Server.Turn)
            {
                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {

                        DragIsBusy = true;
                        LargeCueBall.transform.position = new Vector3(this.transform.position.x, 0.64f, this.transform.position.z);
                        LargeCueBall.enabled = true;
                        IntergatioShowAnimation = 1;
                        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                        rigidbody.isKinematic = true;
                        GetComponent<ShodowFake>().shadow.gameObject.SetActive(false);
                        ActiveAimSystem(false);
                        CancelAnimationHand();
                        Handler_FreazeBall(true);
                        Handler_OnHitBall(-1, Vector3.zero);

                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        DragCueBallInPitokWithTouch(touch);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        CueBallMoveInPitoke = false;
                        LargeCueBall.enabled = false;
                        IntergatioShowAnimation = 1;
                        rigidbody.isKinematic = false;
                        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        DragIsBusy = false;
                        ActiveAimSystem(true);
                        Handler_FreazeBall(false);
                        CancelAnimationHand();
                        GetComponent<ShodowFake>().shadow.gameObject.SetActive(true);
                        Server.Emit_PositionCueBallInPitoks(new Structs.CueBallData { position = this.transform.position, isDrag = DragIsBusy });
                        Debug.Log("CueBallMOve" + CueBallMoveInPitoke);

                    }
                }
            }
        }

        private void DragCueBallInPitokWithTouch(Touch touch)
        {


            if (Server.Turn)
            {
                if (Server.Pitok > 0 && !LimitedMovePitok)

                {
                    DragIsBusy = true;
                    //var touch = Input.GetTouch(0);
                    var ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.SphereCast(ray, RadiusGhostBall, out hit, 1000, mask_for_move_cue_ball))
                    {

                        bool find = false;

                        var limited0_x = Mathf.Clamp(hit.point.x, -5.1f, +5.94f);
                        var limited0_z = Mathf.Clamp(hit.point.z, -2.64f, 2.64f);
                        Vector3 pos_point = new Vector3(limited0_x, 0.0f, limited0_z);

                        // Debug.Log("Touch" +hit.collider.name+":::"+ hit.point);
                        var col = Physics.OverlapSphere(pos_point, RadiusGhostBall, mask_for_move_cue_ball).ToList();

                        col.ForEach((e) =>

                        {
                            if (e.tag == "ball")
                            {
                                find = true;
                                // Debug.Log("cccccc" + e.tag);
                            }
                        });
                        if (find == false)

                        {
                            var limited_x = Mathf.Clamp(hit.point.x, -5.1f, +5.94f);
                            var limited_z = Mathf.Clamp(hit.point.z, -2.64f, 2.64f);
                            transform.DOMove(new Vector3(limited_x, transform.position.y, limited_z), 00.01f, false);
                            LargeCueBall.transform.DOMove(new Vector3(limited_x, 0.64f, limited_z), 00.01f, false);
                            // Debug.Log("Touch" + hit.collider.name + ":::" + hit.point);
                            Server.Emit_PositionCueBallInPitoks(new Structs.CueBallData { position = this.transform.position, isDrag = DragIsBusy });


                        }

                    }

                }
                else if (Server.Pitok > 0 && LimitedMovePitok)
                {
                    DragIsBusy = true;
                    //var touch = Input.GetTouch(0);
                    var ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.SphereCast(ray, RadiusGhostBall, out hit, 1000, mask_for_move_cue_ball))
                    {

                        bool find = false;
                        var limited0_x = Mathf.Clamp(hit.point.x, -5.1f, -2.47f);
                        var limited0_z = Mathf.Clamp(hit.point.z, -2.64f, 2.64f);
                        Vector3 pos_point = new Vector3(limited0_x, 0.0f, limited0_z);
                        /// Debug.Log("Touch" +hit.collider.name+":::"+ hit.point);
                        var col = Physics.OverlapSphere(pos_point, RadiusGhostBall, mask_for_move_cue_ball).ToList();

                        col.ForEach((e) =>
                        {
                            if (e.tag == "ball")
                            {
                                find = true;
                            }
                        });
                        if (find == false)

                        {
                            var limited_x = Mathf.Clamp(hit.point.x, -5.1f, -2.47f);
                            var limited_z = Mathf.Clamp(hit.point.z, -2.64f, 2.64f);
                            LargeCueBall.transform.DOMove(new Vector3(limited_x, 0.64f, limited_z), 00.01f, false);
                            transform.DOMove(new Vector3(limited_x, transform.position.y, limited_z), 00.01f, false);

                            Server.Emit_PositionCueBallInPitoks(new Structs.CueBallData { position = this.transform.position, isDrag = DragIsBusy });

                            /// Debug.Log("PITOOKKKK LLLLIIIIIII");
                        }

                    }
                }

            }
        }
        private void CueRotate()
        {
            float curAngle = prevAngle;
          //  Debug.Log($"touchCount:{Input.touchCount}++ServerTurn:{Server.Turn}++DragISBusy:{DragIsBusy}+++CueBallMoveInPitoke:{CueBallMoveInPitoke}");
            if (Input.touchCount > 0 && Server.Turn && !DragIsBusy)
            {
               // Debug.Log("TTTTT11111");
                Touch touch = Input.GetTouch(0);
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit_touch;

                if (Physics.Raycast(ray, out hit_touch, 100, mask_for_move_Aim))
                {


                    if (TouchWorkInUI == false)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {

                            timetouch = Time.realtimeSinceStartup;
                            Vector2 valveScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
                            Vector2 touchDirectionScreen = touch.position - valveScreenPosition;
                            Vector3 touchDirectionWorld = Camera.main.ScreenToWorldPoint(touchDirectionScreen);


                            curAngle = prevAngle = Angle(Vector2.up, touchDirectionScreen);
                            // dragging = true;
                            if (Server.Pitok > 0)
                            {
                                CancelAnimationHand();
                                HandIconShow(0.0f);
                            }
                           /// Debug.Log("TTTTT2222");
                        }
                        else if (touch.phase == TouchPhase.Moved)
                        {
                            //  if (dragging)
                            // {
                            Vector2 valveScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
                            curAngle = Angle(Vector2.up, touch.position - valveScreenPosition);

                            float angularDistance = ShortestDistance(curAngle, prevAngle);
                            // accumulatedRotation += angularDistance;

                            CUEWood.transform.Rotate(0f, -angularDistance * SensitivityRotate, 0f); ;
                            //   }
                            Server.Emit_AimCueBall(new Diaco.EightBall.Structs.AimData { X_position = CUEWood.transform.position.x, Z_position = CUEWood.transform.position.z, YY_rotation = CUEWood.transform.eulerAngles.y, PosCueBall = this.transform.position });
                          //  Debug.Log("TTTTT3333");
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {

                            if (Server.Pitok > 0)
                            {
                                /// StartAnimationHand();
                                Server.Emit_AimCueBall(new Diaco.EightBall.Structs.AimData { X_position = CUEWood.transform.position.x, Z_position = CUEWood.transform.position.z, YY_rotation = CUEWood.transform.eulerAngles.y, PosCueBall = this.transform.position });

                            }
                            var temp_time_touch = Mathf.Abs(timetouch - Time.realtimeSinceStartup);

                            if (temp_time_touch < 0.1f)
                            {
                                LookAtFinger(touch.position);
                                Server.Emit_AimCueBall(new Diaco.EightBall.Structs.AimData { X_position = CUEWood.transform.position.x, Z_position = CUEWood.transform.position.z, YY_rotation = CUEWood.transform.eulerAngles.y, PosCueBall = this.transform.position });

                            }
                           // Debug.Log("TTTTT4444");
                        }
                    }

                }
            }
            prevAngle = curAngle;
        }
        /* public void initialze()
         {
             // temp_PowerCUE = PowerCUE;
             // temp_PowerSpin = PowerSpin;
             rigidbody = GetComponent<Rigidbody>();
             lineRenderer = GetComponent<LineRenderer>();

             Server = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
             Server.OnTurn += HitBallController_OnTurn;
             //  Server.OnPitok += Gamemanager_OnPitok;
             // rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
             FindObjectOfType<EnergyCUEController>().OnChangeEnergy += HitBallController_OnChangeEnergy;
             FindObjectOfType<EnergyCUEController>().OnEnergyTouchEnd += HitBallController_OnEnergyTouchEnd;
             FindObjectOfType<CueSpinController>().OnChangeValueSpin += HitBallController_OnChangeValueSpin;
            if (Server.Turn)
                 ActiveAimSystem(true);
             else
                 ActiveAimSystem(false);
             //SetYPositionRefrence();

         }*/
        private void CheckPitok()
        {
            if (Server.Pitok == 1)
            {
                IntergatioShowAnimation = 0;
                HandIcon.enabled = true;
                HandIcon.transform.position = new Vector3(transform.position.x, 0.64f, transform.position.z);

                StartAnimationHand();
                LimitedMovePitok = false;
                /// Debug.Log("AAAAA");
            }
            else if (Server.Pitok == 2)
            {
                IntergatioShowAnimation = 0;
                HandIcon.enabled = true;
                HandIcon.transform.position = new Vector3(transform.position.x, 0.64f, transform.position.z);
                StartAnimationHand();
                LimitedMovePitok = true;
                //Debug.Log("BBBBBB");
            }
        }


        //public Vector3 AtPosition;
        private void ForceToBall(float powcue, float powspin)
        {
            rigidbody.maxAngularVelocity = maxanguler;
            var dir = (transform.position - CueRenderer.position).normalized;

           var AtPosition = new Vector3(
                (transform.localPosition.x + ((PosCueSpin.x / powspin) * RadiusGhostBall) * dir.z),
                (transform.localPosition.y + ((PosCueSpin.y / powspin) * RadiusGhostBall)),
                (transform.localPosition.z + ((PosCueSpin.x / powspin) * RadiusGhostBall) * -dir.x));

            // Debug.Log($"AtPosition:{AtPosition}Dir:{dir}");
            var dir2 = (GhostBall.transform.position - transform.position).normalized;
            dir2.y = 0;
            rigidbody.AddForceAtPosition(dir2 * powcue, AtPosition, Forcemode);
            rigidbody.velocity = dir2 * powcue;

            if (Server.InRecordMode == false)
            {
                StartCoroutine(Server.PositionsBallsSendToServer());
            }
            else
            {
                StartCoroutine(Server.CheckMovementAndSendDataInRecordMode());
            }
        }

        float ShortestDistance(float curAngle, float prevAngle)
        {
            float distance = curAngle - prevAngle;

            if (distance > 180f)
            {
                distance -= 360f;
                //Debug.Log(distance);
            }
            else if (distance < -180f)
            {
                distance += 360f;
                //Debug.Log(distance);
            }

            return distance;
        }

        float Angle(Vector2 toVector2, Vector2 fromVector2)
        {
            float ang = Vector2.Angle(fromVector2, toVector2);
            Vector3 cross = Vector3.Cross(fromVector2, toVector2);

            if (cross.z > 0)
                ang = 360 - ang;

            return ang;
        }
        public void CueRendererMove(int navigation, float unitmove, float sensiviti)
        {
            var dir = transform.position - CueRenderer.transform.position;
            var dir_n = dir.normalized;
            var u = unitmove * sensiviti;
           // Debug.Log((navigation * dir_n) * u);
            CueRenderer.Translate((navigation * dir_n) * u, Space.World);

        }

        public void LookAtFinger(Vector3 mousepos)
        {

            var center = Camera.main.WorldToScreenPoint(transform.position);

            var dir = (mousepos - center);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //var angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(-angle, Vector3.up);
            // CUEWood.transform.position = Camera.main.ScreenToWorldPoint(center) + q * OffsetPositionCueWoodFromCueBall;
            CUEWood.transform.rotation = q;



        }

        public void SetCueWoodPositionAndRotationFromServer(Diaco.EightBall.Structs.AimData aimdata)
        {
            CUEWood.transform.position = new Vector3(aimdata.X_position, CUEWood.transform.position.y, aimdata.Z_position);
            CUEWood.transform.eulerAngles = new Vector3(CUEWood.transform.rotation.x, aimdata.YY_rotation, CUEWood.transform.rotation.z);
        }

        public void AimSystem()
        {
            if (AimSystemShow)
            {
                float MagnitudVector = 0;
                var dir = transform.position - CueRenderer.position;
                dir.y = 0;
                dir = dir.normalized;

                ray_line.origin = transform.position;
                ray_line.direction = dir;

                ///**** lineRenderer.SetPosition(0, transform.position);


                if (Physics.SphereCast(ray_line, RadiusGhostBall, out hit2, 20, mask_for_Line_Aim))
                {
                    
                 ///   Vector3 g_end = new Vector3();
                 //   Vector3 b_end = new Vector3();
                    if (hit2.collider && hit2.collider.GetComponent<Ball>())
                    {   
                        if (Server.CheckBallForAllowHit(hit2.collider.GetComponent<AddressBall>().IDPost))
                        {
                            ////GhostBallChangeAndSetPos///
                            if (Server.Turn)
                                GhostBall.GetComponent<GhostBallControll>().ChangeImage(true);
                            else
                                GhostBall.GetComponent<GhostBallControll>().ChangeImage(true);
                            // GhostBall.transform.position = hit2.point + RadiusGhostBall * hit2.normal;/*hit2.point + Vector3.ClampMagnitude(hit2.point - transform.position, -0.15f);*/
                            GhostBall.transform.position = ray_line.origin + (ray_line.direction.normalized * hit2.distance);


                            AimLine.SetPosition(GhostBall.transform.position);

                            ///Draw  Line WhiteBall Dir
                         //   var dir_ghostball_to_whiteball = hit2.point - transform.position;
                            // var scale = (transform.position - hit2.point).magnitude / 180.0f;///
                            var rotate90_R = Quaternion.AngleAxis(-90, Vector3.up);
                            var rotate90_L = Quaternion.AngleAxis(90, Vector3.up);
                            var dir_right = rotate90_R * hit2.normal;
                            var dir_left = rotate90_L * hit2.normal;
                            var angle_right = Vector3.Angle(GhostBall.transform.position - transform.position, dir_right);
                            var angle_left = Vector3.Angle(GhostBall.transform.position - transform.position, dir_left);
                           // Debug.Log("++++"+(angle_left - angle_right));
                            float a;

                            if (angle_right > angle_left)
                            {
                                //  var d = Vector3.Min(dir_right, dir_left);
                                 a = (angle_right - angle_left) * ScaleLineAimGhostBall;
                                var Ghostline = GhostBall.GetComponent<CustomLineRenderer2>();
                                Ghostline.enabled = true;
                                Ghostline.SetPosition((GhostBall.transform.position + dir_left * a));

                               // line.SetPosition(0, GhostBall.transform.position);

                              //  line.SetPosition(1, (GhostBall.transform.position + dir_left * a));
                                MagnitudVector = (GhostBall.transform.position + dir_left * a).magnitude;
                               // Debug.Log("L"+a);
                                Debug.DrawRay(GhostBall.transform.position, dir_left * a, Color.green);////absolut
                             //   g_end = GhostBall.transform.position + dir_left * a;
                            }
                            else 
                            {
                                 a = (angle_left - angle_right) * ScaleLineAimGhostBall;

                                var Ghostline = GhostBall.GetComponent<CustomLineRenderer2>();
                                Ghostline.enabled = true;
                                Ghostline.SetPosition((GhostBall.transform.position + dir_right * a));
                                /*var line = GhostBall.GetComponent<LineRenderer>();
                                line.enabled = true;
                                line.SetPosition(0, GhostBall.transform.position);
                                line.SetPosition(1, (GhostBall.transform.position + dir_right * a));*/
                                MagnitudVector = (GhostBall.transform.position + dir_right * a).magnitude;
                                ///Debug.Log("R" + a);
                                Debug.DrawRay(GhostBall.transform.position, dir_right * a, Color.cyan);////absolut

                            ///    g_end = GhostBall.transform.position + dir_left * a;
                            }
                            
                            ////Dir  Other ball
                            ///
                            Vector3 dir_ghostballTo_targetball;

                            dir_ghostballTo_targetball = hit2.transform.position - hit2.point;

                            powscalefactor = (180 * ScaleLineAimGhostBall - a);
                            Vector3 pos3 = hit2.transform.position + (dir_ghostballTo_targetball.normalized * powscalefactor);

                            vvv = (hit2.transform.position + (AimOffset + 30 * 0.25f) * dir_ghostballTo_targetball) - hit2.transform.position;
                            Handler_OnHitBall(hit2.collider.GetComponent<AddressBall>().IDPost,pos3 );
                            Debug.DrawLine(GhostBall.transform.position, pos3, Color.blue);
                        
                        }
                        else
                        {
                            if (Server.Turn)
                            {
                                GhostBall.GetComponent<GhostBallControll>().ChangeImage(false);

                                GhostBall.transform.position = ray_line.origin + (ray_line.direction.normalized * hit2.distance);
                                //****lineRenderer.SetPosition(0, transform.position);
                               //**** lineRenderer.SetPosition(1, GhostBall.transform.position);

                               var dir_ghostballTo_targetball = hit2.transform.position - hit2.point;
                                vvv = (hit2.transform.position + (AimOffset + 30 * 0.25f) * dir_ghostballTo_targetball) - hit2.transform.position;
                                 Handler_OnHitBall(-1, Vector3.zero);

                            }
                            else
                            {
                                GhostBall.GetComponent<GhostBallControll>().ChangeImage(true);


                                GhostBall.transform.position = ray_line.origin + (ray_line.direction.normalized * hit2.distance);
                                //****lineRenderer.SetPosition(0, transform.position);
                                //****lineRenderer.SetPosition(1, GhostBall.transform.position);
                                ///Draw  Line WhiteBall Dir
                                var dir_ghostball_to_whiteball = hit2.point - transform.position;
                                ///  var scale = (transform.position - hit2.point).magnitude / 180.0f;///
                                var rotate90_R = Quaternion.AngleAxis(-90, Vector3.up);
                                var rotate90_L = Quaternion.AngleAxis(90, Vector3.up);
                                var dir_right = rotate90_R * hit2.normal;
                                var dir_left = rotate90_L * hit2.normal;
                                var angle_right = Vector3.Angle(GhostBall.transform.position - transform.position, dir_right);
                                var angle_left = Vector3.Angle(GhostBall.transform.position - transform.position, dir_left);
                                float a;
                                if (angle_right > angle_left)
                                {
                                    //  var d = Vector3.Min(dir_right, dir_left);
                                     a = (angle_right - angle_left) * ScaleLineAimGhostBall;
                                    var line = GhostBall.GetComponent<LineRenderer>();
                                    line.enabled = true;
                                    line.SetPosition(0, GhostBall.transform.position);
                                    line.SetPosition(1, GhostBall.transform.position + dir_left * a);
                                   
                                    Debug.DrawRay(GhostBall.transform.position, dir_left * a, Color.green);////absolut

                                }
                                else
                                {
                                     a = (angle_left - angle_right) * ScaleLineAimGhostBall;
                                    var line = GhostBall.GetComponent<LineRenderer>();
                                    line.enabled = true;
                                    line.SetPosition(0, GhostBall.transform.position);
                                    line.SetPosition(1, GhostBall.transform.position + dir_right * a);
                                   
                                    Debug.DrawRay(GhostBall.transform.position, dir_right * a, Color.cyan);////absolut
                                }

                                ////Dir  Other ball

                                Vector3 dir_ghostballTo_targetball;


                                dir_ghostballTo_targetball = hit2.transform.position - hit2.point;

                                powscalefactor = (180 * ScaleLineAimGhostBall - a);
                                Vector3 pos3 = hit2.transform.position + (dir_ghostballTo_targetball.normalized * powscalefactor);
                                Debug.DrawLine(GhostBall.transform.position, pos3, Color.blue);
                                // hit2.collider.GetComponent<Ball>().SetlineDirection(dir_ghostballTo_targetball + hit2.transform.position);
                               
                                vvv = (hit2.transform.position + (AimOffset + 30 * 0.25f) * dir_ghostballTo_targetball) - hit2.transform.position;
                                Handler_OnHitBall(hit2.collider.GetComponent<AddressBall>().IDPost, pos3);
                                ///  b_end = hit2.transform.position + (AimOffset + cueAim * 0.25f) * dir_ghostballTo_targetball;

                            }
                        }
                    }
                    else if (hit2.collider && !hit2.collider.GetComponent<Ball>())
                    {
                        ////GhostBallChangeAndSetPos///
                        GhostBall.transform.position = ray_line.origin + (ray_line.direction.normalized * hit2.distance);
                        //***lineRenderer.SetPosition(0, transform.position);
                        AimLine.SetPosition(GhostBall.transform.position);
                        GhostBall.GetComponent<GhostBallControll>().ChangeImage(true);
                        ///Draw  Line WhiteBall Dir
                        var dir_ghostball_to_whiteball = hit2.point - transform.position;
                        ////   var scale = (transform.position - hit2.point).magnitude / 180.0f;////
                        var rotate90_R = Quaternion.AngleAxis(-90, Vector3.up);
                        var rotate90_L = Quaternion.AngleAxis(90, Vector3.up);
                        var dir_right = rotate90_R * hit2.normal;
                        var dir_left = rotate90_L * hit2.normal;
                        var angle_right = Vector3.Angle(GhostBall.transform.position - transform.position, dir_right);
                        var angle_left = Vector3.Angle(GhostBall.transform.position - transform.position, dir_left);
                        if (angle_right > angle_left)
                        {
                            //  var d = Vector3.Min(dir_right, dir_left);
                            float a = (angle_right - angle_left) * ScaleLineAimGhostBall;

                            var Ghostline = GhostBall.GetComponent<CustomLineRenderer2>();

                            Ghostline.Reset();
                            Ghostline.enabled = false;

                            Debug.DrawRay(GhostBall.transform.position, dir_left * a, Color.green);

                        }
                        else if (angle_right < angle_left)
                        {
                            float a = (angle_left - angle_right) * ScaleLineAimGhostBall;

                            var Ghostline = GhostBall.GetComponent<CustomLineRenderer2>();
                            
                            Ghostline.Reset();
                            Ghostline.enabled = false;
                            Debug.DrawRay(GhostBall.transform.position, dir_right * a, Color.cyan);
                        }
                        ///  LastTouchPosition = Camera.main.WorldToScreenPoint(hit2.point);
                         Handler_OnHitBall(-1, Vector3.zero);
                        vvv = Vector3.zero;
                    }
                }
            }

        } 
        [Obsolete]
        public void ActiveAimSystem(bool show)
        {
           
           /* 
      
            if (show == false)
            {
                AimSystemShow = show;
                CUEWood.SetActive(show);
                GhostBall.SetActive(show) ;
                EnergyCue.Show(show);
                AimControllerUI.Show(show);
                HandIcon.enabled = show;
                CancelAnimationHand();


                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
                last_value_cue_energy = 0;
                Debug.Log("DisableAim");
            }
            else if (show == true)
            {

                if (Server.Turn && CheckBallMove() ==  false)
                {
                     
                    AimSystemShow = show;
                    CUEWood.SetActive(show);
                    GhostBall.SetActive(show);
                    EnergyCue.Show(show);
                    AimControllerUI.Show(show);
                    if (Server.Pitok > 0)
                    {
                        CheckPitok();

                    }
                    waitForAim = false;
                   resetpos();
                    Debug.Log("ActiveAim");

                }
                else
                {
                    waitForAim = true;
                    Debug.Log("WaitActiveAim");
                }
                last_value_cue_energy = 0;
                 Handler_OnHitBall(-1, Vector3.zero);

            }*/
        }
        public void ActiveAimSystem111(bool show)
        {



            if (show == false)
            {
                if (AimSystemShow == true)
                {
                    
                    AimSystemShow = show;
                    CUEWood.SetActive(show);
                    GhostBall.SetActive(show);


                    //**** lineRenderer.SetPosition(0, Vector3.zero);
                    //*** lineRenderer.SetPosition(1, Vector3.zero);
                    AimLine.enabled = (show);
                  //  Debug.Log("AimDisable"+DragIsBusy);
                }
            }
            else if (show == true)
            {

                if (AimSystemShow == false)
                {
                   // DragIsBusy = false;
                    AimSystemShow = show;
                    CUEWood.SetActive(show);
                    GhostBall.SetActive(show);
                    AimLine.enabled = show;
                    //  Debug.Log("AimActive" +DragIsBusy);
                }
            }

        }
        [Obsolete]
        public void ActiveAimSystemForShowInOtherClient(bool show)
        {
           
          /*
            if (show == true)
            {
                if (AimSystemShow == false)
                {
                    if (!Server.Turn && CheckBallMove() == false)
                    {

                        AimSystemShow = show;
                        CUEWood.SetActive(show);
                        GhostBall.SetActive(show);
                        //HandIcon.enabled = show;
                        last_value_cue_energy = 0;
                        Debug.Log("ActiveAimXXX2");
                        waitForAim2 = false;
                        resetpos();
                    }
                    else
                    {
                        waitForAim2 = true;
                        Debug.Log("WaitActivexxxAim");
                    }
                }
               // Debug.Log("ActiveAimXXX44");
            }
            else if (show == false)
            {
                if (AimSystemShow == true)
                {
                    AimSystemShow = show;
                    CUEWood.SetActive(show);
                    GhostBall.SetActive(show);
                    // HandIcon.enabled = show;


                    lineRenderer.SetPosition(0, Vector3.zero);
                    lineRenderer.SetPosition(1, Vector3.zero);
                    Handler_OnHitBall(-1, Vector3.zero);
                    Debug.Log("DisableAimXXx");
                }
            }
            */
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
        private  void FixOverflowMovment()
        {
            if (Y_Pos_Refrence > 0.0f)
            {
                var conflict_Y = Mathf.Abs(transform.position.y - Y_Pos_Refrence);
                if (conflict_Y > 0.1f)
                {

                    transform.position = new Vector3(transform.position.x, Y_Pos_Refrence, transform.position.z);

                   // Debug.Log("Fix Y Ball");
                }
             
            }
            if (CheckBallMove() == true  && InMove == false)
            {
                DOVirtual.Float(0, 1, 1.0f, (x) => { }).OnComplete(() =>
                {

                }).OnComplete(() =>
                {
                   
                    InMove = true;
                });
            }
            if (VlocityBall.magnitude < ThresholdSleep && VlocityBall.magnitude > 0.001f && InMove == true)
            {

                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                InMove = false;
            //    Debug.Log("Fix Move Ball");
                
            }
        }

        public void CueBallMoveFromServer(Diaco.EightBall.Structs.CueBallData data, float speed)
        {

            transform.DOMove(data.position, speed).OnComplete(() => { DragIsBusy = data.isDrag; });
           // Debug.Log("XDrag"+DragIsBusy);
            Handler_OnHitBall(-1, Vector3.zero);

        }
      
        public void resetpos()
        {

            CUEWood.transform.position = this.transform.position;
            CueRenderer.localPosition = new Vector3(-8.5f, 1.0f, 0.0f);
            ///GhostBall.transform.position = this.transform.position;
           // HandIcon.transform.position = new Vector3(this.transform.position.x + 0.37f, 0.64f, this.transform.position.z);
           // Debug.Log("ReSETpOSS"); 
        }
        private void HandIconShow(float alpha)
        {
            
                HandIcon.DOColor(new Color(1, 1, 1, alpha), 1f);
        }
        private void HandAnimationOnPitok()
        {
            if (IntergatioShowAnimation == 0)
            {
                DOVirtual.Float(0.0f, 1.0f, 1.0f, (x) =>
                {
                    HandIconShow(x);
                }).OnComplete(() =>
                {
                    DOVirtual.Float(1.0f, 0.0f, 1.0f, (x) =>
                    {
                        HandIconShow(x);
                    });
                });
            }
        }
        private void StartAnimationHand()
        {
            if (Server.Turn)
                InvokeRepeating("HandAnimationOnPitok", 0, 2);
        }
        private void CancelAnimationHand()
        {
            CancelInvoke("HandAnimationOnPitok");
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
        private void BounceBall(Collision collision)
        {



            var normal = collision.contacts[0].normal;
            var reflect2 = Vector3.Reflect(VlocityBall.normalized, normal).normalized;
            rigidbody.velocity = (reflect2 * collision.relativeVelocity.magnitude) * PowerBounceOnWall;
           // Debug.Log("wall");

        }

        /// <summary>
        /// Workon Button SmalllSpin and LargViweSpin In UI 
        /// </summary>
        /// <param name="active">Workon Button SmalllSpin and LargViweSpin In UI </param>
        public void WorkTouchInUI(bool active)
        {
            TouchWorkInUI = active;
        }


        private void SetSetting(float pow, float Drag, float AngularDrag,float MaxAngularDrag, float SpeedThershold,float sensivityrotate ,  float powbounce)
        {
            this.maxanguler = MaxAngularDrag;
            this.PowerCUE = pow;
            rigidbody.drag = Drag;
            rigidbody.angularDrag = AngularDrag;
            this.ThresholdSleep = SpeedThershold;
            this.SensitivityRotate = sensivityrotate;
            this.PowerBounceOnWall = powbounce;
        }
        #region Events

        public event Action<int , Vector3> OnHitBall;

        public  void Handler_OnHitBall(int TargetBall, Vector3 Direction )
        {
            if (OnHitBall != null)
            {
                OnHitBall(TargetBall, Direction);
            }
        }
      
        public event Action<bool> OnFreazeBall;
        protected void Handler_FreazeBall(bool active)
        {
            if (OnFreazeBall != null)
            {
                OnFreazeBall(active);
            }
        }
        public event Action ResetCueSpin;
        protected void Handler_ResetCueSpin()
        {
            if (ResetCueSpin != null)
            {
                ResetCueSpin();
            }
        }
        public event Action<int> OnFristHit;
        protected void Handler_OnFristHit(int id)
        {
            if(OnFristHit != null)
            {
                OnFristHit(id);
            }
        }
        #endregion



    }
}