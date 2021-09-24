using System;
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
        public CueSpinController CueSpin;
        public EnergyCUEController EnergyCue;
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
        public Vector3 OffsetPositionCueWoodFromCueBall;

        public GameObject GhostBall;
        public float RadiusGhostBall;
        public float cueAim = 1;
        public float AimOffset = 3;
        // public Vector3 LastTouchPosition;
        public bool AimSystemShow = true;
        public float ScaleLineAimGhostBall = 2f;
        public float SensitivityRotate;
        public float powerbounce = 1.0f;
        private Vector2 PosCueSpin;
        private new Rigidbody rigidbody;
        private LineRenderer lineRenderer;
        private RaycastHit hit, hit2;
        private Vector3 VlocityBall;

        //public Vector3 LastPosition;
        private int count_imapct;
        private int IntergatioShowAnimation = 0;
        [SerializeField] private bool DragIsBusy = false;
        [SerializeField] private bool TouchWorkInUI = false;
        private float last_value_cue_energy = 0;
        private float timetouch = 0;

       /// private float temp_PowerCUE;
       /// private float temp_PowerSpin;
        private Ray ray_line;
        float prevAngle;
        public bool EnableYFix = true;
        private bool waitForAim;
        private bool waitForAim2;
        public float Y_Pos_Refrence;
        public float ThresholdSleep = 0.09f;
        public bool InMove = false;
        // public bool TEsTaimDir = false;
        public Vector3 vvv;
        void Start()
        {
          // temp_PowerCUE = PowerCUE;
          //  temp_PowerSpin = PowerSpin;
            rigidbody = GetComponent<Rigidbody>();
            lineRenderer = GetComponent<LineRenderer>();

            Server = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
            //  CueSpin = FindObjectOfType<CueSpinController>();
            // EnergyCue= FindObjectOfType<EnergyCUEController>();
            Server.OnTurn += HitBallController_OnTurn;
           // Server.OnPitok += Gamemanager_OnPitok;
            // rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            EnergyCue.OnChangeEnergy += HitBallController_OnChangeEnergy;
            EnergyCue.OnEnergyTouchEnd += HitBallController_OnEnergyTouchEnd;
            EnergyCue.OnBeginChangeEnergy += EnergyCue_OnBeginChangeEnergy;

            CueSpin.OnChangeValueSpin += HitBallController_OnChangeValueSpin;
            testSetting = FindObjectOfType<BillardTestSetting>();
            testSetting.OnChangeSetting += HitBallController_OnChangeSetting;

            UI.OnUIActive += UI_OnUIActive;
            if (Server.Turn)
                ActiveAimSystem(true);
            else
                ActiveAimSystem(false);
            SetYPositionRefrence();
            //CUEWoodSetPosition(new Vector3(1445.30f, 550.35f, 6.63f));
            //  Debug.Log("cCcCCCcCC");
            RadiusGhostBall = GetComponent<SphereCollider>().radius * transform.localScale.x;
        }

        

        void LateUpdate()
        {

            if (EnableYFix)
                FixOverflowMovment();
        }
        private void FixedUpdate()
        {


           
            VlocityBall = rigidbody.velocity;
            if (waitForAim)
            {
                ActiveAimSystem(true);
            }
            if (waitForAim2)
            {
                ActiveAimSystemForShowInOtherClient(true);
            }
            //CueRotate();
            TouchControll();
            AimSystem();



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

     
                
                if(vvv.magnitude >0)
                {
                    collision.rigidbody.velocity = vvv.normalized * collision.relativeVelocity.magnitude;
                  //  Debug.Log("WhiteToBall");
                }
             
                Server.FirstBallImpact = collision.collider.GetComponent<AddressBall>().IDPost;
                count_imapct++;

            }
            if (collision.collider.tag == "wall")
                BounceBall(collision);

        }
        private void OnMouseDown()
        {
           /* if (Server.Pitok > 0 && Server.Turn)
            {
                LargeCueBall.transform.position = new Vector3(this.transform.position.x, 0.64f, this.transform.position.z);
                LargeCueBall.enabled = true;
                ActiveAimSystem(false);
                IntergatioShowAnimation = 1;
                CancelAnimationHand();
                DragIsBusy = true;
                rigidbody.isKinematic = true;
                Handler_FreazeBall(true);

                // GetComponent<SphereCollider>().isTrigger = true;
                // Handler_FreazeBall(true);
                //Debug.Log("PITOOKKKK DOWN");
            }*/



        }
        private void OnMouseDrag()
        {
           // MoveCueBallInPitok();

            // Debug.Log("Drag");

        }
        private void OnMouseUp()
        {
           /* if (Server.Pitok > 0 && Server.Turn)
            {
                LargeCueBall.enabled = false;
                ActiveAimSystem(true);

                IntergatioShowAnimation = 1;



                //GetComponent<SphereCollider>().isTrigger = false;
                rigidbody.isKinematic = false;
                Handler_FreazeBall(false);
                DragIsBusy = false;
                CancelAnimationHand();
                ///    Debug.Log("PITOOKKKK UP");
                Server.Emit_PositionCueBallInPitoks(this.transform.position);
            }*/
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(AtPosition, 0.2f);

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


        private void HitBallController_OnTurn(bool turn)
        {

            if (turn)
            {
                ActiveAimSystem(true);
                /// CUEWoodSetPosition(LastTouchPosition);
                //  Debug.Log("TRUEEEEE");
                count_imapct = 0;
            }
            else if (turn == false)
            {
                ActiveAimSystem(false);
                if (LargeCueBall.enabled)
                    LargeCueBall.enabled = false;
                Handler_OnHitBall(-1, Vector3.zero, Vector3.zero);
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

                    //// Debug.Log("big");
                    last_value_cue_energy = x;
                    navigaion = -1;
                }
                else
                {
                    //Debug.Log("small");
                    last_value_cue_energy = x;
                    navigaion = 1;
                }


                CueRendererMove(navigaion, unit, 0.5f);
                //InvokeRepeating("CheckballMove", 0, gamemanager.SendRate.value); 

            }
        }
        private void HitBallController_OnEnergyTouchEnd(float step)
        {
            if (Server.Turn /*&& Server.Record == false*/)
            {
                TouchWorkInUI = false;
                if (step > 0)
                {
                    float t_step = step / 5.0f;
                    if(t_step == 1)
                    {
                        var rand = UnityEngine.Random.Range(0.0f, PowerCUE * 0.05f);
                        ForceToBall((PowerCUE + rand) * t_step, PowerSpin);
                    }
                    else
                    {
                        ForceToBall(PowerCUE * t_step, PowerSpin);
                    }
                    
                    

                    Server.Turn = false;
                    LimitedMovePitok = false;
                    Handler_OnHitBall(-1, Vector3.zero, Vector3.zero);
                    Handler_ResetCueSpin();
                    PosCueSpin = Vector2.zero;
                    CancelInvoke("HandAnimationOnPitok");
                  //  Debug.Log(step);
                }


                // OffsetPositionCueWoodFromCueBall = new Vector3(-0.3f, 0.0f, 0.00f);
            }
        }
        private void HitBallController_OnChangeSetting(float arg1, float arg2, float arg3, float arg, float arg4, float arg6, float arg7)
        {
            SetSetting(arg1, arg2, arg3, arg, arg4, arg6, arg7);
            Debug.Log("ChengeAccept");
        }

        private void TouchControll()
        {
            if (Input.touchCount > 0 && Server.Turn )
            {

                Touch touch = Input.GetTouch(0);
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit_touch;

                if (Physics.Raycast(ray, out hit_touch, 1000, mask_for_move_Aim))
                {
                    var hitpos = new Vector3(hit_touch.point.x, 00, hit_touch.point.z);
                    var cueball_pos  = new Vector3(transform.position.x, 00, transform.position.z);
                    var dist = Vector3.Distance(hitpos, cueball_pos);
                    if(dist<MinDistancePointTouchToCueBall)
                    {
                        CueBallMoveInPitokTouchController();
                       // Debug.Log("Touckh pitok");
                    }
                    else
                    {
                        CueRotate();
                       // Debug.Log("Touckh aim");
                    }
                  //  Debug.Log("Touckh click");
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
                        rigidbody.isKinematic = true;
                        ActiveAimSystem(false);
                        CancelAnimationHand();
                        Handler_FreazeBall(true);


                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        MoveCueBallInPitok(touch);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {

                        LargeCueBall.enabled = false;
                        ActiveAimSystem(true);

                        IntergatioShowAnimation = 1;



                        //GetComponent<SphereCollider>().isTrigger = false;
                        rigidbody.isKinematic = false;
                        Handler_FreazeBall(false);
                        DragIsBusy = false;
                        CancelAnimationHand();
                        ///    Debug.Log("PITOOKKKK UP");
                        Server.Emit_PositionCueBallInPitoks(this.transform.position);

                    }
                }
            }
        }

        private void MoveCueBallInPitok(Touch touch)
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

                        var limited0_x = Mathf.Clamp(hit.point.x, -5.0f, +5.90f);
                        var limited0_z = Mathf.Clamp(hit.point.z, -2.681f, 2.676f);
                        Vector3 pos_point = new Vector3(limited0_x, 0.0f, limited0_z);

                        // Debug.Log("Touch" +hit.collider.name+":::"+ hit.point);
                        var col = Physics.OverlapSphere(pos_point, RadiusGhostBall, mask_for_move_cue_ball).ToList();

                        col.ForEach((e) =>

                        {// Debug.Log(e.tag);
                            if (e.tag == "ball")
                            {
                                find = true;

                            }
                        });
                        if (find == false)

                        {
                            var limited_x = Mathf.Clamp(hit.point.x, -5.0f, +5.90f);
                            var limited_z = Mathf.Clamp(hit.point.z, -2.681f, 2.676f);
                            transform.DOMove(new Vector3(limited_x, transform.position.y, limited_z), 00.01f, false);
                            LargeCueBall.transform.DOMove(new Vector3(limited_x, 0.64f, limited_z), 00.01f, false);
                            // Debug.Log("Touch" + hit.collider.name + ":::" + hit.point);
                            Server.Emit_PositionCueBallInPitoks(this.transform.position);

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
                        var limited0_x = Mathf.Clamp(hit.point.x, -5.0f, -2.47f);
                        var limited0_z = Mathf.Clamp(hit.point.z, -2.681f, 2.676f);
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
                            var limited_x = Mathf.Clamp(hit.point.x, -5.0f, -2.47f);
                            var limited_z = Mathf.Clamp(hit.point.z, -2.681f, 2.676f);
                            LargeCueBall.transform.DOMove(new Vector3(limited_x, 0.64f, limited_z), 00.01f, false);
                            transform.DOMove(new Vector3(limited_x, transform.position.y, limited_z), 00.01f, false);

                            Server.Emit_PositionCueBallInPitoks(this.transform.position);
                            /// Debug.Log("PITOOKKKK LLLLIIIIIII");
                        }

                    }
                }

            }
        }
        private void CueRotate()
        {
            float curAngle = prevAngle;
            if (Input.touchCount > 0 && Server.Turn && !DragIsBusy)
            {

                Touch touch = Input.GetTouch(0);
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit_touch;

                if (Physics.Raycast(ray, out hit_touch, 1000, mask_for_move_Aim))
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
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {

                            if (Server.Pitok > 0)
                            {
                                StartAnimationHand();
                                Server.Emit_AimCueBall(new Diaco.EightBall.Structs.AimData { X_position = CUEWood.transform.position.x, Z_position = CUEWood.transform.position.z, YY_rotation = CUEWood.transform.eulerAngles.y, PosCueBall = this.transform.position });

                            }
                            var temp_time_touch = Mathf.Abs(timetouch - Time.realtimeSinceStartup);

                            if (temp_time_touch < 0.1f)
                            {
                                LookAtFinger(touch.position);
                                Server.Emit_AimCueBall(new Diaco.EightBall.Structs.AimData { X_position = CUEWood.transform.position.x, Z_position = CUEWood.transform.position.z, YY_rotation = CUEWood.transform.eulerAngles.y, PosCueBall = this.transform.position });

                            }

                        }
                    }

                }
            }
            prevAngle = curAngle;
        }
        public void initialze()
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
            SetYPositionRefrence();

        }
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
      

        public Vector3 AtPosition;
        private void ForceToBall(float powcue, float powspin)
        {
            rigidbody.maxAngularVelocity = maxanguler;
            var dir = (transform.position - CueRenderer.position).normalized;
             AtPosition = new Vector3(
                 (transform.position.x  +  (PosCueSpin.x * RadiusGhostBall)*  dir.z), 
                 (transform.position.y  +  (PosCueSpin.y * RadiusGhostBall)), 
                 (transform.position.z) +  (PosCueSpin.x * RadiusGhostBall) * -dir.x);
            
           // Debug.Log(dir);
            dir.y = 0;

            rigidbody.AddForceAtPosition((GhostBall.transform.position - transform.position).normalized *powcue, AtPosition, Forcemode);

            if (Server.InRecordMode == false)
            {
                StartCoroutine(Server.StartRecordPositionsBallsAndSendToServer());
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

                var dir = transform.position - CueRenderer.position;
                dir.y = 0;
                dir = dir.normalized;

                ray_line.origin = transform.position;
                ray_line.direction = dir;

                lineRenderer.SetPosition(0, transform.position);


                if (Physics.SphereCast(ray_line, RadiusGhostBall, out hit2, 1000, mask_for_Line_Aim))
                {

                 ///   Vector3 g_end = new Vector3();
                 //   Vector3 b_end = new Vector3();
                    if (hit2.collider && hit2.collider.GetComponent<Ball>())
                    {  // Debug.Log(hit2.collider.name);
                        if (Server.CheckBallForAllowHit(hit2.collider.GetComponent<AddressBall>().IDPost))
                        {
                            ////GhostBallChangeAndSetPos///
                            if (Server.Turn)
                                GhostBall.GetComponent<GhostBallControll>().ChangeImage(true);
                            else
                                GhostBall.GetComponent<GhostBallControll>().ChangeImage(true);
                            // GhostBall.transform.position = hit2.point + RadiusGhostBall * hit2.normal;/*hit2.point + Vector3.ClampMagnitude(hit2.point - transform.position, -0.15f);*/
                            GhostBall.transform.position = ray_line.origin + (ray_line.direction.normalized * hit2.distance);


                            lineRenderer.SetPosition(1, GhostBall.transform.position);

                            ///Draw  Line WhiteBall Dir
                            var dir_ghostball_to_whiteball = hit2.point - transform.position;
                            // var scale = (transform.position - hit2.point).magnitude / 180.0f;///
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
                                var line = GhostBall.GetComponent<LineRenderer>();
                                line.enabled = true;
                                line.SetPosition(0, GhostBall.transform.position);

                                line.SetPosition(1, (GhostBall.transform.position + dir_left * a));
                                Debug.DrawRay(GhostBall.transform.position, dir_left * a, Color.green);////absolut
                             //   g_end = GhostBall.transform.position + dir_left * a;
                            }
                            else if (angle_right < angle_left)
                            {
                                float a = (angle_left - angle_right) * ScaleLineAimGhostBall;
                                var line = GhostBall.GetComponent<LineRenderer>();
                                line.enabled = true;
                                line.SetPosition(0, GhostBall.transform.position);
                                line.SetPosition(1, (GhostBall.transform.position + dir_right * a));
                                Debug.DrawRay(GhostBall.transform.position, dir_right * a, Color.cyan);////absolut
                            ///    g_end = GhostBall.transform.position + dir_left * a;
                            }
                         
                            ////Dir  Other ball
                            ///
                            Vector3 dir_ghostballTo_targetball;

                            dir_ghostballTo_targetball = hit2.transform.position - hit2.point;


                            Vector3 pos3 = hit2.transform.position + (AimOffset + cueAim * 0.25f) * dir_ghostballTo_targetball;
                            // hit2.collider.GetComponent<Ball>().SetlineDirection(dir_ghostballTo_targetball + hit2.transform.position);

                            vvv = (hit2.transform.position + (AimOffset + 30 * 0.25f) * dir_ghostballTo_targetball) - hit2.transform.position;
                            Handler_OnHitBall(hit2.collider.GetComponent<AddressBall>().IDPost, pos3,vvv);
                            Debug.DrawLine(GhostBall.transform.position, pos3, Color.blue);
                          //  b_end =  hit2.transform.position + (AimOffset + cueAim * 0.25f) * dir_ghostballTo_targetball;

                            // gamemanager.FirstBallImpact = hit2.collider.GetComponent<AddressBall>().IDPost;
                        }
                        else
                        {
                            if (Server.Turn)
                            {
                                GhostBall.GetComponent<GhostBallControll>().ChangeImage(false);

                                GhostBall.transform.position = ray_line.origin + (ray_line.direction.normalized * hit2.distance);
                                lineRenderer.SetPosition(0, transform.position);
                                lineRenderer.SetPosition(1, GhostBall.transform.position);

                               var dir_ghostballTo_targetball = hit2.transform.position - hit2.point;
                                vvv = (hit2.transform.position + (AimOffset + 30 * 0.25f) * dir_ghostballTo_targetball) - hit2.transform.position;
                                Handler_OnHitBall(-1, Vector3.zero, Vector3.zero);

                            }
                            else
                            {
                                GhostBall.GetComponent<GhostBallControll>().ChangeImage(true);


                                GhostBall.transform.position = ray_line.origin + (ray_line.direction.normalized * hit2.distance);
                                lineRenderer.SetPosition(0, transform.position);
                                lineRenderer.SetPosition(1, GhostBall.transform.position);
                                ///Draw  Line WhiteBall Dir
                                var dir_ghostball_to_whiteball = hit2.point - transform.position;
                                ///  var scale = (transform.position - hit2.point).magnitude / 180.0f;///
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
                                    var line = GhostBall.GetComponent<LineRenderer>();
                                    line.enabled = true;
                                    line.SetPosition(0, GhostBall.transform.position);
                                    line.SetPosition(1, GhostBall.transform.position + dir_left * a);
                                    Debug.DrawRay(GhostBall.transform.position, dir_left * a, Color.green);////absolut

                                }
                                else if (angle_right < angle_left)
                                {
                                    float a = (angle_left - angle_right) * ScaleLineAimGhostBall;
                                    var line = GhostBall.GetComponent<LineRenderer>();
                                    line.enabled = true;
                                    line.SetPosition(0, GhostBall.transform.position);
                                    line.SetPosition(1, GhostBall.transform.position + dir_right * a);
                                    Debug.DrawRay(GhostBall.transform.position, dir_right * a, Color.cyan);////absolut
                                }

                                ////Dir  Other ball

                                Vector3 dir_ghostballTo_targetball;


                                dir_ghostballTo_targetball = hit2.transform.position - hit2.point;

                                Vector3 pos3 = hit2.transform.position + (AimOffset + cueAim * 0.25f) * dir_ghostballTo_targetball;
                                Debug.DrawLine(GhostBall.transform.position, pos3, Color.blue);
                                // hit2.collider.GetComponent<Ball>().SetlineDirection(dir_ghostballTo_targetball + hit2.transform.position);
                               
                                vvv = (hit2.transform.position + (AimOffset + 30 * 0.25f) * dir_ghostballTo_targetball) - hit2.transform.position;
                                Handler_OnHitBall(hit2.collider.GetComponent<AddressBall>().IDPost, pos3,vvv);
                                ///  b_end = hit2.transform.position + (AimOffset + cueAim * 0.25f) * dir_ghostballTo_targetball;

                            }
                        }
                    ///  var aaa=  Vector3.Angle(g_end - GhostBall.transform.position, b_end - hit2.transform.position);
                   //     Debug.Log("Angle::;"+aaa);
                        ///LastTouchPosition = Camera.main.WorldToScreenPoint(hit2.point);
                    }
                    else if (hit2.collider && !hit2.collider.GetComponent<Ball>())
                    {
                        ////GhostBallChangeAndSetPos///
                        GhostBall.transform.position = ray_line.origin + (ray_line.direction.normalized * hit2.distance);
                        lineRenderer.SetPosition(0, transform.position);
                        lineRenderer.SetPosition(1, GhostBall.transform.position);
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
                            var line = GhostBall.GetComponent<LineRenderer>();
                            line.enabled = false;
                            line.SetPosition(0, GhostBall.transform.position);
                            line.SetPosition(1, GhostBall.transform.position + dir_left * 0);
                            Debug.DrawRay(GhostBall.transform.position, dir_left * a, Color.green);

                        }
                        else if (angle_right < angle_left)
                        {
                            float a = (angle_left - angle_right) * ScaleLineAimGhostBall;

                            var line = GhostBall.GetComponent<LineRenderer>();
                            line.enabled = false;
                            line.SetPosition(0, GhostBall.transform.position);
                            line.SetPosition(1, GhostBall.transform.position + dir_right * 0);
                            Debug.DrawRay(GhostBall.transform.position, dir_right * a, Color.cyan);
                        }
                        ///  LastTouchPosition = Camera.main.WorldToScreenPoint(hit2.point);
                        Handler_OnHitBall(-1, Vector3.zero, Vector3.zero);
                        vvv = Vector3.zero;
                    }
                }
            }

        } 

        public void ActiveAimSystem(bool show)
        {
           
            
      
            if (show == false)
            {
                AimSystemShow = show;
                CUEWood.SetActive(show);
                GhostBall.SetActive(show) ;
                EnergyCue.Show(show);
                HandIcon.enabled = show;
                CancelAnimationHand();


                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
                last_value_cue_energy = 0;
               //// Debug.Log("DisableAim");
            }
            else if (show == true)
            {

                if (Server.Turn && CheckBallMove() ==  false)
                {
                     
                    AimSystemShow = show;
                    CUEWood.SetActive(show);
                    GhostBall.SetActive(show);
                    EnergyCue.Show(show);

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
                Handler_OnHitBall(-1, Vector3.zero, Vector3.zero);

            }
        }
        public void ActiveAimSystemForShowInOtherClient(bool show)
        {
           
          
            if (show == true)
            {
               // Debug.Log("ActiveAimXXX1");
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
               // Debug.Log("ActiveAimXXX44");
            }
            else if (show == false)
            {

                AimSystemShow = show;
                CUEWood.SetActive(show);
                GhostBall.SetActive(show);
               // HandIcon.enabled = show;


                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
                Handler_OnHitBall(-1, Vector3.zero, Vector3.zero);
                Debug.Log("DisableAimXXx");

            }

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
       
        public void MoveOnPlayRecord(Vector3 pos, float speed)
        {
            transform.DOMove(pos, speed);
        }
      
        public void resetpos()
        {

            CUEWood.transform.position = this.transform.position;
            CueRenderer.localPosition = new Vector3(-7.5f, 1.0f, 0.0f);
            GhostBall.transform.position = this.transform.position;
            HandIcon.transform.position = new Vector3(this.transform.position.x + 0.37f, 0.64f, this.transform.position.z);
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
        private void BounceBall(Collision collision)
        {



            var normal = collision.contacts[0].normal;
            var reflect2 = Vector3.Reflect(VlocityBall.normalized, normal).normalized;
            rigidbody.velocity = (reflect2 * collision.relativeVelocity.magnitude) * powerbounce;
           // Debug.Log("wall");

        }
        private void SetSetting(float pow, float Drag, float AngularDrag,float MaxAngularDrag, float SpeedThershold,float sensivityrotate ,  float powbounce)
        {
            this.maxanguler = MaxAngularDrag;
            this.PowerCUE = pow;
            rigidbody.drag = Drag;
            rigidbody.angularDrag = AngularDrag;
            this.ThresholdSleep = SpeedThershold;
            this.SensitivityRotate = sensivityrotate;
            this.powerbounce = powbounce;
        }
        #region Events

        public event Action<int , Vector3,Vector3> OnHitBall;

        public  void Handler_OnHitBall(int TargetBall, Vector3 Direction , Vector3 TargetVelocity)
        {
            if (OnHitBall != null)
            {
                OnHitBall(TargetBall, Direction,TargetVelocity);
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