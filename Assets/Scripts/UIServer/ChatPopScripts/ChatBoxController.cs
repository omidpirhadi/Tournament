﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.Chatbox
{
    public class ChatBoxController : MonoBehaviour
    {
        public ServerUI Server;
        public List<Sticker> stickers;
       
        public string IDReciver = "";
        public Image AvatarReciver;
        public Text UserNameReciver;
        public Text Cup;
        public Scrollbar ChatScroll;
        public RectTransform ContentChats;
        public FramChat FrameMyChat;
        public FramChat FrameYouChat;
        public FramChat FrameMyChatWithSticker;
        public FramChat FrameYouChatWithSticker;
        public TMPro.TMP_InputField InputMessage;
        public GameObject StickerPanel_gameobject;
        public Button ShowStickerPanel_Button;
        public Button SendButton;
        
        private List<FramChat> ChatRecivedList = new List<FramChat>();
        private SoundEffectControll soundEffect;
        private Sticker tempsticker;

        [SerializeField] private bool ondrag;
        public bool OnDrag
        {
            set { ondrag = value; }
            get { return ondrag; }
        }
        private bool init = false;
        private void OnEnable()
        {
           
            tempsticker = new Sticker();

            soundEffect = GetComponent<SoundEffectControll>();
            if (ShowStickerPanel_Button)
                ShowStickerPanel_Button.onClick.AddListener(() => {
                    if (StickerPanel_gameobject.activeSelf)
                        StickerPanel_gameobject.SetActive(false);
                    else
                        StickerPanel_gameobject.SetActive(true);
                });
        }
        private void OnDisable()
        {
            tempsticker = null;
            Server.OnChatsRecive -= Server_OnChatsRecive;
            SendButton.onClick.RemoveAllListeners();
            ShowStickerPanel_Button.onClick.RemoveAllListeners();
            AvatarReciver.sprite = null;

            UserNameReciver.text = "";
            Cup.text = "";
          
            clearChatList();

    }
        public void init_chatbox()
        {
            Server.OnChatsRecive += Server_OnChatsRecive;

            SendButton.onClick.AddListener(() =>
            {
                SendChat();

            });
            Server.SendRequestGetAllChat(IDReciver);
            //Debug.Log("ChatWithID:" + IDReciver);
        }
        private void Server_OnChatsRecive(Diaco.HTTPBody.Chats chats)
        {
            ChatRecive(chats);
            soundEffect.PlaySound(0);
        }

        public void SendChat()
        {
            if (InputMessage.text != "")
            {
              //var chat =  PersianFix.Persian.Fix(InputMessage.text, 255);
            
                Server.SendChatToUser(IDReciver, InputMessage.text,false);
                InputMessage.text = "";
            }
        }
        public void SendSticekr(string namesticker)
        {
            Server.SendChatToUser(IDReciver, namesticker,true);
            
        }
        public void ChatRecive(Diaco.HTTPBody.Chats chats)
        {
            clearChatList();
            if (IDReciver == chats.chatId)
            {
                for (int i = 0; i < chats.chats.Count; i++)
                {
                    if (chats.chats[i].type == 0)//FrameMyChat
                    {
                       
                        if (!chats.chats[i].isSticker)///with out sticker
                        {
                            var frame = Instantiate(FrameMyChat, ContentChats);
                            frame.FillFrameChatWithOutAvatar(chats.chats[i].text, chats.chats[i].time, chats.chats[i].date, chats.chats[i].read);
                            ChatRecivedList.Add(frame);
                          //  Debug.Log("C"+chats.chats[i].text);
                        }
                        else///with  sticker
                        {
                            var frame = Instantiate(FrameMyChatWithSticker, ContentChats);
                            var sticker = SelectSticker(chats.chats[i].text);
                            frame.FillFrameChatWithStickerAndWithOutAvatar(sticker, chats.chats[i].time, chats.chats[i].date, chats.chats[i].read);
                            ChatRecivedList.Add(frame);
                          //  Debug.Log("S"+sticker);
                        }
                      //  Debug.Log(chats.chats[i].read);
                    }
                    else//FrameYouChat
                    {
                        
                        if (!chats.chats[i].isSticker)///with out sticker
                        {
                            var frame = Instantiate(FrameYouChat, ContentChats);
                            frame.FillFrameChatWithOutAvatar(chats.chats[i].text, chats.chats[i].time, chats.chats[i].date, chats.chats[i].read);
                            ChatRecivedList.Add(frame);
                          //  Debug.Log("C" + chats.chats[i].text);
                        }
                        else///with  sticker
                        {
                            var frame = Instantiate(FrameYouChatWithSticker, ContentChats);
                            var sticker = SelectSticker(chats.chats[i].text);
                            frame.FillFrameChatWithStickerAndWithOutAvatar(sticker, chats.chats[i].time, chats.chats[i].date, chats.chats[i].read);
                            ChatRecivedList.Add(frame);
                         //   Debug.Log("S" + sticker);
                        }
                    }
                    
                }
                Server.SendReadChat(IDReciver);
            }
            SetScrollDown();
        }
        public void SetElementPage(ChatBoxData data)
        {
            AvatarReciver.sprite = Server.AvatarContainer.LoadImage(data.avatar);
            UserNameReciver.text = data.username;
            IDReciver = data.id;
            Cup.text = data.cup;
        }
        private Sticker SelectSticker(string stickername)
        {
          //  var name = stickername.Remove(0,2);
           
         //  Debug.Log("TRIiM##"+name);
            for (int i = 0; i < stickers.Count; i++)
            {
                
                if (stickers[i].stickerName == name)
                {
                    tempsticker = stickers[i];
                    //Debug.Log("SSSSSSSSS");
                }
            }
            return tempsticker;
        }
        private void SetScrollDown()
        {
            if (!OnDrag)
                ChatScroll.value =0 ;
        }
        private void  clearChatList()
        {
            if (ChatRecivedList.Count > 0)
            {
                for (int i = 0; i < ChatRecivedList.Count; i++)
                {
                    Destroy(ChatRecivedList[i].gameObject);
                }
                ChatRecivedList.Clear();
            }
        }
    }
    [Serializable]
    public struct ChatBoxData
    {
        public string avatar;
        public string username;
        public string id;
        public string cup;
    }
}