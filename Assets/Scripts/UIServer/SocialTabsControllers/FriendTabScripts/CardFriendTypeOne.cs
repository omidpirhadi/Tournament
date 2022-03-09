using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardFriendTypeOne : MonoBehaviour
{
    public ServerUI Server;
    public NavigationUI NavigationUi;

    public Button OpenProfilePopupButton;

    public bool IsOnline = false;
    public Image img_IsOnline;
    public string ID = "";
    public Image Avatar;
    public Text UserName;
    public Text Cup;
    public Button btn_chat;

    private void OnEnable()
    {
        NavigationUi = FindObjectOfType<NavigationUI>();
        Server = FindObjectOfType<ServerUI>();
        OpenProfilePopupButton.onClick.AddListener(() => {
            
            

            NavigationUi.ShowPopUp("profilefromteam");
            Server.GetProfilePerson(UserName.text);
        });
    }
    private void OnDisable()
    {
        OpenProfilePopupButton.onClick.RemoveAllListeners();
    }

}
