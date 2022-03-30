using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.RoyalTournument
{

    public class CompetitionInfo : MonoBehaviour
    {
        [Header("RequirmentAsset")]
        private ServerUI server;
        public ImageContainerTool.ImageContainer CostImage, GameTypeImage,AvatarImage;
        public CompetitionInfoPlayerCard PlayerCard;
        public RectTransform Content;
        private List<GameObject> ListPlayer;
        [Header("PropertyPopups")]
        public string  tournumentId;
        public Text Game_txt;
        public Text GameType_txt;
        public Image CostType;
        public Text Currency;
        public Text Capacity;
        public Text Timer;
        public Button btn_Award;
        public Button btn_Join;
        public Button btn_Leave;
        private float H = 0;
        private float M = 0;
        private float S = 0;


        private void OnEnable()
        {
            ListPlayer = new List<GameObject>();
            server = FindObjectOfType<ServerUI>();
            server.OnCompetitionInfo += Server_OnCompetitionInfo;
            btn_Leave.onClick.AddListener(() => { server.RequestCompetitionCommand(tournumentId, 0); });
            btn_Join.onClick.AddListener(() => {server.RequestCompetitionCommand(tournumentId,1); });
            btn_Award.onClick.AddListener(() => {
                server.navigationUi.ShowPopUp("royalteamaward");
                server.RequestCompetitionCommand(tournumentId,2);

            });
            
        }

        private void Server_OnCompetitionInfo(CompetitionInfoData data)
        {
            InitRoyal(data);
        }


        private void OnDisable()
        {
            server.OnCompetitionInfo -= Server_OnCompetitionInfo;
            btn_Join.onClick.RemoveAllListeners();
            btn_Award.onClick.RemoveAllListeners();
            btn_Leave.onClick.RemoveAllListeners();
            ClearPlayer();
        }
        private void InitRoyal(CompetitionInfoData data)
        {
            ClearPlayer();
            
            tournumentId = data.id;
            if(data.game == 0)
            {
                Game_txt.text = PersianFix.Persian.Fix("فوتبال", 255);
            }
            else
            {
                Game_txt.text = PersianFix.Persian.Fix("بیلیارد", 255);
            }


            if ( data.gameType == 4)
            {
                GameType_txt.text = PersianFix.Persian.Fix("قهرمانی", 255);
            }
            else if (data.gameType == 8)
            {
                GameType_txt.text = PersianFix.Persian.Fix("حماسی", 255);
            }
            else if (data.gameType == 16)
            {
                GameType_txt.text = PersianFix.Persian.Fix("رویال", 255);
            }
 
            if (data.member)
            {
                btn_Leave.gameObject.SetActive(true);
                btn_Join.gameObject.SetActive(false);
            }
            else
            {
                btn_Leave.gameObject.SetActive(false);
                btn_Join.gameObject.SetActive(true);
            }
            CostType.sprite = CostImage.LoadImage(data.costType.ToString());


            Currency.text = data.cost.ToString();
            Capacity.text = data.capacity;
           
            SpawnPlayerCard(data.members);
            CalculateTime(data.time);

        }
        
        private void SpawnPlayerCard(List<PlayerInCompetition> player)
        {
            for (int i = 0; i < player.Count; i++)
            {
                var card = Instantiate(PlayerCard, Content);
                var image = AvatarImage.LoadImage(player[i].avatar);
                var admin = false;
                if (i == 0)
                {
                    admin = true;
                }
                else
                {
                    admin = false;
                }
                card.Set(admin, image, player[i].userName, player[i].cup);
                ListPlayer.Add(card.gameObject);
            }
        }
        private void ClearPlayer()
        {
            for (int i = 0; i < ListPlayer.Count; i++)
            {
                Destroy(ListPlayer[i].gameObject);
            }
            ListPlayer.Clear();
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
            if (S == 0 && M == 0 && H == 0)
            {
                CancelInvoke("RunTimer");


            }
        }
    }

    [Serializable]
    public struct CompetitionInfoData
    {
        public string id;
        public bool member;
        public int game; //00 socor 1 biliard 
        public int gameType;  // 0 ghahremani 1  hamasi 2 royal 
        public int costType;//0 coin 1 gem
        public int cost;
        
        public string capacity;
        public int time;

        public List<PlayerInCompetition> members;
        
        

    }
    [Serializable]
    public struct PlayerInCompetition
    {
        public string avatar;
        public string userName;
        public int cup;
    }
}