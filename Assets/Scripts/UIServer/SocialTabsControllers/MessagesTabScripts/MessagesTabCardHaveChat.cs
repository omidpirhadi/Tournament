using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
namespace Diaco.Social.MessageCards

{
    public class MessagesTabCardHaveChat : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI NavigationUi;
        public Button OpenProfilePopupButton;
        public string UserID;
        public Image OnlineIndicator;
        public Image ProfileImageIndicator;
        public Text UserNameIndicator;
        public Text CountChatIndicator;
        public Button ShowChatBox;
        public Button DeleteChats;
        public string cup;
        private void OnEnable()
        {
            NavigationUi = FindObjectOfType<NavigationUI>();
            Server = FindObjectOfType<ServerUI>();
            OpenProfilePopupButton.onClick.AddListener(() => {



                
                Server.GetProfilePerson(UserID);
            });
        }
        private void OnDisable()
        {
            OpenProfilePopupButton.onClick.RemoveAllListeners();
        }
        public void SetCard(bool isonline, Sprite profileimage, string username, string countchat , Action chatboxaction, Action deletechataction)
        {
            if(isonline)
            {
                OnlineIndicator.enabled = true;
            }
            else
            {
                OnlineIndicator.enabled = false;
            }
            ProfileImageIndicator.sprite = profileimage;
            UserNameIndicator.text = username;
            CountChatIndicator.text = countchat;
            ShowChatBox.onClick.AddListener(() => { chatboxaction(); });
            DeleteChats.onClick.AddListener(() => { deletechataction(); });

        }
    }
}