﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Diaco.Social.TeamsInputField;

namespace Diaco.Social
{
    public class CreateTeamTabController : MonoBehaviour
    {

        public ServerUI Server;
        public SelectFriendsPopup SelectFriendPopup;
        public SelectBadgePopUpController SelectBadgesController;

      /*  public DialogYesNo Dialog_CreateTeam;
        public DialogOk Dialog_Error_Ticket;
        public DialogOk Dialog_Error_Cup;
        public DialogOk Dialog_Error_Gem;
        public DialogOk Dialog_Error_Coin;*/

        public Text soccerLeaguePrecent_text;
        public Text billiardLeaguePrecent_text;
        public Text ReminingTimeToCreateLeague_text;

        public TMPro.TMP_InputField TeamName;
        public TMPro.TMP_InputField Description;

        public InputFieldSocial Game;
        public InputFieldSocial Mode;
        public InputFieldSocial TypeCost;
        public InputFieldSocial Cost;
        public InputFieldSocial Capacity;
        public InputFieldSocial Hour;
        public InputFieldSocial Min;

        public Button CreateButton;
        public Button InviteFriendButton;
        public Button TabButton;
        public int Tickets = 0;
        public List<Toggle> TicketsIndicator;
        public List<string> FriendsAdded;
        public string BadgeID = "";

        public void Awake()
        {
           
        }



        public void OnEnable()
        {

            SelectFriendPopup.OnSelectFriendInPopup += SelectFriendPopup_OnSelectFriendInPopup;
            SelectBadgesController.OnChangeBadgeId += SelectBadgesController_OnChangeBadgeId;
          ////  Server.OnCreateTeamCompeleted += Server_OnCreateTeamCompeleted;

           // Server.OnErrorCreateTeam += Server_OnErrorCreateTeam;
           /// Dialog_CreateTeam.OnClickYes += Dialog_CreateTeam_OnClickYes;

            CreateButton.onClick.AddListener(() =>
            {
                if (checkField())
                {
                    Server.RequestCreateTeam(CreateTeam());

                }
            });
            InviteFriendButton.onClick.AddListener(() => { FriendsAdded.Clear(); });
            TabButton.onClick.AddListener(() => {
                
                Server.RequestLeagueRules();
            });
            Server.RequestLeagueRules();
            // Tickets = Server.BODY.inventory.tickets;
            // ShowTickets();
        }



        public void OnDisable()
        {
             SelectFriendPopup.OnSelectFriendInPopup -= SelectFriendPopup_OnSelectFriendInPopup;
            SelectBadgesController.OnChangeBadgeId -= SelectBadgesController_OnChangeBadgeId;
            //Server.OnCreateTeamCompeleted -= Server_OnCreateTeamCompeleted;
          //  Server.OnErrorCreateTeam -= Server_OnErrorCreateTeam;
           // Dialog_CreateTeam.OnClickYes -= Dialog_CreateTeam_OnClickYes;
            CreateButton.onClick.RemoveAllListeners();
            TabButton.onClick.RemoveAllListeners();
            InviteFriendButton.onClick.RemoveAllListeners();
            FriendsAdded.Clear();
            BadgeID = "";
        }
        private void Dialog_CreateTeam_OnClickYes()
        {
           
        }
     /*   private void Server_OnCreateTeamCompeleted()
        {
           // Tickets = Server.BODY.inventory.tickets;
            //ShowTickets();
        }*/
      /*  private void Server_OnErrorCreateTeam(string error)
        {
            if(error == "1")
            {
                Dialog_Error_Ticket.ShowDialog();
            }
            else if(error == "2")
            {
                Dialog_Error_Coin.ShowDialog();
            }
            else if (error == "3")
            {
                Dialog_Error_Cup.ShowDialog();
            }
            else if (error == "4")
            {
                Dialog_Error_Gem.ShowDialog();
            }

        }*/

        private void SelectBadgesController_OnChangeBadgeId(string badge)
        {
            BadgeID = badge;
        }

        private void SelectFriendPopup_OnSelectFriendInPopup(List<string> list)
        {
            list.ForEach((e) =>
            {
                FriendsAdded.Add(e);
            });
        }
      
        public Diaco.HTTPBody.CreateTeam CreateTeam()
        {
            var Team = new Diaco.HTTPBody.CreateTeam();

            Team.name = TeamName.text;
            Team.description = Description.text;
            Team.game = Game.CurrentElementContext;
            Team.mode = Mode.CurrentElementContext;
           
                Team.typeCost = TypeCost.CurrentElementContext;
            Team.cost = Cost.CurrentValueDigit;
            Team.capacity = System.Convert.ToInt32(Capacity.ElementContexts[Capacity.CurrentElementContext]);
            Team.hour = Hour.CurrentValueDigit;
            Team.min = Min.CurrentValueDigit;
            Team.invitation = FriendsAdded;
            Team.leagueFlag = BadgeID;

            return Team;
        }
        public void Rules(RulesData data)
        {
            soccerLeaguePrecent_text.text = data.soccerAward;
            billiardLeaguePrecent_text.text = data.billiardAward;
            ReminingTimeToCreateLeague_text.text = data.remainingTime;
        }
        [Obsolete]
        public void ShowTickets()
        {
            if (Tickets == 0)
            {
                TicketsIndicator[0].isOn = false;
                TicketsIndicator[1].isOn = false;
                TicketsIndicator[2].isOn = false;
            }
            else if (Tickets == 1)
            {
                TicketsIndicator[0].isOn = true;
                TicketsIndicator[1].isOn = false;
                TicketsIndicator[2].isOn = false;
            }
            else if (Tickets == 2)
            {
                TicketsIndicator[0].isOn = true;
                TicketsIndicator[1].isOn = true;
                TicketsIndicator[2].isOn = false;
            }
            else if (Tickets == 3)
            {
                TicketsIndicator[0].isOn = true;
                TicketsIndicator[1].isOn = true;
                TicketsIndicator[2].isOn = true;
            }
        }
        private bool checkField()
        {
            bool Fill = true;

            if (TeamName.text == "")
            {
                Fill = false;
            }
            if (Description.text == "")
            {
                Fill = false;
            }
            if (Game.FieldOfViwe.text == "")
            {
                Fill = false;
            }
            if (Mode.FieldOfViwe.text == "")
            {
                Fill = false;
            }
            if (TypeCost.FieldOfViwe.text == "")
            {
                Fill = false;
            }
            if (Cost.FieldOfViwe.text == "")
            {
                Fill = false;
            }
            if (Capacity.FieldOfViwe.text == "")
            {
                Fill = false;
            }
            if (Hour.FieldOfViwe.text == "")
            {
                Fill = false;
            }
            if (Min.FieldOfViwe.text == "")
            {
                Fill = false;
            }
           /*if (FriendsAdded.Count == 0)
            {
                Fill = false;
            }*/
            return Fill;
        }
    }
    [Serializable]
    public  struct RulesData

    {
        public string soccerAward;
        public string billiardAward;
        public string remainingTime;

    }
}