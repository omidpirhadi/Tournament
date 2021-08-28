﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.TeamInfo
{
    public class TeamInfoPopUpController : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationui;
        public AwardPopup.PopupAwardConrtoller PopupAwardConrtoller;
        public Diaco.TeamInfo.PlayerCard.CardPlayerTeamInfo PlayerCard;
        public RectTransform Content;

        public DialogYesNo Dialog_Change_Privacy;
        public DialogYesNo Dialog_LeaveTeam;
        public DialogYesNo Dialog_JoinTeam;



        public Image TeamImage;
        public Text TeamName;
        public Text Description;
        public Text Game; // biliard socor
        public Image CostType;// gem cup coin
        public Text Cost;
        public Text Capacity;
        public Text TeamMode;// public private
        public Text TeamTag;
        public Text Time;

        public Button AwardButton;
        public Button JoinButton;
        public Button LeaveTeamButton;

        public bool StatePrivacyPublic = false;
        public Sprite Enable_Private_Public;
        public Sprite Disbale_Private_Public;

        public Button Public_button;
        public Button Private_button;



        public Sprite GemCostAsset;
        public Sprite CupCostAsset;
        public Sprite CoinCostAsset;
         

        private float H;
        private float M;
        private float S;
        private List<Diaco.TeamInfo.PlayerCard.CardPlayerTeamInfo> templist_player_card;

        //private bool teamIsPublic = false;
       // private bool teamIsPrivate = false;
   
        public void Awake()
        {

            Dialog_Change_Privacy.OnClickYes += Dialog_Change_Privacy_OnClickYes;
            Dialog_JoinTeam.OnClickYes += Dialog_JoinTeam_OnClickYes;
            Dialog_LeaveTeam.OnClickYes += Dialog_LeaveTeam_OnClickYes;


            Server.OnGetTeamInfo += Server_OnGetTeamInfo;

            Private_button.onClick.AddListener(() =>
            {
                if (StatePrivacyPublic == true)
                    Dialog_Change_Privacy.ShowDialog(PersianFix.Persian.Fix("تیم خصوصی شود؟", 255));


            });
            Public_button.onClick.AddListener(() =>
            {
                if (StatePrivacyPublic == false)
                    Dialog_Change_Privacy.ShowDialog(PersianFix.Persian.Fix("تیم عمومی شود؟", 255));

            });
            AwardButton.onClick.AddListener(() =>
            {

                navigationui.ShowPopUp("teamaward");
            });
            JoinButton.onClick.AddListener(() =>
            {
                Dialog_JoinTeam.ShowDialog();

            });
            LeaveTeamButton.onClick.AddListener(() => {
                Dialog_LeaveTeam.ShowDialog();
            });

            
        }

        private void Dialog_LeaveTeam_OnClickYes()
        {
            Server.LeaveTheTeam();
            navigationui.ClosePopUp("teaminfo");
        }

        private void Dialog_JoinTeam_OnClickYes()
        {
            Server.JoinToTeam(TeamTag.text);

            JoinButton.gameObject.SetActive(false);
            navigationui.ClosePopUp("teaminfo");
        }

        private void Dialog_Change_Privacy_OnClickYes()
        {
            PrivacyToggleControll();
        }

        public void OnEnable()
        {
           
           // JoinButton.onClick.RemoveAllListeners();
          //  LeaveTeamButton.onClick.RemoveAllListeners();

  

            templist_player_card = new List<PlayerCard.CardPlayerTeamInfo>();
         
      
        }


        public void OnDisable()
        {
           // Server.OnGetTeamInfo -= (m){ };
          //  AwardButton.onClick.RemoveAllListeners();
          //  JoinButton.onClick.RemoveAllListeners();

         //   LeaveTeamButton.onClick.RemoveAllListeners();

          //  Public_toggle.onValueChanged.RemoveAllListeners();
         //   Private_toggle.onValueChanged.RemoveAllListeners();
            ClearTeamInfo();
            
        }
        private void Server_OnGetTeamInfo(Diaco.HTTPBody.TeamInfo teamInfos)
        {
            InitializeTeamInfo(teamInfos);
        }


        private void InitializeTeamInfo(Diaco.HTTPBody.TeamInfo teamInfos)
        {
            ClearTeamInfo();
            SetElementInfoTeam(teamInfos);
            SpawnPlayerCard(teamInfos);
            EnablePanelAdminInPage(Server.BODY.userName, teamInfos.from, teamInfos.mode);
            
           
            SendTagTeamToPopupAward();
            if(CheckMember(teamInfos) == 0)
            {
                JoinButton.gameObject.SetActive(true);
                LeaveTeamButton.gameObject.SetActive(false);
            }
            else if(CheckMember(teamInfos) == 1)
            {
                JoinButton.gameObject.SetActive(false);
                LeaveTeamButton.gameObject.SetActive(true);
            }
            else
            {
                JoinButton.gameObject.SetActive(false);
                LeaveTeamButton.gameObject.SetActive(false);
            }

        }
        private void SpawnPlayerCard(Diaco.HTTPBody.TeamInfo teamInfos)
        {
            for (int i = 0; i < teamInfos.members.Count; i++)
            {
                var card = Instantiate(PlayerCard, Content);
                var image = Server.AvatarContainer.LoadImage(teamInfos.members[i].avatar);
                var admin = false;
                if (i == 0)
                {
                    admin = true;
                }
                else
                {
                    admin = false;
                }
                card.SetCard(admin, image, teamInfos.members[i].userName, teamInfos.members[i].cup.ToString(),teamInfos.teamId);
                templist_player_card.Add(card);
            }
        }
        private void SetElementInfoTeam(Diaco.HTTPBody.TeamInfo teamInfos)
        {
            TeamImage.sprite = Server.BadgesContainer.LoadImage(teamInfos.avatar);
            TeamName.text = teamInfos.name;

            Description.text = teamInfos.description;
            if (teamInfos.game == 0)
            {
                Game.text = "ﺩﺭﺎﯿﻠﯿﺑ";
            }
            else
            {
                Game.text = "ﺮﮐﺎﺳ";
            }
            if(teamInfos.costType == 0)
            {
                CostType.sprite = CupCostAsset;
            }
            else if(teamInfos.costType == 1)
            {
                CostType.sprite = CoinCostAsset;
            }
            else
            {
                CostType.sprite = GemCostAsset;
            }

            Cost.text = teamInfos.cost.ToString();
            Capacity.text = teamInfos.capacity.ToString();

            if(teamInfos.mode == 0)
            {
                TeamMode.text = "ﻪﻧﺎﺘﺳﻭﺩ";
            }
            else
            {
                TeamMode.text = "ﯽﻣﻮﻤﻋ";

            }

            TeamTag.text = teamInfos.teamId;
            CalculateTime(teamInfos.remainingTime);
           /// Time.text = teamInfos.remainingTime;
    }
        private void EnablePanelAdminInPage(string username, string from , int mode)
        {

           /// temp_toggleCounter++;
            if (username == from)
            {
                Public_button.gameObject.SetActive(true);
                Private_button.gameObject.SetActive(true);

             
                if (mode == 0)///private team
                {

                    Private_button.image.sprite = Enable_Private_Public;
                    Public_button.image.sprite = Disbale_Private_Public;
                    StatePrivacyPublic = false;
                }
                else ///public team
                {

                    Private_button.image.sprite = Disbale_Private_Public;
                    Public_button.image.sprite = Enable_Private_Public;
                    StatePrivacyPublic = true;
                }

             
            }
            else
            {
                Public_button.gameObject.SetActive(false);
                Private_button.gameObject.SetActive(false);
         
            }
        }
        private int CheckMember(Diaco.HTTPBody.TeamInfo teamInfos)
        {
            if (Server.BODY.social.team.teamId == "")
            {
                return 0;
            }
            else if (Server.BODY.social.team.teamId == teamInfos.teamId)
            {
                
                return 1;
            }
            else
            {
                return -1;
            }

        }
        private void SendTagTeamToPopupAward()
        {

            PopupAwardConrtoller.TeamTag = TeamTag.text;
 
        }
        private  void PrivacyToggleControll()
        {
            if(StatePrivacyPublic  == false)
            {
                Private_button.image.sprite = Disbale_Private_Public;
                Public_button.image.sprite = Enable_Private_Public;
                StatePrivacyPublic = true;
                Server.RequestChangeTeamMode(1);
            }
            else///
            {
                Private_button.image.sprite = Enable_Private_Public;
                Public_button.image.sprite = Disbale_Private_Public;
                StatePrivacyPublic = false;
                Server.RequestChangeTeamMode(0);
            }

        }
        private void  ClearTeamInfo()
        {
            for (int i = 0; i < templist_player_card.Count; i++)
            {
                Destroy(templist_player_card[i].gameObject);
            }
            templist_player_card.Clear();
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
        public void GetInfoInBackToPopup()
        {
            Server.GetTeamInfo(TeamTag.text);
        }

    }
}