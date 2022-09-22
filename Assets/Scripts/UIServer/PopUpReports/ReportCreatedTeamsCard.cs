using System;

using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Reports
{
    public class ReportCreatedTeamsCard : MonoBehaviour
    {
        public Image Profile;
        public RTLTMPro.RTLTextMeshPro TeamName;
        public Text CapacityTeam;
        public Text Cost_count;
        public Image Cost;

  

        private Button BtnCard;
        public Diaco.ImageContainerTool.ImageContainer CostImageContainer;
        public void InitializReportCreatedTeamsCard(Sprite profile, string teamName, string capacityTeam ,string cost_count, short costType,Action OnClickCard )
        {
            BtnCard = this.GetComponent<Button>();
           // server = FindObjectOfType<ServerUI>();
            Profile.sprite = profile;
            TeamName.text = teamName;
            CapacityTeam.text = capacityTeam;
           Cost_count.text = cost_count;
            if(costType == 0 )
            {
                Cost.sprite = CostImageContainer.LoadImage(costType.ToString());
            }
            else
            {
                Cost.sprite = CostImageContainer.LoadImage(costType.ToString()); 
            }
            BtnCard.onClick.AddListener(()=> { OnClickCard(); });
            Debug.Log("+++++++"+costType);
        }
    }
}