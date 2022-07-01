using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.TournumentCard
{
    [RequireComponent(typeof(Button))]
    public class TournumentCard : MonoBehaviour
    {
        public string Id;
        public string Type;
        public Text Name;
        public Text Timer;

        private Button btn_tournament;
        private ServerUI server;
        private float H = 0;
        private float M = 0;
        private float S = 0;


        public void Set(string id , string name , int time, string type)
        {
            btn_tournament = GetComponent<Button>();
            server = FindObjectOfType<ServerUI>();
            Id = id;
            Type = type;
            Name.text = name;
            CalculateTime(time);
           
        }
        public void OnEnable()
        {
            // Set("id:test0", "Classic", 100, "ff"); for test
            btn_tournament = GetComponent<Button>();
            btn_tournament.onClick.AddListener(() => {
                if (H < 0.1f && M < 0.1 && S < 0.1f)
                {

                    server.RequestGoToTableRanking();

                }
                else
                {
                    if (Type == "competition")
                    {
                        server.RequestCompetitionInfo(Id);
                    }
                    else if (Type == "league")
                    {
                        server.GetLeagueInfo(Id);
                    }
                }

            });
        }
        public void OnDisable()
        {
            btn_tournament.onClick.RemoveAllListeners();
        }
        public void OnDestroy()
        {
            btn_tournament.onClick.RemoveAllListeners();
        }
        private void CalculateTime(int time)
        {
            H = 0;
            M = 0;
            S = 0;
            CancelInvoke("RunTimer");


            H = (float)Mathf.Floor(time / 3600);
            M = (float)Mathf.Floor(time / 60 % 60);
            S = (float)Mathf.Floor(time % 60);
            InvokeRepeating("RunTimer", 0, 1.0f);
        }
        /// <summary>
        /// INVOKE IN Calculate
        /// </summary>
        private void RunTimer()
        {
            S--;
            if (S < 0)
            {
                if (M > 0 || H > 0)
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


            Timer.text = H + ":" + M + ":" + S;
            if (H == 0 && M == 00)
            {

                if (GetComponent<SoundEffectControll>())
                    GetComponent<SoundEffectControll>().PlaySound(0);
            }
            if (S == 0 && M == 0 && H == 0)
            {
                CancelInvoke("RunTimer");


            }
        }
    }
   
}