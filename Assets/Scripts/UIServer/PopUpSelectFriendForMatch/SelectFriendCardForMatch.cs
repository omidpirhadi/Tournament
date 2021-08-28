using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.SelectFriendForMatchs
{
    public class SelectFriendCardForMatch : MonoBehaviour
    {

        public Image Profile;
        public Text Username;
        public Text TotalResult;
        //public Text Cup;
       /// public Toggle Checkbox;
        public Button SendRequest_Button;
        
        public void SetCard(Sprite profile, string username, string totalresualt , Action SetSendRequestButton)
        {
            Profile.sprite = profile;
            Username.text = username;
            TotalResult.text = totalresualt;
            SendRequest_Button.onClick.AddListener(() => {
                SetSendRequestButton();
            });
        }
    }
}