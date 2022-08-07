using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

namespace Diaco.Notification
{


    public class PushNotification : MonoBehaviour
    {
        public ServerUI server;
        public  string ChannelID;
        public string ChannelName;
        public string ChannelDescription;
        public int NumberNotification
        {
            get;set;
        }
        private AndroidNotificationChannel channel;
        private AndroidNotification notification;

        public void InstantiateEvent()
        {
          CreateNotificationChannel();
            server.OnPushNotification += Server_OnPushNotification;
            server.OnPushNotificationCancle += Server_OnPushNotificationCancle;
        }

        private void Server_OnPushNotificationCancle(int id)
        {
            CancleNotification(id);
        }

        private void Server_OnPushNotification(PushNotifcationsData data)
        {
            SendNotifications(data);
        }

        public void CreateNotificationChannel()
        {
            channel = new AndroidNotificationChannel();
            channel.Id = ChannelID;
            channel.Name = ChannelName;
            channel.Description = ChannelDescription;
            channel.Importance = Importance.High;
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

           

            
        }
        public void SendNotifications(PushNotifcationsData data)
        {
            for (int i = 0; i < data.notifications.Count; i++)
            {


                notification.Number = NumberNotification;
                notification.Color = Color.green;
                notification.SmallIcon = "small_ic";
                notification.LargeIcon = "large_ic";
                notification.Title = data.notifications[i].title;
                notification.Text = data.notifications[i].context;
                notification.FireTime = System.DateTime.Now.AddMinutes(data.notifications[i].addMinutes);
                AndroidNotificationCenter.SendNotificationWithExplicitID(notification, ChannelID, data.notifications[i].id);
            }
        }
        public void CancleNotification(int id)
        {
            AndroidNotificationCenter.CancelNotification(id);
            Debug.Log("push cancel:" + id);
        }
                
    }
    [Serializable]
    public struct PushNotificationBody
    {
        public int id;
        public string title;
        public string context;
        public double addMinutes;
    }
    [Serializable]
    public struct PushNotifcationsData
    {
        public List<PushNotificationBody> notifications;
    }
}