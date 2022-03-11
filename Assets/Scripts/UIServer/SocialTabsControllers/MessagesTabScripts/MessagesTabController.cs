using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Diaco.Social.MessageCards;
using Sirenix.OdinInspector;
namespace Diaco.Social
{
    public class MessagesTabController : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationui;
        public DialogDeleteMessage DialogDelete;
        private enum TypeCardMessage { ChatRequest , FriendRequest, TeamInvitRequset}
       // public Diaco.UI.Chatbox.ChatBoxController ChatBox;
        [FoldoutGroup("MessageTabElements")]
        public Text TotalMessageIndicator;
        [FoldoutGroup("MessageTabElements")]
        public Text TotalFriendRequestIndicator;
        [FoldoutGroup("MessageTabElements")]
        public Text TotalInviteTeamIndicator;
        [FoldoutGroup("MessageTabElements")]
        public Text TotalChatIndicator;
        [FoldoutGroup("MessageTabElements")]
        public Button FilterByChat;
        [FoldoutGroup("MessageTabElements")]
        public Button FilterByTeamRequest;
        [FoldoutGroup("MessageTabElements")]
        public Button FilterByFriendRequest;
        [FoldoutGroup("MessageTabElements")]
        public RectTransform Content;

        [FoldoutGroup("CardAssetsForFillList")]
        public MessagesTabCardHaveChat IndicatorMassagePrefab;
     [SerializeField]   private List<MessagesTabCardHaveChat> temp_chatcard_list;
        [FoldoutGroup("CardAssetsForFillList")]
        public MessagesTabCardInviteToTeam IndicatorInviteToTeamPrefab;
       [SerializeField] private List<MessagesTabCardInviteToTeam> temp_invitecard_list;
        [FoldoutGroup("CardAssetsForFillList")]
        public MessagesTabCardReqFriend IndicatorRequestFriend;
       [SerializeField] private List<MessagesTabCardReqFriend> temp_friendcard_list;

        private void OnEnable()
        {
            temp_friendcard_list = new List<MessagesTabCardReqFriend>();
            temp_invitecard_list = new List<MessagesTabCardInviteToTeam>();
            temp_chatcard_list = new List<MessagesTabCardHaveChat>();
            SetIndicatorTotalCountMessages(Server.BODY.social.inRequests);
            SetIndicatorCountChat(0);
            SetIndicatorCountFriendRequset(0);
            SetIndicatorCountInviteTeamRequest(0);
            Server.OnGetMessages += Server_OnGetMessages;
            FilterByChat.onClick.AddListener(() => { FilterByType(TypeCardMessage.ChatRequest);  });
            FilterByFriendRequest.onClick.AddListener(() => { FilterByType(TypeCardMessage.FriendRequest); });
            FilterByTeamRequest.onClick.AddListener(() => { FilterByType(TypeCardMessage.TeamInvitRequset); });
        }
        private void OnDisable()
        {
            ClearMessageTab();
           Server.OnGetMessages -= Server_OnGetMessages;
           
           FilterByChat.onClick.RemoveAllListeners();
           FilterByFriendRequest.onClick.RemoveAllListeners();
           FilterByTeamRequest.onClick.RemoveAllListeners();
            
        }
        private void Server_OnGetMessages(HTTPBody.InRequsets reqs)
        {
            SetIndicatorCountChat(0);
            SetIndicatorCountFriendRequset(0);
            SetIndicatorCountInviteTeamRequest(0);
            MessageInitializ(reqs);
            SetIndicatorTotalCountMessages(Server.BODY.social.inRequests);
            Debug.Log(reqs.inRequests.Count);
        }

  
        public void GetMessages()
        {
            Server.GetMessages();
        }
      
