using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
namespace Diaco.UI.SocialTabs
{
    public class FriendTabCard : MonoBehaviour
    {
        public bool IsOnline = false;
        public bool IsSendReq = false;

        public Image img_IsOnline;
        public NavigationUI NavigationUi;
        public ServerUI Server;
        
        public Image Avatar;
        public Text UserName;
        public string ID;
        public Text Cup;
        public Button OpenProfilePopupButton;
        public Button btn_Add;
        public Button btn_sendmessage;
        private void OnEnable()
        {
            NavigationUi = FindObjectOfType<NavigationUI>();
            Server = FindObjectOfType<ServerUI>();
            OpenProfilePopupButton.onClick.AddListener(() =>
            {
                NavigationUi.ShowPopUp("profilefromteam");
                Server.GetProfilePerson(UserName.text);
            });
        }
        private void OnDisable()
        {
            OpenProfilePopupButton.onClick.RemoveAllListeners();
        }
        public void SetForSearchedFriend(SearchUser data)
        {
            ID = data.id;
            UserName.text = data.userName;
            Avatar.sprite = Server.AvatarContainer.LoadImage(data.avatar);
            Cup.text = data.cup.ToString();
            if (data.isOnline)
                img_IsOnline.enabled = true;
            else
                img_IsOnline.enabled = false;
            if (data.friend == 0)///not friend
            {
                btn_sendmessage.gameObject.SetActive(false);
                btn_Add.gameObject.SetActive(true);

                btn_Add.interactable = true;
                btn_Add.onClick.AddListener(onclick_btn_Add);
            }
            else if(data.friend == 1)/// req  for friend sended
            {
                btn_sendmessage.gameObject.SetActive(false);
                btn_Add.gameObject.SetActive(true);

                btn_Add.interactable = false;
            }
          
            else if(data.friend == 2)
            {
                btn_Add.gameObject.SetActive(false);
                btn_sendmessage.gameObject.SetActive(true);

                btn_sendmessage.interactable = true;
                btn_sendmessage.onClick.AddListener(onclick_btn_sendmessage);
            }

        }
        public void SetForFriend(FriendBody data)
        {

            btn_Add.gameObject.SetActive(false);
            btn_sendmessage.gameObject.SetActive(true);

            btn_sendmessage.interactable = true;
            btn_sendmessage.onClick.AddListener(onclick_btn_sendmessage);

            ID = data.id;
            UserName.text = data.userName;
            Avatar.sprite = Server.AvatarContainer.LoadImage(data.avatar);
            Cup.text = data.cup.ToString();
            if (data.isOnline)
            {
                img_IsOnline.enabled = true;
                //    Debug.Log("FriendOnline2");
            }
            else
            {
                img_IsOnline.enabled = false;
                //     Debug.Log("Friendoffline");
            }

        }
        private void onclick_btn_Add()
        {
            Server.RequsetAddFriend(ID);
            btn_Add.interactable = false;
        }
        private void onclick_btn_sendmessage()
        {
            Server.SendRequestOpenChatBox(ID);

        }
    }
}