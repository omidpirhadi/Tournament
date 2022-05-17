using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ResultGameSoccer : MonoBehaviour
{
    // public Diaco.EightBall.Server.BilliardServer Server;
    public Diaco.SoccerStar.Server.ServerManager Server;
    public GameLuncher luncher;
    public Image WinnerBackground;
    public Image LoserBackground;

    public Image PlayerOne;
    public Image PlayerLeftCostType;
    public Image PlayerTwo;
    public Image PlayerRightCostType;

    public Text PlayerOneUserName;
    public Text GoalLeft;
    public Text PlayerOneCup;
    public Text PlayerOneCoin;
    public Text PlayerOneXp;

    public Text PlayerTwoUserName;
    public Text GoalRight;
    public Text PlayerTwoCup;
    public Text PlayerTwoCoin;
    public Text PlayerTwoXp;

    public Text Rank;

    public Button AddFriendButton;
    public Button EmojiButton;
    public Button RematchButton;
    public Button CloseGameButton;

    public Sprite Coin;
    public Sprite Cup;
    public Sprite Gem;

    void OnEnable()
    {
        luncher = FindObjectOfType<GameLuncher>();
        Server.OnGameResult += Server_OnGameResult;
        RematchButton.onClick.AddListener(() =>
        {
            Server.Emit_PlayAgain();
            RematchButton.interactable = false;
        });
        CloseGameButton.onClick.AddListener(() =>
        {

            Server.Emit_LeftGame();
            // Server.CloseSocket();
            luncher.BackToMenu();
            CloseGameButton.interactable = false;
            this.gameObject.SetActive(false);

        });
        if (Server.NamespaceServer == "_competition")
            DOVirtual.Float(0, 1, 3f, (x) => { }).OnComplete(() =>
            {
                Server.Emit_LeftGame();
                //  Server.CloseSocket();
                luncher.BackToMenu();
                CloseGameButton.interactable = false;
                this.gameObject.SetActive(false);
            });
    }
    void OnDisable()
    {
        Server.OnGameResult -= Server_OnGameResult;
        RematchButton.onClick.RemoveAllListeners();

        CloseGameButton.onClick.RemoveAllListeners();
    }
    void OnDestroy()
    {

        Server.OnGameResult -= Server_OnGameResult;
        RematchButton.onClick.RemoveAllListeners();

        CloseGameButton.onClick.RemoveAllListeners();
    }
    private void Server_OnGameResult(Diaco.EightBall.Structs.ResultGame result, bool playaginactive)
    {

        CloseGameButton.interactable = true;
        if (playaginactive)
            RematchButton.interactable = true;
        else
            RematchButton.interactable = false;

        // SetCostType(result.costType);
        if (Server.Info.userName == result.winner.userName)
        {
            WinnerBackground.enabled = true;
            LoserBackground.enabled = false;

            EnableAddFriendButton(result.winner.friends, result.loser.userName);

            SetResultPageElements(true,
                Server.imageContainer.LoadImage(result.winner.avatar),
                Server.imageContainer.LoadImage(result.loser.avatar),

                result.winner.userName,
                result.winner.goalcount,
                result.winner.cup,
                result.winner.coin,
                result.winner.xp,
                result.loser.userName,
                result.loser.goalcount,

                result.loser.cup,
                result.loser.coin,
                result.loser.xp,
                result.winner.rank
                );

        }
        else if (Server.Info.userName == result.loser.userName)
        {
            WinnerBackground.enabled = false;
            LoserBackground.enabled = true;

            EnableAddFriendButton(result.loser.friends, result.winner.userName);

            SetResultPageElements(false,
               Server.imageContainer.LoadImage(result.loser.avatar),
               Server.imageContainer.LoadImage(result.winner.avatar),
                result.loser.userName,
                result.loser.goalcount,
                result.loser.cup,
                result.loser.coin,
                result.loser.xp,
                result.winner.userName,
            result.winner.goalcount,
                result.winner.cup,
                result.winner.coin,
                result.winner.xp,
               result.loser.rank
               );
        }
        ///StartCoroutine(Server.ResetData());
    }


    public void EnableAddFriendButton(List<string> friends, string opponetUsername)
    {
        friends.ForEach((E) =>
        {

            if (E == opponetUsername)
            {
                AddFriendButton.gameObject.SetActive(false);
            }
            else
            {
                AddFriendButton.gameObject.SetActive(true);
            }
        });

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
