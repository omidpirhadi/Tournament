using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.Notification.Manager
{


    public class Notification_Dialog_Manager : MonoBehaviour
    {

    }



    public enum AlartType { Notification = 0, Dialog = 1 }
    public enum NotificationType { InviteLeague = 0, InviteMatch = 1, Alart = 2 }
    public enum DialogType { Alart_Ok = 0, Alart_YesNo = 1, Alart_Edit = 2 }
    public struct Notification_Dialog_Body
    {
        public int alartType;
        public int notificationType;
        public int dialogType;
        public string ID;
        public string title;
        public string image;
        public string context;
    }
}