        public void MessageInitializ(Diaco.HTTPBody.InRequsets requsets)
        {
            ClearMessageTab();
            var list_req = requsets.inRequests;
            for (int i = 0; i < list_req.Count; i++)
            {
               // Debug.Log("Initialze Messages");
                if (list_req[i].type == "chat")
                {
                    var card = Instantiate(IndicatorMassagePrefab, Content);
                    card.UserID = list_req[i].id;
                    var image = Server.AvatarContainer.LoadImage(list_req[i].avatar);
                   card.cup = list_req[i].cup.ToString();
                    ///Debug.Log("card.cup::::::::::::::"+ list_req[i].cup.ToString());
                    card.SetCard(list_req[i].isOnline, image, list_req[i].from, list_req[i].messageCount,
                        () => {
                            //Server.SendCurrentPage("chat", card.ID);
                            //  ChatBox.SetElementPage(image, card.UserNameIndicator.text,card.ID, card.cup);
                            //  navigationui.ShowPopUp("chat");
                            Server.SendRequestOpenChatBox(card.UserID);
                        },
                        () => {
                            //Server.RejectRequest("chat", card.UserNameIndicator.text);
                            DialogDelete.EmitUserOrTeamName = card.UserID;
                            DialogDelete.messagesType = DialogDeleteMessage.DeleteMessagesType.Chat;
                            DialogDelete.ShowDialog();
                        }
                        );
                    temp_chatcard_list.Add(card);
                    SetIndicatorCountChat(1);
                }
                else if (list_req[i].type == "friend")
                {
                    var card = Instantiate(IndicatorRequestFriend, Content);
                    card.UserID = list_req[i].id;
                    var image = Server.AvatarContainer.LoadImage(list_req[i].avatar);
                    card.SetCard(image, list_req[i].from, list_req[i].cup.ToString(), list_req[i].isOnline,
                        () => { Server.AcceptRequest("friend", card.UserID); }, 
                        () => {
                           // Server.RejectRequest("friend", card.UserNameIndicator.text);
                            DialogDelete.EmitUserOrTeamName = card.UserID;
                            DialogDelete.messagesType = DialogDeleteMessage.DeleteMessagesType.RequestFriend;
                            DialogDelete.ShowDialog();
                        });
                    temp_friendcard_list.Add(card);
                    SetIndicatorCountFriendRequset(1);
                }
                else if (list_req[i].type == "team")
                {
                    var card = Instantiate(IndicatorInviteToTeamPrefab, Content);
                    var image = Server.LeagueFlagsContainer.LoadImage(list_req[i].avatar);
                    card.SetCard(
                        (Diaco.Social.MessageCards.MessagesTabCardInviteToTeam.TypeGame)list_req[i].game,
                        image,
                        list_req[i].from,
                        list_req[i].capacity,
                        list_req[i].remainingTime,
                        (Diaco.Social.MessageCards.MessagesTabCardInviteToTeam.TypeCost)list_req[i].costType,
                        list_req[i].cost.ToString(),
                        list_req[i].teamId,
                        () => {

                            navigationui.ShowPopUp("teaminfo"); Server.GetTeamInfo(card.TeamID);
                        }
                        );
                   
                    temp_invitecard_list.Add(card);
                    SetIndicatorCountInviteTeamRequest(1);
                }
            }
        }
        private void ClearMessageTab()
        {


            for (int i = 0; i < temp_friendcard_list.Count; i++)
            {
                Destroy(temp_friendcard_list[i].gameObject);

             
            }
            for (int i = 0; i < temp_invitecard_list.Count; i++)
            {

                Destroy(temp_invitecard_list[i].gameObject);
                
            }
            for (int i = 0; i < temp_chatcard_list.Count; i++)
            {

                Destroy(temp_chatcard_list[i].gameObject);

            }
            temp_friendcard_list.Clear();
            temp_chatcard_list.Clear();
            temp_invitecard_list.Clear();

        }
        private void FilterByType(TypeCardMessage type)
        {
            if (type == TypeCardMessage.ChatRequest)
            {
                for (int i = 0; i < temp_chatcard_list.Count; i++)
                {
                    temp_chatcard_list[i].gameObject.SetActive(true);
                }
                for (int i = 0; i < temp_invitecard_list.Count; i++)
                {
                    temp_invitecard_list[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < temp_friendcard_list.Count; i++)
                {
                    temp_friendcard_list[i].gameObject.SetActive(false);
                }
            }
            else if (type == TypeCardMessage.FriendRequest)
            {
                for (int i = 0; i < temp_chatcard_list.Count; i++)
                {
                    temp_chatcard_list[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < temp_invitecard_list.Count; i++)
                {
                    temp_invitecard_list[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < temp_friendcard_list.Count; i++)
                {
                    temp_friendcard_list[i].gameObject.SetActive(true);
                }
            }
            else if (type == TypeCardMessage.TeamInvitRequset)
            {
                for (int i = 0; i < temp_chatcard_list.Count; i++)
                {
                    temp_chatcard_list[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < temp_invitecard_list.Count; i++)
                {
                    temp_invitecard_list[i].gameObject.SetActive(true);
                }
                for (int i = 0; i < temp_friendcard_list.Count; i++)
                {
                    temp_friendcard_list[i].gameObject.SetActive(false);
                }
            }
        }
        private void SetIndicatorTotalCountMessages(int count)
        {
            if (count == 0)
            {
                //TotalMessageIndicator.enabled = false;
                TotalMessageIndicator.transform.parent.gameObject.SetActive(false);
            }
            else
            {

                TotalMessageIndicator.transform.parent.gameObject.SetActive(true);
                TotalMessageIndicator.text = count.ToString();
                //Debug.Log("SET:TOTAL MESSAGE Indicator.");
            }
        }
        private void SetIndicatorCountChat(int count)
        {
            if (count == 0)
            {
                //TotalMessageIndicator.enabled = false;
                TotalChatIndicator.text = "0";
                TotalChatIndicator.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                TotalChatIndicator.transform.parent.gameObject.SetActive(true);
                var t = System.Convert.ToInt16(TotalChatIndicator.text);
                t++;
                TotalChatIndicator.text = (t).ToString();
            }
           
        }
        private void SetIndicatorCountFriendRequset(int count)
        {
            if (count == 0)
            {
                TotalFriendRequestIndicator.text = "0";
                TotalFriendRequestIndicator.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                TotalFriendRequestIndicator.transform.parent.gameObject.SetActive(true);
                var t = System.Convert.ToInt16(TotalFriendRequestIndicator.text);
                t++;
                TotalFriendRequestIndicator.text = (t ).ToString();
            }
        }
        private void SetIndicatorCountInviteTeamRequest(int count)
        {
            if(count == 0)
            {
                TotalInviteTeamIndicator.text = "0";
                TotalInviteTeamIndicator.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                TotalInviteTeamIndicator.transform.parent.gameObject.SetActive(true);
                var t = System.Convert.ToInt16(TotalInviteTeamIndicator.text);
                t++;
                TotalInviteTeamIndicator.text = (t).ToString();
            }
           
        }
    }
}
