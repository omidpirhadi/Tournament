using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Chat
{
    public class FramChat : MonoBehaviour
    {
        public Text UserName;
        public Image Avatar;
        public Text Date;
        public Text Time;
        public Text Context;
        public Image ReadMessageIndicator;
        //public bool Read;

        public void FillFrameChat(string context, string time, string date, bool readmessage)
        {
            this.Context.text = context;
            this.Time.text = time;
            this.Date.text = date;
            if (readmessage)
            {
                ReadMessageIndicator.enabled = true;
            }
            else
            {
                ReadMessageIndicator.enabled = false;
            }
        }
        public void FillFrameChat(string username , Sprite avatar, string context, string time, string date)
        {
            this.Context.text = context;
            this.Time.text = time;
            this.Date.text = date;
            this.UserName.text = username;
            this.Avatar.sprite = avatar;

        }
    }
}