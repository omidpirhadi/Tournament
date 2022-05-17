using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ResultGame : MonoBehaviour
{
    public Diaco.EightBall.Server.BilliardServer Server;
    //public Diaco.SoccerStar.Server.ServerManager Server;
    public GameLuncher luncher;
    public Image WinnerBackground;
    public Image LoserBackground;

    public Image PlayerOne;
    public Image PlayerLeftCostType;
    public Image PlayerTwo;
    public Image PlayerRightCostType;

    public Text PlayerOneUserName;
    public Text PlayerOneCup;
    public Text PlayerOneCoin;
    public Text PlayerOneXp;

    public Text PlayerTwoUserName;
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
    private void Awake()
    {

    }
    void Start()
    {
       // Server.OnGameResult += Server_OnGameResult;
        
        
    }
    void OnEnable()
    {
        DOVirtual.Float(0, 1, 2f, (x) => { }).OnComplete(() =>
        {
            RematchButton.interactable = true;
            CloseGameButton.interactable = true;

        });
        luncher = FindObjectOfType<GameLuncher>();
        Server.OnGameResult += Server_OnGameResult;
        RematchButton.onClick.AddListener(() =>
        {
            Server.Emit_PlayAgain();
            //  this.gameObject.SetActive(false);
            RematchButton.interactable = false;
        });
        CloseGameButton.onClick.AddListener(() =>
        {

            Server.Emit_LeftGame();
            // Server.CloseConnection();
            luncher.BackToMenu();
            CloseGameButton.interactable = false;
            this.gameObject.SetActive(false);
        });
        if (Server.Namespaceserver == "_competition")
            DOVirtual.Float(0, 1, 3f, (x) => { }).OnComplete(() =>
            {
                Server.Emit_LeftGame();
                //  Server.CloseConnection();
                luncher.BackToMenu();
                CloseGameButton.interactable = false;
                this.gameObject.SetActive(false);
            });


    }
    void  OnDisable()
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
    private void Server_OnGameResult(Diaco.EightBall.Structs.ResultGame result,bool playaginactive)
    {

       
        CloseGameButton.interactable = true;
        if (playaginactive)
            RematchButton.interactable = true;
        else
            RematchButton.interactable = false;
        //SetCostType(result.costType);
        if(Server.UserName.userName == result.winner.userName)
        {
            WinnerBackground.enabled = true;
            LoserBackground.enabled = false;
            EnableAddFriendButton(result.winner.friends, result.loser.userName);
            SetResultPageElements(true,
                 Server.Avatars.LoadImage(result.winner.avatar),
                 Server.Avatars.LoadImage(result.loser.avatar),

                 result.winner.userName,
                 result.winner.cup,
                 result.winner.coin,
                 result.winner.xp,
                 result.loser.userName,
                 result.loser.cup,
                 result.loser.coin,
                 result.loser.xp,
                 result.winner.rank
                 );
        }
        else if(Server.UserName.userName == result.loser.userName)
        {
            WinnerBackground.enabled = false;
            LoserBackground.enabled = true;
            EnableAddFriendButton(result.loser.friends, result.winner.userName);
           
            SetResultPageElements(false,
               Server.Avatars.LoadImage(result.loser.avatar),
               Server.Avatars.LoadImage(result.winner.avatar),
                result.loser.userName,
                result.loser.cup,
                result.loser.coin,
                result.loser.xp,
                result.winner.userName,
                result.winner.cup,
                result.winner.coin,
                result.winner.xp,
               result.loser.rank
               );
        }
       StartCoroutine(Server.ResetData());
    }


  
    public void EnableAddFriendButton( List<string>friends , string opponetUsername)
    {
        friends.ForEach((E) => {

            if(E == opponetUsername)
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
        if(cost == 0)
        {
            PlayerLeftCostType.sprite = Cup;
            PlayerRightCostType.sprite = Cup;
        }
        else if( cost == 1)
        {
            PlayerLeftCostType.sprite = Coin;
            PlayerRightCostType.sprite = Coin;
        }
        else if(cost == 2)

        {
            PlayerLeftCostType.sprite = Gem;
            PlayerRightCostType.sprite = Gem;
        }
    }
    public void SetResultPageElements(bool Winner, Sprite leftavatar, Sprite rightavatar, string leftusername, string leftCup, string leftcoin, string leftXP, string rightusername, string rightCup, string rightcoin, string rightXP, string rank)
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
