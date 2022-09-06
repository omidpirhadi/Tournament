﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Diaco.SoccerStar.Server;
using Diaco.SoccerStar.Marble;
public class TempPlayerControll : MonoBehaviour
{
    public ServerManager server;
    public AimCircle aimCricle;

    public LayerMask MaskForSelect;
    public LayerMask MaskForMove;
    //public float DistanceFingerFromMarble = 2.0f;
    public ForceToBall MarbleSelected;
    public int ID = 0;
   /// public GameObject CircleAim;
   // public GameObject indicator;
    public float Sensiviti = 0.1f;
    public bool AimForward = false;
    public Vector3 LastPos;
    public Vector3 FirstPosFinger2;
   // private bool activeselection;

    private RaycastHit hit, hit2;
    [SerializeField] private Vector3 firstouch/*, secondtouch*/;
    [SerializeField] private bool Touch2Clicked = false;
    [SerializeField] private bool rotateType2;
    //[SerializeField] private NavigationUI ui;
    // [SerializeField] private bool UIActive = false;

    private Diaco.Setting.GeneralSetting generalSetting;
    void Start()
    {
        // ui = FindObjectOfType<NavigationUI>();
        //  ui.OnUIActive += Ui_OnUIActive;
        generalSetting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
        generalSetting.OnChangeSetting += TempPlayerControll_OnChangeSetting;
        AimForward = generalSetting.Setting.soccersettingdata.aimForward;
    }


    /* private void Ui_OnUIActive(bool active)
     {
         UIActive = active;
     }*/

