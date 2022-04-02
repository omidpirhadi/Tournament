using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.Notification
{


    public class Notification_Dialog_Manager : MonoBehaviour
    {
        public ServerUI server;
        public NotificationPallet notificationPallet;
        public DialogPallet dialogPallet;
        public TextPallet textPallet;
        public void init_Notification()
        {


            server.OnNotification += Server_OnNotification;
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
    }



    public enum AlartType { Notification = 0, Dialog = 1 ,RedText = 2}
    public enum NotificationType { TwoButton = 0 , HaveImage = 1}
    public enum DialogType { Alart_Ok = 0, Alart_YesNo = 1, Alart_Edit = 2 }

    
    public struct Notification_Dialog_Body
    {
        public string eventName;
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