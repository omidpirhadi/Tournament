using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diaco.SoccerStar.Server;
using Diaco.SoccerStar.Marble;
namespace Diaco.SoccerStar.PlayerControllers
{
   /* public class PlayerController : MonoBehaviour
    {
        public ServerManager server;
        public AimCircle aimCricle;

        public LayerMask MaskForSelect;
        public LayerMask MaskForMove;

        public ForceToBall MarbleSelected;
        public int ID = 0;
        public GameObject CircleAim;
        public GameObject indicator;
        public float Sensiviti = 0.1f;

        public Vector3 LastPos;
       
        private bool activeselection;

        private RaycastHit hit , hit2;
        private Vector3 firstouch, secondtouch;
        private bool Touch2Clicked = false;
        private bool rotateType2;

        public bool ActiveSelection
        {
            set
            {
                activeselection = value;
                if(activeselection)
                {
                    Handler_OnActiveSelectEffect(true);
                }
                else
                {
                    Handler_OnActiveSelectEffect(false);
                }
            }

            get
            {
                return activeselection;
            }
        }

       
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

                    var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out hit,MaskForSelect))
                    {
                        Deselect();
                       
                        if (hit.collider && hit.collider.tag == "marble")
                        {
                            var marble = hit.collider.GetComponent<ForceToBall>();
                            if (marble.MarbleType == ForceToBall.Marble_Type.Marble && (CheckOwnerMarble(marble.ID)) && server.Play)
                            {
                                firstouch = hit.collider.transform.position;
                                this.MarbleSelected = marble;
                                ID = marble.ID;
                                CircleAim = marble.CircleAim;
                                indicator = marble.indicator;
                                marble.Sensiviti = this.Sensiviti;
                                if (!Touch2Clicked)
                                    LastPos = hit.collider.transform.position;
                                else
                                    LastPos = hit.point;
                                //aimCricle.gameObject.transform.parent = marble.transform;
                            // ActiveSelection = false;
                            }
                        }
                    }

                }

                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {



                    var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out hit, MaskForMove) && MarbleSelected)
                    {
  

                        if (!Touch2Clicked)
                        {
                           // MarbleSelected.ScaleCricleAim(firstouch, hit.point);
                          //  Debug.Log("Current POS" +  hit.point + "\r\n\t"+"LastChange" + LastPos);
                            var dis_current = Vector3.Distance(hit.point, firstouch);
                            var dis_last = Vector3.Distance(LastPos, firstouch);
             
                            MarbleSelected.ScaleAim(dis_current - dis_last);
                         
                            LastPos = hit.point;

                            if (rotateType2)
                            {
                                MarbleSelected.AimCircleRotate(Input.GetTouch(0).deltaPosition.y * Sensiviti);
                              //  MarbleSelected.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });

                            }
                            else
                            {
                                MarbleSelected.AimCircleAndIndicatorRotate(hit.point);
                                // MarbleSelected.AimCircleRotate(Input.GetTouch(0).position.y * Sensiviti);
                                // MarbleSelected.AimCircleRotate(Input.GetTouch(0).deltaPosition.y * Sensiviti);
                              //  MarbleSelected.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });
                            }

                        }
                        else
                        {
                            LastPos = hit.point;
                            var dis_current = Vector3.Distance(hit.point, firstouch);
                            var dis_last = Vector3.Distance(LastPos, firstouch);

                            MarbleSelected.ScaleAim(dis_current - dis_last);
                            Touch2Clicked = false;
                            
                            // MarbleSelected.ScaleCricleAim(firstouch, hit.point);
                            //MarbleSelected.RotateIndicator(Input.GetTouch(0));
                            //MarbleSelected.AimCircleRotate(Input.GetTouch(0).deltaPosition.y * Sensiviti);
                           // MarbleSelected.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });

                        }
                    }
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {

                    if (MarbleSelected != null && MarbleSelected.CurrentAimPower > 9.5f)
                    {
                      //  MarbleSelected.Move(indicator.transform.position);
                        MarbleSelected.ResetAimAndArrow();
                        Touch2Clicked = false;
                        rotateType2 = false;
                      // ActiveSelection = false;
                    }
                    else if (MarbleSelected != null && MarbleSelected.CurrentAimPower < 10f)
                    {
                        Touch2Clicked = false;
                        rotateType2 = false;
                        MarbleSelected.ResetAimAndArrow();
                       // ActiveSelection = true;

                    }
                    
                }
              //  Debug.Log("One Finger");
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
                      //  Debug.Log("One Finger2 Relase");
                    }
                    
                }
                if (Input.GetTouch(1).phase == TouchPhase.Moved)
                {

                    var ray = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);
                    if (Physics.Raycast(ray, out hit2,MaskForMove) && MarbleSelected)
                    {

                        var dis_current = Vector3.Distance(hit2.point, firstouch);
                        var dis_last = Vector3.Distance(LastPos, firstouch);

                        MarbleSelected.ScaleAim(dis_current - dis_last);

                        LastPos = hit2.point;

                        // firstouch = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        //  MarbleSelected.ScaleCricleAim(transform.position, hit2.point);
                        ///MarbleSelected.RotateIndicator(Input.GetTouch(1));
                        //  MarbleSelected.AimCircleRotate(Input.GetTouch(1).deltaPosition.y * Sensiviti);
                        MarbleSelected.AimCircleAndIndicatorRotate2(hit2.point);
                      //  MarbleSelected.SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData { ID = ID, CircleRotate_Y = CircleAim.transform.eulerAngles.y, CricleScale = CircleAim.transform.localScale.x, PositionIndicator = indicator.transform.localPosition, RotateIndicator_Y = indicator.transform.eulerAngles.y });
                    }

                }
                //Debug.Log("Two Finger");
            }
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

        public void Deselect()
        {
            MarbleSelected = null;
            CircleAim = null;
            indicator = null;
            ID = -1000;

            firstouch = Vector3.zero;
            secondtouch = Vector3.zero;
            Touch2Clicked = false;

        }
        private Action<bool, GameObject> onactiveselecteffect;

        public event Action<bool , GameObject> OnActiveSelectEffect
        {
            add
            {
                onactiveselecteffect += value;
            }
            remove
            {
                onactiveselecteffect -= value;
            }
        }
        protected void Handler_OnActiveSelectEffect(bool active )
        {
            if(onactiveselecteffect !=null)
            {
                onactiveselecteffect(active, MarbleSelected.gameObject);
            }
        }
    }*/

}