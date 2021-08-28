using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class DialogDeleteMessage : MonoBehaviour
{
    public ServerUI Server;
    public Text Context;
    public Button YESButton;
    public Button NoButton;
    public enum DeleteMessagesType {none, Chat, RequestFriend, TeamInvite};
    public DeleteMessagesType messagesType;
    public string EmitUserOrTeamName = "";
    public void Awake()
    {
        Server = FindObjectOfType<ServerUI>();
        YESButton.onClick.AddListener(() => { DeleteData(); });
        NoButton.onClick.AddListener(() => {  CloseDialog();


        });
    }

    public void OnDisable()
    {
        // YESButton.onClick.RemoveAllListeners();
        // NoButton.onClick.RemoveAllListeners();
    }
    private void DeleteData()
    {
        if(messagesType  == DeleteMessagesType.Chat)
        {
            Server.RejectRequest("chat", EmitUserOrTeamName);
            messagesType = DeleteMessagesType.none;
            EmitUserOrTeamName = "";
            this.gameObject.SetActive(false);


        }
        else if(messagesType == DeleteMessagesType.RequestFriend)
        {
            Server.RejectRequest("friend", EmitUserOrTeamName);
            messagesType = DeleteMessagesType.none;
            EmitUserOrTeamName = "";
            this.gameObject.SetActive(false);
        }
        else if(messagesType == DeleteMessagesType.TeamInvite)
        {

        }
    }
    public void ShowDialog(string Message)
    {

        Context.text = Message;
        this.gameObject.SetActive(true);
    }
    public void ShowDialog()
    {
        this.gameObject.SetActive(true);
    }
    public void CloseDialog()
    {
        messagesType = DeleteMessagesType.none;
        EmitUserOrTeamName = "";
        this.gameObject.SetActive(false);

    }
 
}
