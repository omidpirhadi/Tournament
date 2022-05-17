using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.MatchRecord
{
    public class MatchRecordMode : MonoBehaviour
    {
        public Diaco.ImageContainerTool.ImageContainer ImageProfileContainer;


        public Text ReminingTime;
        [Header("RecordGame")]
        public GameObject PanelRecordMode;
        public Text Point;
        public Text Rank;
        public Text CostCoin_txt;
        public Toggle[] TryToggle;
        public Button btn_Start;
        [Header("PracticeGame")]
        public GameObject PanelPracticRecordMode;
        public Text P_Point;
        public Text P_Time;
        public Button btn_StartPractice;

        public Button FilterByAllContent_btn;
        public Button FilterByFriend_btn;
        public MatchRecordPlayerCard playerCard;
        public RectTransform Content;
        private List<MatchRecordPlayerCard> listplayer;

        private ServerUI server;

        private float H;
        private float M;
        private float S;

        private void OnEnable()
        {
            server = FindObjectOfType<ServerUI>();
            server.RequestGetRecordMode();
            server.OnMatchRecord += Server_OnMatchRecord;
            btn_Start.onClick.AddListener(() =>
            {
                server.RequestJoinRecordMode();
            });
            btn_StartPractice.onClick.AddListener(() =>
            {
                server.RequestJoinRecordMode();
            });
            FilterByAllContent_btn.onClick.AddListener(() => { LeaderboardFilterByAllContacts(); });
            FilterByFriend_btn.onClick.AddListener(() => { LeaderboardFilterByFriends(); });
        }



        private void OnDisable()
        {
            server.OnMatchRecord -= Server_OnMatchRecord;
            btn_Start.onClick.RemoveAllListeners();
            btn_StartPractice.onClick.RemoveAllListeners();
            FilterByAllContent_btn.onClick.RemoveAllListeners();
            FilterByFriend_btn.onClick.RemoveAllListeners();
            ClearListPlayer();
            CancelInvoke("RunTimer");
        }
        private void Server_OnMatchRecord(MatchRecordModeData obj)
        {
            initMatchRecord(obj);
        }
        private void initMatchRecord(MatchRecordModeData data)
        {
            listplayer = new List<MatchRecordPlayerCard>();


            CalculateTime(data.remainingTime);
            if (data.start)
            {

                ChangePanel(data.start);
                Point.text = data.point.ToString();
                Rank.text = data.rank;
                CostCoin_txt.text = (data.cost).ToString();
                SetFailTry(data.tryToggle);
            }
            else

            {
                ChangePanel(false);
                P_Point.text = data.point.ToString();
                P_Time.text = data.rank;
            }
            Leaderbord(data.leaderboardPlayers);

        }
        private void ChangePanel(bool start)
        {
            if (start)
            {
                PanelRecordMode.SetActive(true);
                PanelPracticRecordMode.SetActive(false);
            }
            else
            {
                PanelRecordMode.SetActive(false);
                PanelPracticRecordMode.SetActive(true);
            }
        }
        private void Leaderbord(List<LeaderboardPlayer> players)
        {
            ClearListPlayer();
            for (int i = 0; i < players.Count; i++)
            {
                var card = Instantiate(playerCard, Content);
                var image = ImageProfileContainer.LoadImage(players[i].avatar);
                card.Set(image, i + 1, players[i].userName, players[i].point, players[i].time, players[i].tryToggle);
                listplayer.Add(card);

            }
        }


        private void LeaderboardFilterByAllContacts()
        {
            for (int i = 0; i < listplayer.Count; i++)
            {
                listplayer[i].gameObject.SetActive(true);
            }


        }

        private void LeaderboardFilterByFriends()
        {

            for (int i = 0; i < listplayer.Count; i++)
            {
                listplayer[i].gameObject.SetActive(false);
                for (int j = 0; j < server.BODY.social.friends.Count; j++)
                {
                    if (listplayer[i].Username.text == server.BODY.social.friends[j].userName)
                    {
                        listplayer[i].gameObject.SetActive(true);
                    }


                }
            }

        }
    


        private void SetFailTry(int count)
        {
            TryToggle[0].isOn = false;
            TryToggle[1].isOn = false;
            TryToggle[2].isOn = false;
            for (int i = 0; i <count; i++)
            {
                TryToggle[i].isOn = true;
            }
        }
        private void ClearListPlayer()
        {
            for (int i = 0; i < listplayer.Count; i++)
            {
                Destroy(listplayer[i].gameObject);
            }
            listplayer.Clear();
          
        }

        private void CalculateTime(float time)
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


            ReminingTime.text = H + ":" + M + ":" + S;
            if (S == 0 && M == 0 && H == 0)
            {
                CancelInvoke("RunTimer");
                if (PanelPracticRecordMode.activeSelf)
                {
                    server.RequestGetRecordMode();
                }

            }
        }
    }
    [Serializable]
    public struct LeaderboardPlayer
    {
        public string avatar;
        public string userName;
        public int point;
        public string time;
        public int tryToggle;

    }
    [Serializable]
    public  struct MatchRecordModeData
    {
        public bool start;

        public int remainingTime;

        public int point;
        public string rank;
        public int cost;
        public int tryToggle;
        

        public List<LeaderboardPlayer> leaderboardPlayers;
    }
}