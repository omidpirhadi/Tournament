
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.RoyalTournument
{

    public class MatchCard : MonoBehaviour
    {
        public Diaco.ImageContainerTool.ImageContainer CostImage;
        public string ID;
        public Image CostType;
        public Text  Currency;
        public Text  Capacity;
        public Text  Time;
        private Button btn_card;
        private ServerUI server;

        private float H = 0;
        private float M = 0;
        private float S = 0;

        public void Set(string id ,int costType,int currency, string capacity , int time )
        {
            btn_card = GetComponent<Button>();
            server = FindObjectOfType<ServerUI>();
           // navigationUI = FindObjectOfType<NavigationUI>();
            ID = id;
            if(costType == 0)
            {
                CostType.sprite = CostImage.LoadImage("0");
            }
            else
            {
                CostType.sprite = CostImage.LoadImage("1");
            }

            Currency.text = currency.ToString();
            Capacity.text = capacity;
            
            btn_card.onClick.AddListener(Onclick);
            CalculateTime(time);
        }
        private void Onclick()
        {
            server.RequestCompetitionInfo(ID);
            Debug.Log("MOSABEGHE:" + ID);
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


            Time.text = H + ":" + M + ":" + S;
            if (S == 0 && M == 0 && H == 0)
            {
                CancelInvoke("RunTimer");


            }
        }
    }
}