using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
using DG.Tweening;
namespace Diaco.SelectFriendForMatchs
{
    public class SelectFriendForMatch : MonoBehaviour
    {
        public enum Game { Soccer, Billiard};
        public Game game;
        public ServerUI Server;
        public RectTransform Content;
        
        public SelectFriendCardForMatch CardFriend;
       // public List<string> FriendsSelectedList;

        private List<SelectFriendCardForMatch> CardFriendList;

        private void OnEnable()
        {
            ///FriendsSelectedList = new List<string>();
            CardFriendList = new List<SelectFriendCardForMatch>();
            var list = Server.BODY.social.friends;
            InitializeFriendsList(list);

        }
        private void OnDisable()
        {
            ClearCardFriend();
           // FriendsSelectedList.Clear();
            //  Send.onClick.RemoveAllListeners();
        }

        public void InitializeFriendsList(List<FriendBody>friends)
        {
            for (int i = 0; i < friends.Count; i++)
            {
               
                var card = Instantiate(CardFriend, Content);
                var image = Server.AvatarContainer.LoadImage(friends[i].avatar);
                card.SetCard(
                    image,
                    friends[i].userName,
                    friends[i].cup.ToString(), 
                    () => {
                        if(game == Game.Soccer)
                        {
                            Server.RequestPlayGameWithFriend(card.Username.text, 0, 0);
                            DisableRequestButtonAfterRequest();
                        }
                        else
                        {
                            Server.RequestPlayGameWithFriend(card.Username.text, 1, 0);
                            DisableRequestButtonAfterRequest();
                        }

                    });
                    
               
                CardFriendList.Add(card);
               // Debug.Log(friends[i].userName);
            }
        }
        private void DisableRequestButtonAfterRequest()
        {
            for (int i = 0; i < CardFriendList.Count; i++)
            {
                CardFriendList[i].SendRequest_Button.interactable = false;

            }
            DOVirtual.Float(0, 1, 10, (x) =>
            {
            }).OnComplete(() =>
            {
                for (int i = 0; i < CardFriendList.Count; i++)
                {
                    CardFriendList[i].SendRequest_Button.interactable = true;
                }
            });

        }

        public void ClearCardFriend()
        {
            for (int i = 0; i < CardFriendList.Count; i++)
            {
                Destroy(CardFriendList[i].gameObject);
            }
            CardFriendList.Clear();
        }
    }
}