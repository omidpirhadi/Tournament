using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;
namespace Diaco.FirebasePushNotificaton
{
    public class FirebasePushNotification : MonoBehaviour
    {
        private void Awake()
        {
           var s =  Firebase.FirebaseApp.DefaultInstance;
           
        }

        void Start()
        {
            

            FirebaseMessaging.TokenReceived += FirebaseMessaging_TokenReceived;
            FirebaseMessaging.MessageReceived += FirebaseMessaging_MessageReceived;
        }

        private void FirebaseMessaging_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.Log($"Message:{e.Message.From}");

        }

        private void FirebaseMessaging_TokenReceived(object sender, TokenReceivedEventArgs e)
        {
            Debug.Log($"Token:RoyallBall");
        }
    }
}