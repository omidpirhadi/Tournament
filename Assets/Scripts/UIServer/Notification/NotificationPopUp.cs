using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Diaco.Notification
{
  
    public class NotificationPopUp : MonoBehaviour
    {
        public NotificationMessage notificationMessage;
        public NotificationInvitMatch notificationInvit;
        public NotificationReminderTournument notifReminderTournument;

        public void NotificationInviteMatchShow(_GameLobby game,_SubGame subGame, string message, string username)
        {
            notificationInvit.GetGame = game;
            notificationInvit.subGame = subGame;
            notificationInvit.Message.text = message;
            notificationInvit.username.text = username;
            notificationInvit.gameObject.SetActive(true);
            notificationInvit.Show();
        }
   
    }
    
}