using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
namespace Diaco.Social
{
    public class TabFirendsController : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationUi;
        public InputField InputSearch;
        public Button SearchButton;
        public Diaco.Chat.ChatBoxController chat_box;
        public CardFriendTypeOne CardwithMessage;
        public CardFriendTypeTwo CardwithReqFriend;
        public RectTransform Content;
        public List<CardFriendTypeOne> Cardone;
        public List<CardFriendTypeTwo> Cardtwo;
        public void Awake()
        {
            Server.OnComingFriends += Server_OnComingFriends;
            Server.OnResualtSearchFriend += Server_OnResualtSearchFriend;
            SearchButton.onClick.AddListener(() => {
                Server.SearchFriendRequest(InputSearch);
                InputSearch.text = "";
            });
        }
        public void OnEnable()
        {
           
        }
        public void OnDisable()
        {
           // Server.OnComingFriends -= Server_OnComingFriends;
           // Server.OnResualtSearchFriend -= Server_OnResualtSearchFriend;
           // SearchButton.onClick.RemoveAllListeners();
            ClearListCard();
        }
        private void Server_OnResualtSearchFriend(SearchUser friend)
        {
            SpawnCardsAddFriend(friend);
        }

        private void Server_OnComingFriends(Friends friends)
        {
            SpawnCardsFriend(friends);
        }

        private void SpawnCardsAddFriend(SearchUser friends)
        {
            ClearListCard();


            if (friends.friend == 0)///addfriend
            {
                var card = Instantiate(CardwithReqFriend, Content);
                card.UserName.text = friends.userName;
                card.Avatar.sprite = Server.AvatarContainer.LoadImage(friends.avatar);
                card.Cup.text = friends.cup.ToString();
                if (friends.isOnline)
                    card.img_IsOnline.enabled = true;
                else
                    card.img_IsOnline.enabled = false;
                card.btn_Add.interactable = true;
                card.btn_Add.onClick.AddListener(() => { Server.RequsetAddFriend(friends.userName); card.btn_Add.interactable = false; });
                Cardtwo.Add(card);
            }
            else if (friends.friend == 1)//SendedReq
            {
                var card = Instantiate(CardwithReqFriend, Content);
                card.UserName.text = friends.userName;
                card.Avatar.sprite = Server.AvatarContainer.LoadImage(friends.avatar);
                card.Cup.text = friends.cup.ToString();
                if (friends.isOnline)
                    card.img_IsOnline.enabled = true;
                else
                    card.img_IsOnline.enabled = false;
                card.btn_Add.interactable = false;
                Cardtwo.Add(card);
            }
            else if (friends.friend == 2)///Is friend
            {
                var card = Instantiate(CardwithMessage, Content);
                card.UserName.text = friends.userName;
                card.Avatar.sprite = Server.AvatarContainer.LoadImage(friends.avatar);
                card.Cup.text = friends.cup.ToString();
                if (friends.isOnline)
                {
                    card.img_IsOnline.enabled = true;
                    //  Debug.Log("FriendOnline");
                }
                else
                {
                    card.img_IsOnline.enabled = false;
                    ///   Debug.Log("Friendoffline");
                }
                card.btn_chat.interactable = true;
                card.btn_chat.onClick.AddListener(() =>
                {


             
                    chat_box.SetElementPage(card.Avatar.sprite, card.UserName.text, card.Cup.text);
                    Server.SendCurrentPage("chat", card.UserName.text);
                    navigationUi.ShowPopUp("chat");

                });
                Cardone.Add(card);

            }
        }
        private void SpawnCardsFriend(Friends friends)
        {
            ClearListCard();
            //var friendslist = friends.friends;
            for (int i = 0; i < friends.friends.Count; i++)
            {
                var card = Instantiate(CardwithMessage, Content);
                card.UserName.text = friends.friends[i].userName;
                card.Avatar.sprite = Server.AvatarContainer.LoadImage(friends.friends[i].avatar);
                card.Cup.text = friends.friends[i].cup.ToString();
                if (friends.friends[i].isOnline)
                {
                    card.img_IsOnline.enabled = true;
                    //    Debug.Log("FriendOnline2");
                }
                else
                {
                    card.img_IsOnline.enabled = false;
                    //     Debug.Log("Friendoffline");
                }
                card.btn_chat.interactable = true;
                card.btn_chat.onClick.AddListener(() =>
                {

                    chat_box.AvatarReciver.sprite = card.Avatar.sprite;
                    chat_box.UserNameReciver.text = card.UserName.text;
                    chat_box.Cup.text = card.Cup.text;
                    navigationUi.ShowPopUp("chat");
                });
                Cardone.Add(card);
            }
        }
        private void ClearListCard()
        {

            for (int i = 0; i < Cardone.Count; i++)
            {
                Destroy(Cardone[i].gameObject);

                Debug.Log("ClearFriendTab");
            }
            for (int i = 0; i < Cardtwo.Count; i++)
            {

                Destroy(Cardtwo[i].gameObject);
                Debug.Log("ClearFriendTab");
            }
            Cardone.Clear();
            Cardtwo.Clear();

        }
    }
}