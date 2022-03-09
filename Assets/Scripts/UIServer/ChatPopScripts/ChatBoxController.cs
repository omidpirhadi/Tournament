using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Chat
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
        private void Awake()
        {
            Server.OnChatsRecive += Server_OnChatsRecive;

            SendButton.onClick.AddListener(() =>
            {
                SendChat();
            });
        }
        private void OnEnable()
        {

            Debug.Log(UserNameReciver.text);
            Server.SendRequestChatWithUser(UserNameReciver.text);

          
        }
        private void OnDisable()
        {
            AvatarReciver.sprite = null;

            UserNameReciver.text = "";
            Cup.text = "";
            Server.SendCurrentPage("", "");
            clearChatList();
           // Server.OnChatsRecive -= Server_OnChatsRecive;
         //   SendButton.onClick.RemoveAllListeners();
    }
        private void Server_OnChatsRecive(Diaco.HTTPBody.Chats chats)
        {
            ChatRecive(chats);
        }

        public void SendChat()
        {
            if (InputMessage.text != "")
            {
              var chat =  PersianFix.Persian.Fix(InputMessage.text, 0);
            
                Server.SendChatToUser(UserNameReciver.text, chat);
                InputMessage.text = "";
            }
        }
        public void ChatRecive(Diaco.HTTPBody.Chats chats)
        {
              clearChatList();
           
            for (int i = 0; i < chats.chats.Count; i++)
            {
                if(chats.chats[i].type == 0)//FrameMyChat
                {
                    var frame = Instantiate(FrameMyChat, ContentChats);
                    frame.FillFrameChat(chats.chats[i].text, chats.chats[i].time, chats.chats[i].date, chats.chats[i].read);
                    ChatRecivedList.Add(frame);
                }
                else//FrameYouChat
                {
                    var frame = Instantiate(FrameYouChat, ContentChats);
                    frame.FillFrameChat(chats.chats[i].text, chats.chats[i].time ,chats.chats[i].date, chats.chats[i].read);
                    ChatRecivedList.Add(frame);
                }
            }
        }
        public void SetElementPage(Sprite avatar, string username, string id  , string cup)
        {
            AvatarReciver.sprite = avatar;
            UserNameReciver.text = username;
            IDReciver = id;
            Cup.text = cup;
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
}