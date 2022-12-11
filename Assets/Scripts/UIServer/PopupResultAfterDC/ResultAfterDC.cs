using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Diaco.UI.PopupResualtAfterDC
{
    public class ResultAfterDC : MonoBehaviour
    {
       // public ServerUI Server;
        public Diaco.ImageContainerTool.ImageContainer avatar;


        public Image WinnerBackground;
        public Image LoserBackground;

        public Image PlayerOne;
        public Image cup_left_logo_image;
        public Image PlayerLeftCostType;
        public Image PlayerTwo;
        public Image cup_right_logo_image;
        public Image PlayerRightCostType;

        public RTLTMPro.RTLTextMeshPro PlayerOneUserName;
        public Text GoalLeft;
        public Text PlayerOneCup;
        public Text PlayerOneCoin;
        public Text PlayerOneXp;

        public RTLTMPro.RTLTextMeshPro PlayerTwoUserName;
        public Text GoalRight;
        public Text PlayerTwoCup;
        public Text PlayerTwoCoin;
        public Text PlayerTwoXp;

        public Text Rank;

        public Button Close_Btn;


        public Sprite Coin;
        public Sprite Cup;
        public Sprite Gem;
        public Sprite BilliardCup_logo_sprite;
        public Sprite SoccerCup_logo_sprite;
        void OnEnable()
        {


            Close_Btn.onClick.AddListener(() => { gameObject.SetActive(false); });

        }
        void OnDisable()
        {

            Close_Btn.onClick.RemoveAllListeners();

        }
        void OnDestroy()
        {



        }
        public void Set(ResultAfterDCData result)
        {


            if (result.GameType == 0)// soccer
            {
                cup_left_logo_image.sprite = SoccerCup_logo_sprite;
                cup_right_logo_image.sprite = SoccerCup_logo_sprite;
                GoalLeft.enabled = true;
                GoalRight.enabled = true;
            }
            else/// billird
            {
                cup_left_logo_image.sprite = BilliardCup_logo_sprite;
                cup_right_logo_image.sprite = BilliardCup_logo_sprite;
                GoalLeft.enabled = false;
                GoalRight.enabled = false;
            }
            if (result.isWinner)
            {
                WinnerBackground.enabled = true;
                LoserBackground.enabled = false;

                SetResultPageElements(true,
                    avatar.LoadImage(result.winner.avatar),
                    avatar.LoadImage(result.loser.avatar),

                    result.winner.userName,
                    result.winner.goalCount,
                    result.winner.cup,
                    result.winner.coin,
                    result.winner.xp,
                    result.loser.userName,
                    result.loser.goalCount,

                    result.loser.cup,
                    result.loser.coin,
                    result.loser.xp,
                    result.winner.rank
                    );

            }
            else if (!result.isWinner)
            {
                WinnerBackground.enabled = false;
                LoserBackground.enabled = true;
                SetResultPageElements(false,
                   avatar.LoadImage(result.loser.avatar),
                   avatar.LoadImage(result.winner.avatar),
                    result.loser.userName,
                    result.loser.goalCount,
                    result.loser.cup,
                    result.loser.coin,
                    result.loser.xp,
                    result.winner.userName,
                result.winner.goalCount,
                    result.winner.cup,
                    result.winner.coin,
                    result.winner.xp,
                   result.loser.rank
                   );
            }

        }



        public void SetCostType(int cost)
        {
            if (cost == 0)
            {
                PlayerLeftCostType.sprite = Cup;
                PlayerRightCostType.sprite = Cup;
            }
            else if (cost == 1)
            {
                PlayerLeftCostType.sprite = Coin;
                PlayerRightCostType.sprite = Coin;
            }
            else if (cost == 2)

            {
                PlayerLeftCostType.sprite = Gem;
                PlayerRightCostType.sprite = Gem;
            }
        }
        public void SetResultPageElements(bool Winner, Sprite leftavatar, Sprite rightavatar, string leftusername, int leftgoal, string leftCup, string leftcoin, string leftXP, string rightusername, int rightgoal, string rightCup, string rightcoin, string rightXP, string rank)
        {
            if (Winner)
            {
                WinnerBackground.enabled = true;
                LoserBackground.enabled = false;

            }
            else
            {
                WinnerBackground.enabled = false;
                LoserBackground.enabled = true;
            }
            GoalLeft.text = leftgoal.ToString();
            GoalRight.text = rightgoal.ToString();
            PlayerOne.sprite = leftavatar;
            PlayerTwo.sprite = rightavatar;

            PlayerOneUserName.text = leftusername;

            PlayerOneCup.text = leftCup;
            PlayerOneCoin.text = leftcoin;
            PlayerOneXp.text = leftXP;

            PlayerTwoUserName.text = rightusername;
            PlayerTwoCup.text = rightCup;
            PlayerTwoCoin.text = rightcoin;
            PlayerTwoXp.text = rightXP;

            Rank.text = rank;
        }
    }
    [Serializable]
    public struct ResultPlayer
    {
        public string userName;
        public string avatar;

        public int goalCount;

        public string rank;
        public string coin;
        public string cup;
        public string xp;

    }
    [Serializable]
    public struct ResultAfterDCData
    {

        public ResultPlayer winner;
        public ResultPlayer loser;
        public bool isWinner;
        public int GameType; //0 = soccer ,,,,,1 = billiard

    }
}