using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
namespace Diaco.Social
{
    public class SelectFriendsPopup : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationUi;
        public RectTransform Content;
        public PopSelectFriendCard.CardFriend_selectPop CardFriend;
        public Button Send;
        public Text SelectedFriendIndicator;
        public List<string> FriendsSelectedList;

        private List<PopSelectFriendCard.CardFriend_selectPop> CardFriendList;
        private void Awake()
        {
           
        }
        private void OnEnable()
        {
            FriendsSelectedList = new List<string>();
            CardFriendList = new List<PopSelectFriendCard.CardFriend_selectPop>();
            var list = Server.BODY.social.friends;
            InitializeFriendsList(list);
            Send.onClick.AddListener(() => {
                handler_OnSelectFriend(FriendsSelectedList);
                ClearCardFriend();
                FriendsSelectedList.Clear();
                SelectedFriendIndicator.text = "0";
                navigationUi.ClosePopUp("selectfriend");
            });
        }
        private void OnDisable()
        {
            Send.onClick.RemoveAllListeners();
            ClearCardFriend();
            FriendsSelectedList.Clear();
           
        }

        public void InitializeFriendsList(List<FriendBody> friends)
        {
            for (int i = 0; i < friends.Count; i++)
            {
                if (friends[i].leagueBlock == true)
                {
                    var card = Instantiate(CardFriend, Content);
                    var image = Server.AvatarContainer.LoadImage(friends[i].avatar);
                    card.SetCard(image, friends[i].userName, friends[i].id, friends[i].cup.ToString());
                    card.SelectFriendPopController = this;
                    CardFriendList.Add(card);
                    // Debug.Log(friends[i].userName);
                }
            }
            Debug.Log("FriendsListLoaded");
        }
        public void ClearCardFriend()
        {
            for (int i = 0; i < CardFriendList.Count; i++)
            {
                Destroy(CardFriendList[i].gameObject);
            }
            CardFriendList.Clear();
        }

        private Action<List<string>> onselectfriend;
        public event Action<List<string>> OnSelectFriendInPopup
        {
            add { onselectfriend += value; }
            remove { onselectfriend -= value; }
        }

        protected void handler_OnSelectFriend(List<string> list)
        {
            if(onselectfriend != null)
            {
                onselectfriend(list);
            }
        }
    }
}