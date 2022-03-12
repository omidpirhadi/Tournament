using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
namespace Diaco.Chat
{


    public class TeamChatsController : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationui;
        public Text TeamName;
        public Button TeamInfoButton;
        public Image TeamImage;
        public Button SendMessageButton;
        public InputField InputMessage;
        public RectTransform ContentChats;

        public Sprite AssetsSoccer;
        public Sprite AssetBiliard;
        public Button TabTeamChat_button;
        public Diaco.UI.Chatbox.FramChat FrameMyChat;
        public Diaco.UI.Chatbox.FramChat FrameYouChat;
        private List<Diaco.UI.Chatbox.FramChat> ChatRecivedList = new List<Diaco.UI.Chatbox.FramChat>();
        private void Awake()
        {
            
        }
        public void OnEnable()
        {
            Server.OnUpdateChatTeam += Server_OnUpdateChatTeam;
            SendMessageButton.onClick.AddListener(() => { SendChat(); });
            TeamInfoButton.onClick.AddListener(() => { Server.GetLeagueInfo(Server.BODY.social.team.teamId); });
            TabTeamChat_button.onClick.AddListener(() => { Server.SendChatToTeam(""); });
            initializPage();
            UpadteChats(Server.BODY.social.team.chats);
        }

        private void OnDisable()
        {
            TeamName.text = "";
            TeamImage.sprite = null;
            Server.OnUpdateChatTeam -= Server_OnUpdateChatTeam;
            SendMessageButton.onClick.RemoveAllListeners();
            TeamInfoButton.onClick.RemoveAllListeners();
            TabTeamChat_button.onClick.RemoveAllListeners();
            clearChatList();
        }

        private void Server_OnUpdateChatTeam()
        {
            initializPage();
            UpadteChats(Server.BODY.social.team.chats);
            
        }
        public void initializPage()
        {
            TeamName.text = Server.BODY.social.team.name;
            TeamInfoButton.GetComponent<Image>().sprite = Server.LeagueFlagsContainer.LoadImage(Server.BODY.social.team.avatar);
            if (Server.BODY.social.team.game == 0)
            {
                TeamImage.sprite = AssetBiliard;
            }
            else
            {
                TeamImage.sprite = AssetsSoccer;
            }
           
            
            UpadteChats(Server.BODY.social.team.chats);
        }
        public void UpadteChats(List<ChatBodyTeam> chats)
        {
            clearChatList();

            for (int i = 0; i < chats.Count; i++)
            {
                if (chats[i].userName == Server.BODY.userName)//FrameMyChat
                {
                    var frame = Instantiate(FrameMyChat, ContentChats);
                    var avatar = Server.AvatarContainer.LoadImage(chats[i].avatar);
                    frame.FillFrameChat(chats[i].userName, avatar, chats[i].text, chats[i].time, chats[i].date);
                    ChatRecivedList.Add(frame);
                }
                else//FrameYouChat
                {
                    var frame = Instantiate(FrameYouChat, ContentChats);
                    var avatar = Server.AvatarContainer.LoadImage(chats[i].avatar);
                    frame.FillFrameChat(chats[i].userName, avatar, chats[i].text, chats[i].time, chats[i].date);
                    ChatRecivedList.Add(frame);
                }
            }
        }
        public void SendChat()
        {
            if (InputMessage.text != "")
            {
                var chat = PersianFix.Persian.Fix(InputMessage.text, 255);
                Server.SendChatToTeam(chat);
                InputMessage.text = "";
            }
        }
        private void clearChatList()
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