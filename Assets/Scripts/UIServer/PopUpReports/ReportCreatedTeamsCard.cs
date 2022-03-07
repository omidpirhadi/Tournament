using System;

using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Reports
{
    public class ReportCreatedTeamsCard : MonoBehaviour
    {
        public Image Profile;
        public Text TeamName;
        public Text CapacityTeam;
        
        public Image Cost;

  

        private Button BtnCard;
        private ServerUI server;
        public void InitializReportCreatedTeamsCard(Sprite profile, string teamName, string capacityTeam , short costType,Action OnClickCard )
        {
            BtnCard = this.GetComponent<Button>();
            server = FindObjectOfType<ServerUI>();
            Profile.sprite = profile;
            TeamName.text = teamName;
            CapacityTeam.text = capacityTeam;
            if(costType == 0 )
            {
                Cost.sprite = server.ImageTypeCosts.LoadImage("0");
            }
            else
            {
                Cost.sprite = server.ImageTypeCosts.LoadImage("1");
            }
            BtnCard.onClick.AddListener(()=> { OnClickCard(); });
        }
    }
}