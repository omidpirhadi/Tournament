using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diaco.SoccerStar.Server;
using Diaco.SoccerStar.Marble;
public class TempPlayerControll : MonoBehaviour
{
    public ServerManager server;
    public AimCircle aimCricle;

    public LayerMask MaskForSelect;
    public LayerMask MaskForMove;
    public float DistanceFingerFromMarble = 2.0f;
    public ForceToBall MarbleSelected;
    public int ID = 0;
   /// public GameObject CircleAim;
   // public GameObject indicator;
    public float Sensiviti = 0.1f;

    public Vector3 LastPos;

    private bool activeselection;

    private RaycastHit hit, hit2;
    private Vector3 firstouch, secondtouch;
    private bool Touch2Clicked = false;
    private bool rotateType2;



    void FixedUpdate()
    {
        TouchControll();
    }
    public void TouchControll()
    {
        if (Input.touchCount == 1)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
               // Debug.Log("OMMMMMMDDDDD");
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit,100000000, MaskForSelect))
                {
                   // Debug.Log(hit.collider.name+"    OMMMMMMDDDDD");
                    Deselect();
                 ///   FindNearMarbleFromFinger(hit.point);
                    if (hit.collider && hit.collider.tag == "marble")
                    {
                       // Debug.Log("OMMMMMMDDDDD3");
                        var marble = hit.collider.GetComponent<ForceToBall>();
                        if (marble.MarbleType == ForceToBall.Marble_Type.Marble && (CheckOwnerMarble(marble.ID)) /*&& server.Play*/)
                        {
                            SetAimCircleOnMarble(marble.transform);
                            aimCricle.StartRecordAim(ID);
                            firstouch = hit.collider.transform.position;
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
                if (Physics.Raycast(ray, out hit, 100000000, MaskForSelect) && MarbleSelected)
                {


                    if (!Touch2Clicked)
                    {

                        var dis_current = Vector3.Distance(hit.point, firstouch);
                        var dis_last = Vector3.Distance(LastPos, firstouch);
                        Debug.Log($"DistancFirst:{dis_current}DistancLast:{dis_last}");
                        aimCricle.ScaleAim(ID, dis_current - dis_last);

                        LastPos = hit.point;

                        if (rotateType2)
                        {
                            aimCricle.AimCircleRotate(ID, Input.GetTouch(0).deltaPosition.y * Sensiviti);
                            ///    aimCricle.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });

                        }
                        else
                        {
                            aimCricle.AimCircleAndIndicatorRotate(ID, hit.point);
                            //   aimCricle.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });
                        }

                    }
                    else
                    {
                        LastPos = hit.point;
                        var dis_current = Vector3.Distance(hit.point, firstouch);
                        var dis_last = Vector3.Distance(LastPos, firstouch);

                        aimCricle.ScaleAim(ID, dis_current - dis_last);
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
                    var pow = aimCricle.CurrentAimPower / aimCricle.PowerRadius;

                    var dir = aimCricle.DirectionShoot();
                    Handler_OnShoot(ID, dir.normalized, pow);
                    aimCricle.StopRecordAim();
                    aimCricle.ResetAimCircle();


                }
                else if (MarbleSelected != null && aimCricle.CurrentAimPower < 3.5f)
                {
                    Touch2Clicked = false;
                    rotateType2 = false;
                    
                    aimCricle.StopRecordAim();
                    aimCricle.ResetAimCircle();
                    Handler_EnableRingEffect(true);

                }

            }

        }
        else if (Input.touchCount == 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);
                if (Physics.Raycast(ray, out hit2, MaskForMove) && MarbleSelected)
                {

                    Touch2Clicked = true;
                    rotateType2 = true;
                    LastPos = hit2.point;

                }

            }
            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {

                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);
                if (Physics.Raycast(ray, out hit2, MaskForMove) && MarbleSelected)
                {

                    var dis_current = Vector3.Distance(hit2.point, firstouch);
                    var dis_last = Vector3.Distance(LastPos, firstouch);

                    aimCricle.ScaleAim(ID, dis_current - dis_last);

                    LastPos = hit2.point;

                    aimCricle.AimCircleAndIndicatorRotate2(ID, hit2.point);
                    // MarbleSelected.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });
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
            //Debug.Log("Two Finger");
        }
    }
    private  void SetAimCircleOnMarble( Transform marble)
    {
     //   aimCricle.gameObject.transform.parent = marble.transform;
        aimCricle.gameObject.transform.position = marble.transform.position;
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
        secondtouch = Vector3.zero;
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

