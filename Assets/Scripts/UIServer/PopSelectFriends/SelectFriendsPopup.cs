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
            Send.onClick.AddListener(() => {
                handler_OnSelectFriend(FriendsSelectedList);
                ClearCardFriend();
                FriendsSelectedList.Clear();
                SelectedFriendIndicator.text = "0";
                navigationUi.ClosePopUp("selectfriend");
            });
        }
        private void OnEnable()
        {
            FriendsSelectedList = new List<string>();
            CardFriendList = new List<PopSelectFriendCard.CardFriend_selectPop>();
            var list = Server.BODY.social.friends;
            InitializeFriendsList(list);

        }
        private void OnDisable()
        {
            ClearCardFriend();
            FriendsSelectedList.Clear();
          //  Send.onClick.RemoveAllListeners();
        }

        public void InitializeFriendsList(List<FriendBody> friends)
        {
            for (int i = 0; i < friends.Count; i++)
            {
                var card = Instantiate(CardFriend, Content);
                var image = Server.AvatarContainer.LoadImage(friends[i].avatar);
                card.SetCard(image, friends[i].userName, friends[i].cup.ToString());
                card.SelectFriendPopController = this;
                CardFriendList.Add(card);
                Debug.Log(friends[i].userName);
            }
        }
        public void ClearCardFriend()
        {
            for (int i = 0; i < CardFriendList.Count; i++)
            {
                Destroy(CardFriendList[i].gameObject);
            }
            CardFriendList.Clear();
        }
        public event Action<List<string>> OnSelectFriendInPopup;

        protected void handler_OnSelectFriend(List<string> list)
        {
            if(OnSelectFriendInPopup != null)
            {
                OnSelectFriendInPopup(list);
            }
        }
    }
}