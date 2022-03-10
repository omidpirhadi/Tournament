using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
namespace Diaco.UI.SocialTabs
{
    public class TabFirendsController : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationUi;
        public InputField InputSearch;
        public Button SearchButton;
        public FriendTabCard FriendTabCard_Prefab;

        public RectTransform Content;
        private List<GameObject> listfriendcard = new List<GameObject>();
        

        public void OnEnable()
        {
            Server.OnComingFriends += Server_OnComingFriends;
            Server.OnResualtSearchFriend += Server_OnResualtSearchFriend;
            SearchButton.onClick.AddListener(() => {
                Server.SearchFriendRequest(InputSearch);
                InputSearch.text = "";
            });
        }
        public void OnDisable()
        {
           Server.OnComingFriends -= Server_OnComingFriends;
           Server.OnResualtSearchFriend -= Server_OnResualtSearchFriend;
           SearchButton.onClick.RemoveAllListeners();
            ClearListCard();
        }
        private void Server_OnResualtSearchFriend(SearchUser friend)
        {
            SpawnCardsNotFriend(friend);
        }

        private void Server_OnComingFriends(Friends friends)
        {
            SpawnCardsFriend(friends);
        }

        private void SpawnCardsNotFriend(SearchUser data)
        {
            ClearListCard();
            var card = Instantiate(FriendTabCard_Prefab, Content);
            card.SetForNoFriend(data);
            listfriendcard.Add(card.gameObject);

        }
        private void SpawnCardsFriend(Friends data)
        {
            ClearListCard();
            //var friendslist = friends.friends;
            for (int i = 0; i < data.friends.Count; i++)
            {
               var card = Instantiate(FriendTabCard_Prefab, Content);

                card.SetForFriend(data.friends[i]);
                listfriendcard.Add(card.gameObject);
            }
        }
        private void ClearListCard()
        {

            for (int i = 0; i < listfriendcard.Count; i++)
            {
                Destroy(listfriendcard[i]);

                Debug.Log("ClearFriendTab");
            }
            listfriendcard.Clear();
        

        }
    }
}