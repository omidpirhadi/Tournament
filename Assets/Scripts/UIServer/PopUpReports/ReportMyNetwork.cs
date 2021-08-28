using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Reports
{


    public class ReportMyNetwork : MonoBehaviour
    {
        ServerUI server;
        [Header("AwardElements")]
        public Text TimeRemaining;
        public Text AwardGem;
        public Text AwardCoin;
        public Text ReciveAwardGem;
        public Text ReciveAwardCoin;
        public Text TotalInvitedPlayer;
        public Button Withdraw;
        [Header("InvitedPlayer")]
        public RectTransform Content;
        public ReportInvitedPlayerCard InvitedPlayerCardElement;
        private List<ReportInvitedPlayerCard> listreportInvitedPlayerCards;


        public void InitializeMyTeams(MYNETWORK myNetwork)
        {
            listreportInvitedPlayerCards = new List<ReportInvitedPlayerCard>();
            TimeRemaining.text = myNetwork.award.timeRemaining;
            AwardGem.text = (myNetwork.award.awardGem).ToString();
            AwardCoin.text = (myNetwork.award.awardCoin).ToString();
            ReciveAwardGem.text = (myNetwork.award.recivedawardGem).ToString();
            ReciveAwardCoin.text = (myNetwork.award.recivedawardCoin).ToString();
            TotalInvitedPlayer.text = (myNetwork.award.totalinvitedPlayer).ToString();
            for (int i = 0; i < myNetwork.PlayerInviteds.Count; i++)
            {
              var card = Instantiate(InvitedPlayerCardElement, Content);

                card.InvitedPlayerCard(
                    server.ImageGameType.LoadImage(myNetwork.PlayerInviteds[i].profile),
                    myNetwork.PlayerInviteds[i].name,
                    myNetwork.PlayerInviteds[i].cup,
                    
                     () => { Debug.Log("Show Info MYneTWORRk"); }

                    );
                listreportInvitedPlayerCards.Add(card);
            }
        }
        public void ClearCardTeamCreated()
        {
            for (int i = 0; i < listreportInvitedPlayerCards.Count; i++)
            {
                Destroy(listreportInvitedPlayerCards[i].gameObject);
            }
            listreportInvitedPlayerCards.Clear();
        }

    }
    [Serializable]
    public struct AwardMyNetwork
    {
        public string timeRemaining;
        public short awardGem;
        public short awardCoin;
        public short recivedawardGem;
        public short recivedawardCoin;
        public short totalinvitedPlayer;
    }
    [Serializable]
    public struct PlayerInvited
    {
        public string profile;
        public string name;
        public string cup;
        
    }
    [Serializable]
    public struct MYNETWORK
    {
        public AwardMyNetwork award;
        public List<PlayerInvited> PlayerInviteds;
    }
}