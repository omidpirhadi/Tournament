using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Diaco.HTTPBody;
namespace Diaco.Chat
{


    public class TeamChatsController : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationui;
        public List<Sticker> stickers;

        public Text TeamName;
        public Button TeamInfoButton;
        public Image TeamImage;
        public Button SendMessageButton;
        public TMPro.TMP_InputField InputMessage;

        public GameObject StickerPanel_gameobject;
        public Button ShowStickerPanel_Button;

        public Scrollbar ChatScroll;
        public RectTransform ContentChats;

        public Sprite AssetsSoccer;
        public Sprite AssetBiliard;
        public Button TabTeamChat_button;

        public Diaco.UI.Chatbox.FramChat FrameMyChat;
        public Diaco.UI.Chatbox.FramChat FrameYouChat;

        public Diaco.UI.Chatbox.FramChat FrameMyChatWithSticker;
        public Diaco.UI.Chatbox.FramChat FrameYouChatWithSticker;

        private List<Diaco.UI.Chatbox.FramChat> ChatRecivedList = new List<Diaco.UI.Chatbox.FramChat>();
        private SoundEffectControll soundEffect;
        private Sticker tempsticker;
        [SerializeField] private bool ondrag;
        public bool OnDrag
        {
            set { ondrag = value; }
            get { return ondrag; }
        }
        private bool init = false;
        public void OnEnable()
        {
            tempsticker = new Sticker();
            Server.OnUpdateChatTeam += Server_OnUpdateChatTeam;
            SendMessageButton.onClick.AddListener(() => { SendChat(); });
            TeamInfoButton.onClick.AddListener(() => { Server.GetLeagueInfo(Server.BODY.social.team.teamId); });
            TabTeamChat_button.onClick.AddListener(() => { Server.SendChatToTeam(""); });



            soundEffect = GetComponent<SoundEffectControll>();
            if (ShowStickerPanel_Button)
                ShowStickerPanel_Button.onClick.AddListener(() => {
                    if (StickerPanel_gameobject.activeSelf)
                        StickerPanel_gameobject.SetActive(false);
                    else
                        StickerPanel_gameobject.SetActive(true);
                });

            initializPage();
            ///   UpadteChats(Server.BODY.social.team.chats);
        }

        private void OnDisable()
        {
            TeamName.text = "";
            TeamImage.sprite = null;
            Server.OnUpdateChatTeam -= Server_OnUpdateChatTeam;
            SendMessageButton.onClick.RemoveAllListeners();
            TeamInfoButton.onClick.RemoveAllListeners();
            TabTeamChat_button.onClick.RemoveAllListeners();
            ShowStickerPanel_Button.onClick.RemoveAllListeners();
            tempsticker = null;
            init = false;
            clearChatList();
        }

        private void Server_OnUpdateChatTeam()
        {
            initializPage();
            /// UpadteChats(Server.BODY.social.team.chats);
            soundEffect.PlaySound(0);
        }
        public void initializPage()
        {
            if (!init)
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

            }
            UpadteChats(Server.BODY.social.team.chats);
            SetScrollDown();
        }
        public void UpadteChats(List<ChatBodyTeam> chats)
        {
            clearChatList();

            for (int i = 0; i < chats.Count; i++)
            {

                if (chats[i].userName == Server.BODY.userName)//FrameMyChat
                {
                    if (!chats[i].text.Contains("##"))///with out sticker
                    {
                        var frame = Instantiate(FrameMyChat, ContentChats);
                        var avatar = Server.AvatarContainer.LoadImage(chats[i].avatar);
                        frame.FillFrameChatWithAvatar(chats[i].userName, avatar, chats[i].text, chats[i].time, chats[i].date);
                        ChatRecivedList.Add(frame);
                    }
                    else //with sticker
                    {
                        var frame = Instantiate(FrameMyChatWithSticker, ContentChats);
                        var avatar = Server.AvatarContainer.LoadImage(chats[i].avatar);
                        Debug.Log(chats[i].text);
                        var sticker = SelectSticker(chats[i].text);
                        Debug.Log(sticker);

                        frame.FillFrameChatWithStickerAndWithAvatar(chats[i].userName, avatar, sticker, chats[i].time, chats[i].date);
                        ChatRecivedList.Add(frame);
                    }
                }
                else//FrameYouChat
                {
                    if (!chats[i].text.Contains("##"))///with out sticker
                    {
                        var frame = Instantiate(FrameYouChat, ContentChats);
                        var avatar = Server.AvatarContainer.LoadImage(chats[i].avatar);
                        frame.FillFrameChatWithAvatar(chats[i].userName, avatar, chats[i].text, chats[i].time, chats[i].date);
                        ChatRecivedList.Add(frame);
                    }
                    else///with  sticker
                    {
                        var frame = Instantiate(FrameYouChatWithSticker, ContentChats);
                        var avatar = Server.AvatarContainer.LoadImage(chats[i].avatar);
                        var sticker = SelectSticker(chats[i].text);
                        frame.FillFrameChatWithStickerAndWithAvatar(chats[i].userName, avatar, sticker, chats[i].time, chats[i].date);
                        ChatRecivedList.Add(frame);
                    }
                }


            }
        }
        public void SendChat()
        {
            if (InputMessage.text != "")
            {
                //var chat = PersianFix.Persian.Fix(InputMessage.text, 255);
                Server.SendChatToTeam(InputMessage.text);
                InputMessage.text = "";
            }
        }
        private void SetScrollDown()
        {
            if (!OnDrag)
                ChatScroll.value = 0;
        }
        public void SendSticekr(string namesticker)
        {
            Server.SendChatToTeam(namesticker);

        }
        private Sticker SelectSticker(string stickername)
        {
            var name = stickername.Remove(0, 2);

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