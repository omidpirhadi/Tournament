using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.Notification
{


    public class Notification_Dialog_Manager : MonoBehaviour
    {
        
        public ServerUI server;
        public Diaco.SoccerStar.Server.ServerManager server_soccer;
        public Diaco.EightBall.Server.BilliardServer server_billiard;

        public NotificationPallet notificationPallet;
        public DialogPallet dialogPallet;
        public TextPallet textPallet;
        public InternetConnectionPallet internetConnectionPallet;
        public void init_Notification_menu()
        {

            ClearEvent();
            server.OnNotification += Server_OnNotification;
        }

        public void init_Notification_soccer()
        {

            ClearEvent();
            server_soccer.OnNotification += Server_OnNotification;
        }
        public void init_Notification_billiard()
        {

            ClearEvent();
            server_billiard.OnNotification += Server_OnNotification;
        }
        public void InternetPingDialog(Notification_Dialog_Body body, bool show)
        {

            internetConnectionPallet.SetInternetDialog(body, show);

        }
        private void Server_OnNotification(Notification_Dialog_Body body)
        {

            if (body.alartType == 0)
            {
                notificationPallet.SetNotification(body);
            }
            else if (body.alartType == 1)
            {
                dialogPallet.SetDialog(body);
            }
            else if(body.alartType == 2)
            {
                textPallet.SetTextPallet(body);
            }

        }

        private void ClearEvent()
        {
            if (server)
                server.OnNotification -= Server_OnNotification;
            if (server_soccer)
                server_soccer.OnNotification -= Server_OnNotification;
            if (server_billiard)
                server_billiard.OnNotification -= Server_OnNotification;
        }
    }



    [Serializable]
    
    public struct Notification_Dialog_Body
    {
        public string closeEvent;
        public string eventName;
        public string eventData;
        public int actionButton; //0 =open page in event name ,  1 = emit eventName

        public int alartType;//Notification = 0, Dialog = 1
        public int notificationType;// TwoButton = 0 , Have Image With AcceptButtonOnly = 1
        public int dialogType;
       
        public string title;
        public string context;
        public string image;
        public string greenButtonText;
        
        public string redButtonText;
       
    }
}