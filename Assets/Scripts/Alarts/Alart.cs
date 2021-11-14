using System;
using UnityEngine;


namespace Diaco.Alart
{

    public enum AlartType { Notification = 0, Dialog = 1, SmallPopup = 2 }

    public class Alart : MonoBehaviour
    {
        public void AlartShow(AlartData alartData)
        {
            if (alartData.alartType == (int)AlartType.Notification)
            {

            }
            else if (alartData.alartType == (int)AlartType.Dialog)
            {

            }
            else if (alartData.alartType == (int)AlartType.SmallPopup)
            {

            }

        }
    }
    [Serializable]
    public struct AlartData
    {
        public int alartType;//0 
        public string[] images;
        public string[] contexts;
        public int buttonType;
        public string command;
    }

}