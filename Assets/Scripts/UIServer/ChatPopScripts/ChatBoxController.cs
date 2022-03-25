using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.Chatbox
{
    public class ChatBoxController : MonoBehaviour
    {
        public ServerUI Server;
        public string IDReciver = "";
        public Image AvatarReciver;
        public Text UserNameReciver;
        public Text Cup;
        public RectTransform ContentChats;
        public FramChat FrameMyChat;
        public FramChat FrameYouChat;
        public InputField InputMessage;
        public Button SendButton;
        
        private List<FramChat> ChatRecivedList = new List<FramChat>();
        private SoundEffectControll soundEffect;
        private void OnEnable()
        {

            soundEffect = GetComponent<SoundEffectControll>();
          
        }
        private void OnDisable()
        {
            Server.OnChatsRecive -= Server_OnChatsRecive;
            SendButton.onClick.RemoveAllListeners();
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
              var chat =  PersianFix.Persian.Fix(InputMessage.text, 0);
            
                Server.SendChatToUser(IDReciver, chat);
                InputMessage.text = "";
            }
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
                        var frame = Instantiate(FrameMyChat, ContentChats);
                        frame.FillFrameChat(chats.chats[i].text, chats.chats[i].time, chats.chats[i].date, chats.chats[i].read);
                        ChatRecivedList.Add(frame);
                        Debug.Log(chats.chats[i].read);
                    }
                    else//FrameYouChat
                    {
                        var frame = Instantiate(FrameYouChat, ContentChats);
                        frame.FillFrameChat(chats.chats[i].text, chats.chats[i].time, chats.chats[i].date, chats.chats[i].read);
                        ChatRecivedList.Add(frame);
                    }
                    
                }
                Server.SendReadChat(IDReciver);
            }
        }
        public void SetElementPage(ChatBoxData data)
        {
            AvatarReciver.sprite = Server.AvatarContainer.LoadImage(data.avatar);
            UserNameReciver.text = data.username;
            IDReciver = data.id;
            Cup.text = data.cup;
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