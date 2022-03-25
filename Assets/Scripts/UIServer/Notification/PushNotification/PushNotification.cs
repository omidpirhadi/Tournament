using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

namespace Diaco.Notification
{


    public class PushNotification : MonoBehaviour
    {
        public int NumberNotification
        {
            get;set;
        }
        private AndroidNotificationChannel channel;
        private AndroidNotification notification;
        public void CreateNotificationChannel()
        {
            channel = new AndroidNotificationChannel();
            channel.Id = "diacostudio";
            channel.Name = "royallball";
            channel.Description = "localnotifications";
            channel.Importance = Importance.High;
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

           

            
        }
        public void SendNotification(string title, string context , double addminutes)
        {
            notification.Number = NumberNotification;
            notification.Color = Color.green;
            notification.SmallIcon = "small_ic";
            notification.LargeIcon = "large_ic";
            notification.Title = title;
            notification.Text = context;
            notification.FireTime = System.DateTime.Now.AddMinutes(addminutes);
            AndroidNotificationCenter.SendNotification(notification, "diacostudio");
            NumberNotification++;
        }
        public void CancleNotification()
        {
           // AndroidNotificationCenter.ca
        }
    }
}