    void Update()
    {
        TouchControll();
        //MarbleRingEffect();
    }
    void OnDestroy()
    {
        if (generalSetting)
            generalSetting.OnChangeSetting -= TempPlayerControll_OnChangeSetting;
    }
    private void TempPlayerControll_OnChangeSetting()
    {
        AimForward = generalSetting.Setting.soccersettingdata.aimForward;
    }

   
    public void TouchControll()
    {
        if (Input.touchCount == 1)
        {
            rotateType2 = false;
           // Touch2Clicked = false;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {

              
                Deselect();
               // Debug.Log("OMMMMMMDDDDD");
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit,100000000, MaskForSelect))
                {
                   //Debug.Log(hit.collider.name+"    OMMMMMMDDDDD");
                    
                 ///   FindNearMarbleFromFinger(hit.point);
                    if (hit.collider && hit.collider.tag == "marble")
                    {
                 ///       Debug.Log("OMMMMMMDDDDD3");
                        var marble = hit.collider.GetComponent<ForceToBall>();
                        if (marble.MarbleType == ForceToBall.Marble_Type.Marble && (CheckOwnerMarble(marble.ID)) /*&& server.Play*/)
                        {

                      ///      Debug.Log("OMMMMMMDDDDD4");
                            SetAimCircleOnMarble(marble.transform);
                            aimCricle.StartRecordAim(ID);
                            firstouch = new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
                            this.MarbleSelected = marble;
                            ID = marble.ID;

                            //marble.Sensiviti = this.Sensiviti;
                            if (!Touch2Clicked)
                                LastPos = hit.collider.transform.position;
                            else
                                LastPos = hit.point;

                           
                            /// Handler_EnableRingEffect(false);
                            
                            
                        }
                    }
                }

            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {



                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit, 100000000, MaskForMove) && MarbleSelected)
                {


                    if (!Touch2Clicked)
                    {

                       // var dis_current = Vector3.Distance(hit.point, firstouch);
                        var pos_marble = new Vector3(firstouch.x, 0, firstouch.z);
                        var pos_lasttouch = new Vector3(LastPos.x, 0, LastPos.z);

                        var dis_last = Vector3.Distance(pos_marble, pos_lasttouch);

                        LastPos = new Vector3(hit.point.x, 0, hit.point.z);

                        if (rotateType2)
                        {
                            var dis_current = Vector3.Distance(new Vector3(hit.point.x, 0, hit.point.z), pos_marble);
                            aimCricle.ScaleAim(ID, aimCricle.transform.localScale.x + dis_current - dis_last);
                            aimCricle.AimCircleRotate(ID, Input.GetTouch(0).deltaPosition.y * Sensiviti);
                            ///    aimCricle.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });

                        }
                        else
                        {
                            aimCricle.ScaleAim(ID,  dis_last);
                            if (AimForward == false)
                                aimCricle.AimCircleAndIndicatorRotate(ID, hit.point);
                            else if (AimForward == true)
                                aimCricle.AimCircleAndIndicatorRotate2(ID, hit.point);
                            //   aimCricle.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });
                        }

                    }
                    else
                    {
                        LastPos = new Vector3(hit.point.x, 0, hit.point.z);
                        // var dis_current = Vector3.Distance(hit.point, firstouch);
                        // var dis_last = Vector3.Distance(LastPos, firstouch);

                        //aimCricle.ScaleAim(ID, dis_current - dis_last);
                        Touch2Clicked = false;

                    }
                }

                if (aimCricle.CurrentAimPower < 3.5f)
                {
                    Handler_EnableRingEffect(true);
                    aimCricle.HideAimCricle(true);
                }
                else
                {
                    Handler_EnableRingEffect(false);
                    aimCricle.HideAimCricle(false);
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {

                if (MarbleSelected != null && aimCricle.CurrentAimPower > 3.5f)
                {

                    Touch2Clicked = false;
                    rotateType2 = false;
                    var pow = (aimCricle.CurrentAimPower - 3.5f) / (aimCricle.PowerRadius - 3.5f);

                    var dir = aimCricle.DirectionShoot();
                    Handler_OnShoot(ID, dir.normalized, pow);
                    aimCricle.StopRecordAim();
                    aimCricle.ResetAimCircle();
                    MarbleSelected = null;


                }
                else if (MarbleSelected != null && aimCricle.CurrentAimPower < 3.5f)
                {
                    Touch2Clicked = false;
                    rotateType2 = false;
                    
                    aimCricle.StopRecordAim();
                    aimCricle.ResetAimCircle();
                    Handler_EnableRingEffect(true);

                }
                return;
            }

        }
        else if (Input.touchCount == 2 && AimForward == false)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);
                if (Physics.Raycast(ray, out hit2, MaskForMove) && MarbleSelected)
                {

                    Touch2Clicked = true;
                    rotateType2 = true;
                    FirstPosFinger2 = new Vector3(hit2.point.x, 0, hit.point.z);

                    LastPos = hit2.point;
                    
                }

            }
            else if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {

                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);
                if (Physics.Raycast(ray, out hit2, MaskForMove) && MarbleSelected)
                {
                 
                    var dis_current = Vector3.Distance(hit2.point, firstouch);
                    var dis_last = Vector3.Distance(LastPos, firstouch);
                    var first_dis_finger2FromMarble = Vector3.Distance(FirstPosFinger2, firstouch);
                    if(dis_current - dis_last < 0)
                    {
                        if (dis_current < (first_dis_finger2FromMarble - 2))
                            aimCricle.ScaleAim(ID, aimCricle.transform.localScale.x + dis_current - dis_last);
                    }
                    else
                    {
                        aimCricle.ScaleAim(ID, aimCricle.transform.localScale.x + dis_current - dis_last);

                    }


                    LastPos = hit2.point;
                    aimCricle.AimCircleRotate(ID, Input.GetTouch(1).deltaPosition.y * Sensiviti);
                   // aimCricle.AimCircleRotate(ID, Input.GetTouch(1).deltaPosition.x * Sensiviti);
                    // aimCricle.AimCircleAndIndicatorRotate2(ID, hit2.point);
                }
                if (aimCricle.CurrentAimPower < 3.5f)
                {
                    Handler_EnableRingEffect(true);
                }
                else
                {
                    Handler_EnableRingEffect(false);
                }
            }
            else if (Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                if (Input.touchCount == 0)
                {
                    if (MarbleSelected != null && aimCricle.CurrentAimPower > 3.5f)
                    {

                        Touch2Clicked = false;
                        rotateType2 = false;
                        var pow = (aimCricle.CurrentAimPower - 3.5f) / (aimCricle.PowerRadius - 3.5f);

                        var dir = aimCricle.DirectionShoot();
                        Handler_OnShoot(ID, dir.normalized, pow);
                        aimCricle.StopRecordAim();
                        aimCricle.ResetAimCircle();
                        MarbleSelected = null;


                    }
                    else if (MarbleSelected != null && aimCricle.CurrentAimPower < 3.5f)
                    {
                        Touch2Clicked = false;
                        rotateType2 = false;

                        aimCricle.StopRecordAim();
                        aimCricle.ResetAimCircle();
                        Handler_EnableRingEffect(true);

                    }
                    Debug.Log("Two Finger");


                    return;
                }
            }
            /// Debug.Log("Two Finger");
        }

        if(Input.touchCount ==  0 && MarbleSelected != null  )
        {
            Touch2Clicked = false;
            rotateType2 = false;
           // var pow = (aimCricle.CurrentAimPower - 3.5f) / (aimCricle.PowerRadius - 3.5f);

          //  var dir = aimCricle.DirectionShoot();
            //Handler_OnShoot(ID, dir.normalized, pow);
            aimCricle.StopRecordAim();
            aimCricle.ResetAimCircle();
            MarbleSelected = null;
            Debug.Log("XxXXXXX");
            return;
        }
    }
    private  void SetAimCircleOnMarble( Transform marble)
    {
     //   aimCricle.gameObject.transform.parent = marble.transform;
        aimCricle.gameObject.transform.position = marble.transform.position;
        aimCricle.MaskCircle.position = marble.transform.position;
    }
    public bool CheckOwnerMarble(int ID)
    {
        bool c = false;
        if (ID >= server.MinRangMarblesId && ID <= server.MaxRangMarblesId)
        {
            c = true;
        }
        return c;
    }
    /*public ForceToBall FindNearMarbleFromFinger(Vector3 touchPosition)
    {
        ForceToBall marble = new ForceToBall();
        for(int i  = 0;i<server.Marbles.Count;i++)
        {
            var pos_marble = server.Marbles[i].transform.position;

            var dis = Vector3.Distance(pos_marble, touchPosition);
            //Debug.Log(dis);
            if (dis<=DistanceFingerFromMarble)
            {
                Debug.Log(server.Marbles[i].name);
                marble = server.Marbles[i].gameObject.GetComponent<ForceToBall>();
            }
        }
        return marble; 
    }*/
    public void Deselect()
    {
        MarbleSelected = null;
        //CircleAim = null;
       // indicator = null;
        ID = -1000;

        firstouch = Vector3.zero;
       // secondtouch = Vector3.zero;
        Touch2Clicked = false;

    }

    private Action<int,Vector3,float > shoot;
    public event Action<int,Vector3, float> OnShoot
    {
        add
        {
            shoot += value; 
        }
        remove
        {
            shoot -= value;
        }
    }
    protected void Handler_OnShoot(int marbleID,Vector3 directionShoot ,float powerShoot)
    {
        if(shoot !=null)
        {
            shoot(marbleID, directionShoot, powerShoot);
        }
    }

    private Action<bool> enableselectringeffect;
    public event Action<bool> EnableSelectRingEffect
    {
        add
        {
            enableselectringeffect += value;
        }
        remove
        {
            enableselectringeffect -= value;
        }
    }
    protected void Handler_EnableRingEffect(bool enable)
    {
        if(enableselectringeffect !=null)
        {
            enableselectringeffect(enable);
        }
    }

  
}

