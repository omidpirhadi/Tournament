using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Notification
{

    public class NotificationInvitMatch : MonoBehaviour
    {
        public PlayeAnimationUI animationUI;
        public _GameLobby GetGame;
        public _SubGame subGame;
        public Text username;
        public Text Message;
        public Button AcceptRequestButton;
        public Button IgnorRequestButton;
        void OnEnable()
        {
            AcceptRequestButton.onClick.AddListener(AcceptInvite);
            IgnorRequestButton.onClick.AddListener(RejectInvite);
       
        }
        void OnDisable()
        {
            AcceptRequestButton.onClick.RemoveListener(AcceptInvite);
            IgnorRequestButton.onClick.RemoveListener(RejectInvite);
        }
    
        public void Show()
        {
            animationUI.playanimation();
        }
        void AcceptInvite()
        {
            var server = FindObjectOfType<ServerUI>();
            server.AcceptRequsetPlayGameWithFriend(username.text, (short)GetGame, (short)subGame);
        }
        void RejectInvite()
        {
            Debug.Log("RejectInvite");
            animationUI.playanimation();
        }
    }
}