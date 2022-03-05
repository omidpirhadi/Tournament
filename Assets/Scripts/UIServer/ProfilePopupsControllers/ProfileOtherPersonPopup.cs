using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Profile
{
    public class ProfileOtherPersonPopup : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationUi;

        public Diaco.Chat.ChatBoxController ChatBoxFromProfile;

        public Button AddFriendButton;
        public Button BlockPersonButton;
        public Button MessageButton;


        public Image Avatar;
        public Text Username;
        public Text Cup;
        public Image RankLevel;
        public Text Description;
        /// <summary>
        /// /soocer state
        /// </summary>
        public Text S_WinCount;
        public Text S_WinRate;
        public Text S_PurpleCardCount;
        public Text S_BlueCardCount;
        public Text S_GreenCardCount;
        public Text S_YellowCardCount;
        /// <summary>
        /// biliard
        /// </summary>
        public Text B_WinCount;
        public Text B_WinRate;
        public Text B_PurpleCardCount;
        public Text B_BlueCardCount;
        public Text B_GreenCardCount;
        public Text B_YellowCardCount;

        public List<Image> Achivments;

        
        private void Awake()
        {
            Server.OnGetProfileOtherPerson += Server_OnGetProfileOtherPerson;
             AddFriendButton.onClick.AddListener(() => {
                  Server.RequsetAddFriend(Username.text);
                  AddFriendButton.gameObject.SetActive(false);
              });
            BlockPersonButton.onClick.AddListener(() =>
                   {
                       Server.RequestBlockUser(Username.text);
                   });
              MessageButton.onClick.AddListener(() =>
                 {
                     navigationUi.LastProfileChecked = Username.text;
                     navigationUi.ShowPopUp("teamprofilechat");

                 });
        }
        private void OnEnable()
        {

           

            // InitializeProfile();
        }
        private void OnDisable()
        {
          //  Server.OnGetProfileOtherPerson -= Server_OnGetProfileOtherPerson;
         //   MessageButton.onClick.RemoveAllListeners();
         //   BlockPersonButton.onClick.RemoveAllListeners();
         //   AddFriendButton.onClick.RemoveAllListeners();
        }
        private void Server_OnGetProfileOtherPerson(HTTPBody.ProfileOtherPerson profile)
        {
            AddFriendButton.onClick.RemoveAllListeners();
            MessageButton.onClick.RemoveAllListeners();
            BlockPersonButton.onClick.RemoveAllListeners();
            InitializeProfile(profile);
        }

        private void InitializeProfile(HTTPBody.ProfileOtherPerson profile)
        {


            Avatar.sprite = Server.AvatarContainer.LoadImage(profile.profile.avatar);

            Username.text = profile.userName;
            Cup.text = (profile.profile.soccer_cup).ToString();
            RankLevel.sprite = Server.AvatarContainer.LoadImage(profile.profile.avatar);
            Description.text = profile.profile.description;

            S_WinCount.text = (profile.profile.soccer.win).ToString() + "/" + (profile.profile.soccer.total).ToString();
            try
            {
                S_WinRate.text = ((profile.profile.soccer.win / profile.profile.soccer.total) * 100.00f).ToString();
            }
            catch (DivideByZeroException e)
            {
                Debug.Log(e.Data.ToString());
            }
            finally
            {
                S_WinRate.text = "0%";
            }


            S_PurpleCardCount.text = (profile.profile.soccer.purple).ToString();
            S_BlueCardCount.text = (profile.profile.soccer.blue).ToString();
            S_GreenCardCount.text = (profile.profile.soccer.green).ToString();
            S_YellowCardCount.text = (profile.profile.soccer.yellow).ToString();

            B_WinCount.text = (profile.profile.billiard.win).ToString() + "/" + (profile.profile.billiard.total).ToString();

            try
            {
                B_WinRate.text = ((profile.profile.billiard.win / profile.profile.billiard.total) * 100.00f).ToString();
            }
            catch (DivideByZeroException e)
            {
               /// Debug.Log(e.Data.ToString());
            }
            finally
            {
                B_WinRate.text = "0%";
            }


            B_PurpleCardCount.text = (profile.profile.billiard.purple).ToString();
            B_BlueCardCount.text = (profile.profile.billiard.blue).ToString();
            B_GreenCardCount.text = (profile.profile.billiard.green).ToString();
            B_YellowCardCount.text = (profile.profile.billiard.yellow).ToString();


            SettingButtons(profile);
        }
        private void SettingButtons(HTTPBody.ProfileOtherPerson profile)
        {
            if (profile.isFriend)
            {
                // check is block ?
                if (profile.isBlock == false)
                {
                    MessageButton.gameObject.SetActive(true);
                    ///// Chat Box Ready for Chat this User

                    SetChatBox();
                  /*  MessageButton.onClick.AddListener(() =>
                    {
                        navigationUi.LastProfileChecked = Username.text;
                        navigationUi.ShowPopUp("teamprofilechat");

                    });*/
                    /////
                    BlockPersonButton.gameObject.SetActive(true);
                    /*BlockPersonButton.onClick.AddListener(() =>
                    {
                        Server.RequestBlockUser(Username.text);
                    });*/
                    AddFriendButton.gameObject.SetActive(false);
                    AddFriendButton.onClick.RemoveAllListeners();
                }

                else
                {
                    BlockPersonButton.gameObject.SetActive(true);
                    BlockPersonButton.onClick.AddListener(() =>
                    {
                        Server.RequestBlockUser(Username.text);
                    });

                    MessageButton.gameObject.SetActive(false);
                    MessageButton.onClick.RemoveAllListeners();

                    AddFriendButton.gameObject.SetActive(false);
                    AddFriendButton.onClick.RemoveAllListeners();


                }
            }
            else
            {
                AddFriendButton.gameObject.SetActive(true);
              /*  AddFriendButton.onClick.AddListener(() => {
                    Server.RequsetAddFriend(Username.text);
                    AddFriendButton.gameObject.SetActive(false);
                });*/

                BlockPersonButton.gameObject.SetActive(false);
                BlockPersonButton.onClick.RemoveAllListeners();

                MessageButton.gameObject.SetActive(false);
                MessageButton.onClick.RemoveAllListeners();
            }
        }
        private void SetChatBox()
        {
            Server.SendCurrentPage("chat", Username.text);
            ChatBoxFromProfile.SetElementPage(Avatar.sprite, Username.text, Cup.text);
        }
    }
}