using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Profile
{

    public class ProfileOtherPersonPopup : MonoBehaviour
    {

        public Diaco.ImageContainerTool.ImageContainer AchivmentImage;
        public ServerUI Server;
        public NavigationUI navigationUi;

      //  public Diaco.UI.Chatbox.ChatBoxController ChatBoxFromProfile;

        public Button AddFriendButton;
        public Button BlockPersonButton;
        public Button MessageButton;


        public Image Avatar;
        public Text Username;
        public string UserID;
        public Text CupSoccer;
        public Text CupBilliard;
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

        

        private void OnEnable()
        {

          
             AddFriendButton.onClick.AddListener(() => {
                  Server.RequsetAddFriend(UserID);
                  AddFriendButton.gameObject.SetActive(false);
              });
            BlockPersonButton.onClick.AddListener(() =>
                   {
                       Server.RequestBlockUser(UserID);
                   });
              MessageButton.onClick.AddListener(() =>
                 {
                     Server.SendRequestOpenChatBox(UserID);

                 });

        
        }
        private void OnDisable()
        {
         
           MessageButton.onClick.RemoveAllListeners();
            BlockPersonButton.onClick.RemoveAllListeners();
          AddFriendButton.onClick.RemoveAllListeners();
        }


        public void InitializeProfile(HTTPBody.ProfileOtherPerson data)
        {


            Avatar.sprite = Server.AvatarContainer.LoadImage(data.profile.avatar);
            UserID = data.id;
            Username.text = data.userName;
            CupSoccer.text = (data.profile.soccer_cup).ToString();
            CupBilliard.text = (data.profile.billiard_cup).ToString();
            RankLevel.sprite = Server.AvatarContainer.LoadImage(data.profile.avatar);
            Description.text = data.profile.description;

            S_WinCount.text = (data.profile.soccer.win).ToString() + "/" + (data.profile.soccer.total).ToString();
            try
            {
                if(data.profile.soccer.win ==0.0f)
                    S_WinRate.text = "0%";
                else
                S_WinRate.text = ((data.profile.soccer.win / data.profile.soccer.total) * 100.00f).ToString();
            }
            catch (DivideByZeroException e)
            {
                S_WinRate.text = "0%";
                Debug.Log(e.Data.ToString());
            }
    


            S_PurpleCardCount.text = (data.profile.soccer.purple).ToString();
            S_BlueCardCount.text = (data.profile.soccer.blue).ToString();
            S_GreenCardCount.text = (data.profile.soccer.green).ToString();
            S_YellowCardCount.text = (data.profile.soccer.yellow).ToString();

            B_WinCount.text = (data.profile.billiard.win).ToString() + "/" + (data.profile.billiard.total).ToString();

            try
            {
                if (data.profile.billiard.win == 0.0f)
                    B_WinRate.text = "0%";
                else
                    B_WinRate.text = ((data.profile.billiard.win / data.profile.billiard.total) * 100.00f).ToString();
            }
            catch (DivideByZeroException e)
            {
                 Debug.Log(e.Data.ToString());

                B_WinRate.text = "0%";
            }



            B_PurpleCardCount.text = (data.profile.billiard.purple).ToString();
            B_BlueCardCount.text = (data.profile.billiard.blue).ToString();
            B_GreenCardCount.text = (data.profile.billiard.green).ToString();
            B_YellowCardCount.text = (data.profile.billiard.yellow).ToString();


            SettingButtons(data);
            Achivment_Init();
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

                   // SetChatBox();
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
                  /*  BlockPersonButton.onClick.AddListener(() =>
                    {
                        Server.RequestBlockUser(UserID);
                    });*/

                    MessageButton.gameObject.SetActive(false);
                    MessageButton.onClick.RemoveAllListeners();

                    AddFriendButton.gameObject.SetActive(false);
                    AddFriendButton.onClick.RemoveAllListeners();


                }
            }
            else
            {
                AddFriendButton.gameObject.SetActive(true);
                /*AddFriendButton.onClick.AddListener(() => {
                    Server.RequsetAddFriend(UserID);
                    AddFriendButton.gameObject.SetActive(false);
                });*/

                BlockPersonButton.gameObject.SetActive(false);
                BlockPersonButton.onClick.RemoveAllListeners();

                MessageButton.gameObject.SetActive(false);
                MessageButton.onClick.RemoveAllListeners();
            }
        }
        private void Achivment_Init()
        {
            for (int i = 0; i < Server.BODY.profile.achievements.Count; i++)
            {


                if (Server.BODY.profile.achievements[i].active)
                {
                    Achivments[i].color = new Color(1f, 1f, 1f, 1f);
                    Achivments[i].sprite = AchivmentImage.LoadImage(Server.BODY.profile.achievements[i].name);
                }
                else
                {
                    Achivments[i].color = new Color(1f, 1f, 1f, 0.4f);
                    Achivments[i].sprite = AchivmentImage.LoadImage(Server.BODY.profile.achievements[i].name);
                }

            }
        }
        private void SetChatBox()
        {
            //Server.SendCurrentPage("chat", Username.text);
           // ChatBoxFromProfile.SetElementPage(Avatar.sprite, Username.text, Cup.text);
        }
    }
}