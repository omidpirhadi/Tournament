using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Reports
{

    public class ReportMyTeams : MonoBehaviour
    {
        ServerUI server;
        [Header("AwardElement")]
        public Text TimeRemaining;
        public Text AwardGem;
        public Text AwardCoin;
        public Text TotalAwardGem;
        public Text TotalAwardCoin;
        public Button ButtonWithdrawAward;
        [Header("CreatedTeamsViweElement")]
        public RectTransform Content;
        public ReportCreatedTeamsCard CreatedTeamsCardElement;

        private List<ReportCreatedTeamsCard> listCreateTeams = new List<ReportCreatedTeamsCard>();



        public void InitializeMyTeams(MyTeamsData myTeams )
        {
            server = FindObjectOfType<ServerUI>();
            TimeRemaining.text = myTeams.award.timeRemaining;
            AwardGem.text = (myTeams.award.awardGem).ToString();
            AwardCoin.text = (myTeams.award.awardCoin).ToString();
            TotalAwardGem.text = (myTeams.award.totalawardGem).ToString();
            TotalAwardCoin.text = (myTeams.award.totalawardGem).ToString();
            for (int i = 0; i < myTeams.createdTeams.Count; i++)
            {
                var card = Instantiate(CreatedTeamsCardElement, Content);

                card.InitializReportCreatedTeamsCard(
                    server.ImageGameType.LoadImage(myTeams.createdTeams[i].avatar),
                     myTeams.createdTeams[i].teamName,
                     myTeams.createdTeams[i].capacityTeam,
                     myTeams.createdTeams[i].cost,
                     () => { Debug.Log("Show Info Team Created"); });
                listCreateTeams.Add(card);
                    
            }
        }
        public void ClearCardTeamCreated()
        {
            for (int i = 0; i < listCreateTeams.Count; i++)
            {
                Destroy(listCreateTeams[i].gameObject);
            }
            listCreateTeams.Clear();
        }
    }

    [Serializable]
    public struct Award
    {
        public string timeRemaining;
        public short awardGem;
        public short awardCoin;
        public short totalawardGem;
        public short totalawardCoin;
    }
    [Serializable]
    public struct CreatedTeam
    {
        public string avatar;
        public string teamName;
        public string capacityTeam;
        public short cost;
    }
    [Serializable]
    public struct MyTeamsData
    {
        public Award award;
        public List<CreatedTeam> createdTeams;
    }
}