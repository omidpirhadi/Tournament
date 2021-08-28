using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
namespace Diaco.Social.MessageCards

{
    public class MessagesTabCardInviteToTeam : MonoBehaviour
    {
        public enum TypeGame { Billiard = 0 , Soccer = 1}
        public enum TypeCost { cup = 0, coin = 1, gem = 2 }

       // public NavigationUI NavigationUi;

        ///public Button OpenTeamPopupButton;

        public string TeamID = "";
        public Image GameTypeIndicator8ball;
        public Image GameTypeIndicatorsoccer;
        public Image TeamImageIndicator;
        public Text TeamNameIndicator;
        public Text CapacityIndicator;
        public Text TimeIndicator;
        public Image CostTypeIndicator;
        public Text CostCountIdicator;


        [FoldoutGroup("CardAssets")]
        public Sprite CostGemAsset;
        [FoldoutGroup("CardAssets")]
        public Sprite CostCoinAsset;
        [FoldoutGroup("CardAssets")]
        public Sprite CostCupAsset;


        private void OnDisable()
        {
            this.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        public void SetCard(TypeGame game , Sprite teamimage, string teamname, string capacity, string time, TypeCost cost, string costcount ,string idteam , Action onclick)
        {
            if(game ==  TypeGame.Billiard)
            {
                GameTypeIndicator8ball.enabled = true;
                GameTypeIndicatorsoccer.enabled = false;
            }
            else
            {
                GameTypeIndicator8ball.enabled = false;
                GameTypeIndicatorsoccer.enabled = true;
            }
            TeamImageIndicator.sprite = teamimage;
            TeamNameIndicator.text = teamname;
            CapacityIndicator.text = capacity;
            TimeIndicator.text = time;
            if(cost == TypeCost.coin)
            {
                CostTypeIndicator.sprite = CostCoinAsset;
            }
            else if( cost == TypeCost.cup)
            {
                CostTypeIndicator.sprite = CostCupAsset;
            }
            else if( cost == TypeCost.gem)
            {
                CostTypeIndicator.sprite = CostGemAsset;
            }
            CostCountIdicator.text = costcount;
            TeamID = idteam;
            this.GetComponent<Button>().onClick.AddListener(() => { onclick(); });
        }
    }
}