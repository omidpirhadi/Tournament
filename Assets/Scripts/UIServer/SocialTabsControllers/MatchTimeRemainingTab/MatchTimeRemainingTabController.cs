using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Social
{
    public class MatchTimeRemainingTabController : MonoBehaviour
    {
        public ServerUI Server;
        public Text TimerIndicator;
        public Button StartGame;
        public float H = 0;
        public float M = 0;
        public float S = 0;
        private void Awake()
        {
            Server.OnGetTimeTeam += Server_OnGetTimeTeam;
        }
        private void OnEnable()
        {
            //Server.OnGetTimeTeam += Server_OnGetTimeTeam;
        }
        private void OnDisable()
        {
           // Server.OnGetTimeTeam -= Server_OnGetTimeTeam;
            H = 0;
            M = 0;
            S = 0;
            CancelInvoke("RunTimer");
        }
        private void Server_OnGetTimeTeam(float time)
        {
            CalculateTime(time);

        }
        public void SendRequestTimeMatch()
        {
            Server.SendRequestMatchTimeRemaining();
        }
        public void CalculateTime(float time)
        {
            H = 0;
            M = 0;
            S = 0;
            CancelInvoke("RunTimer");
       

            H = (float)Math.Floor(time / 3600);
            M = (float)Math.Floor(time / 60 % 60);
            S = (float)Math.Floor(time % 60);
            InvokeRepeating("RunTimer", 0, 1.0f); 
        }
        /// <summary>
        /// INVOKE IN Calculate
        /// </summary>
        public void RunTimer()
        {
            S--;
            if (S < 0)
            {
                if (M > 0 || H>0)
                {
                    S = 59;
                    M--;
                    if (M < 0)
                    {
                        if (H > 0)
                        {
                            M = 59;
                            H--;
                        }
                        else
                        {
                            M = 0;
                        }
                    }

                }
                else
                {
                    S = 0;
                }
            }


            TimerIndicator.text = H + ":" + M + ":" + S;
            if(S == 0 && M == 0&& H == 0)
            {
                CancelInvoke("RunTimer");
                StartGame.interactable = true;

            }
        }
    }
}