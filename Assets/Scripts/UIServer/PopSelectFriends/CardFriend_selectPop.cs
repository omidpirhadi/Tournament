using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Social.PopSelectFriendCard
{
    public class CardFriend_selectPop : MonoBehaviour
    {

        public Image Profile;
        public Text Username;
        public Text Cup;
        public Toggle Checkbox;
        public Diaco.Social.SelectFriendsPopup SelectFriendPopController;
        private void Start()
        {
            Checkbox.onValueChanged.AddListener((x) => {
                if(x)
                {
                    var num = Convert.ToInt16(SelectFriendPopController.SelectedFriendIndicator.text);

                    num++;
                    SelectFriendPopController.SelectedFriendIndicator.text = Mathf.Clamp(num, 0, 99).ToString();
                    SelectFriendPopController.FriendsSelectedList.Add(Username.text);
                }
                else
                {
                    var num = Convert.ToInt16(SelectFriendPopController.SelectedFriendIndicator.text);
                    num--;
                    SelectFriendPopController.SelectedFriendIndicator.text= Mathf.Clamp(num, 0, 99).ToString();
                    SelectFriendPopController.FriendsSelectedList.Remove(Username.text);

                }
            });
        }
        public void SetCard(Sprite profile, string username, string cup)
        {
            Profile.sprite = profile;
            Username.text = username;
            Cup.text = cup;
        }
    }
}