using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Chatbox
{
    public class FramChat : MonoBehaviour
    {
        public Text UserName;
        public Image Avatar;
        public Text Date;
        public Text Time;
        public RTLTMPro.RTLTextMeshPro Context;
        public StickerController StickerViwer;
        public Image ReadMessageIndicator;
        //public bool Read;

        /// <summary>
        /// with out avatar
        /// </summary>
        /// <param name="context"></param>
        /// <param name="time"></param>
        /// <param name="date"></param>
        /// <param name="readmessage"></param>
        public void FillFrameChatWithOutAvatar(string context, string time, string date, bool readmessage)
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

        public void FillFrameChatWithStickerAndWithOutAvatar(Sticker sticker, string time, string date, bool readmessage)
        {

            this.StickerViwer.sticker = sticker;


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
            StartCoroutine(this.StickerViwer.PlaySticker());
        }

        /// <summary>
        /// with profile avatar
        /// </summary>
        /// <param name="username"></param>
        /// <param name="avatar"></param>
        /// <param name="context"></param>
        /// <param name="time"></param>
        /// <param name="date"></param>
        public void FillFrameChatWithAvatar(string username, Sprite avatar, string context, string time, string date)
        {
            this.Context.text = context;
            this.Time.text = time;
            this.Date.text = date;
            this.UserName.text = username;
            this.Avatar.sprite = avatar;

        }
        public void FillFrameChatWithStickerAndWithAvatar(string username, Sprite avatar, Sticker sticker, string time, string date)
        {
            this.StickerViwer.sticker = sticker;
            this.Time.text = time;
            this.Date.text = date;
            this.UserName.text = username;
            this.Avatar.sprite = avatar;
            StartCoroutine(this.StickerViwer.PlaySticker());
        }

    }
}