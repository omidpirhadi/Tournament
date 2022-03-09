using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardFriendTypeTwo : MonoBehaviour
{
    public bool IsOnline = false;
    public bool IsSendReq = false;

    public Image img_IsOnline;
    public NavigationUI NavigationUi;
    public ServerUI Server;
    public Button OpenProfilePopupButton;
    public Image Avatar;
    public Text UserName;
    public string ID ;
    public Text Cup;
    public Button btn_Add;
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
