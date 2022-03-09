using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Social.MessageCards

{
    public class MessagesTabCardReqFriend : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI NavigationUi;
        public Button OpenProfilePopupButton;
        public string ID;
        public Image OnlineIndicator;
        public Image ProfileImageIndicator;
        public Text UserNameIndicator;
        public Text CupCountIndicator;
        public Button Accept;
        public Button Reject;

        private void OnEnable()
        {
            NavigationUi = FindObjectOfType<NavigationUI>();
            Server = FindObjectOfType<ServerUI>();
            OpenProfilePopupButton.onClick.AddListener(() =>
            {



                NavigationUi.ShowPopUp("profilefromteam");
                Server.GetProfilePerson(UserNameIndicator.text);
            });
        }
        private void OnDisable()
        {
            OpenProfilePopupButton.onClick.RemoveAllListeners();
        }
        public void SetCard(Sprite profileimage, string username, string cupcount, bool isonline, Action accept, Action reject)
        {
            if (isonline)
            {
                OnlineIndicator.enabled = true;
            }
            else
            {
                OnlineIndicator.enabled = false;
            }
            ProfileImageIndicator.sprite = profileimage;
            UserNameIndicator.text = username;
            CupCountIndicator.text = cupcount;
            Accept.onClick.AddListener(() => { accept(); });
            Reject.onClick.AddListener(() => { reject(); });

        }
    }
}