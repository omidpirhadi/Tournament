using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.TicketManagers
{
    public class Ticket : MonoBehaviour
    {
        public string ID;
        public Diaco.ImageContainerTool.ImageContainer Cost_images;
        public Diaco.ImageContainerTool.ImageContainer Background_images;

        public RTLTMPro.RTLTextMeshPro GameTicketName;
        public Image Background_image;

        public Image Cost_image;
        public Text Cost_text;

        public Text Capacity_text;
        public Text Timer_text;


        private float H = 0;
        private float M = 0;
        private float S = 0;

        private Button ticket_button;
        private void OnEnable()
        {
            ticket_button = GetComponent<Button>();
            ticket_button.onClick.AddListener(() =>
            {

                var server = FindObjectOfType<ServerUI>();
                server.Emit_Ticket(ID);
            });
        }
        private void OnDisable()
        {
            ticket_button.onClick.RemoveAllListeners();
        }
        public void Set(Diaco.HTTPBody.TicketData data)
        {
            
            Cost_text.text = data.cost;
            Capacity_text.text = data.capacity;
            ID = data.id;
            setBackground(data.gameType);
            setCostType(data.costType);
            CalculateTime(data.time);
        }
        private void setBackground(int gameType)
        {
            if (gameType == 0)/// soccer
            {
                Background_image.sprite = Background_images.LoadImage("0");
                GameTicketName.text = "فوتبال";
            }
            else // billiard
            {
                Background_image.sprite = Background_images.LoadImage("1");
                GameTicketName.text = "بیلیارد";
            }
        }
        private  void setCostType(int costType)
        {
            if (costType == 0)// cup
            {
                Cost_image.sprite = Cost_images.LoadImage("0");
            }
            else if (costType == 1) // coin
            {
                Cost_image.sprite = Cost_images.LoadImage("1");
            }
            else if (costType == 2) // gem
            {
                Cost_image.sprite = Cost_images.LoadImage("2");
            }
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


            Timer_text.text = H + ":" + M + ":" + S;

            if (S == 0 && M == 0 && H == 0)
            {
                CancelInvoke("RunTimer");


            }
        }
    }
}