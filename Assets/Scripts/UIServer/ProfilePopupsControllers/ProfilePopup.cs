﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Profile
{


    public class ProfilePopup : MonoBehaviour
    {
        public   ServerUI Server;
        public Diaco.ImageContainerTool.ImageContainer AchivmentImage;

        public Image Avatar;
        public Text Username;
        public Text Cup;
        public Image RankLevel;
        public InputField EditDescription_Inputfield;
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
            EditDescription_Inputfield.onEndEdit.AddListener((des) => {

                if (des.Length > 0 && des != Server.BODY.profile.description)
                {
                    var context = PersianFix.Persian.Fix(des, 255);
                    EditDescription_Inputfield.text = context;
                    Server.RequestEditDescription(context);
                }
            });
            InitializeProfile();
        }
        public void InitializeProfile()
        {
            Avatar.sprite = Server.AvatarContainer.LoadImage(Server.BODY.profile.avatar);

            Username.text = Server.BODY.userName;
            Cup.text = (Server.BODY.profile.soccer_cup).ToString();
           // RankLevel.sprite = Server.AvatarContainer.LoadImage(Server.BODY.profile.avatar);
            EditDescription_Inputfield.text = Server.BODY.profile.description;

            S_WinCount.text = (Server.BODY.profile.soccer.win).ToString() + "/" + (Server.BODY.profile.soccer.total).ToString();
            try
            {
                S_WinRate.text = ((Server.BODY.profile.soccer.win / Server.BODY.profile.soccer.total) * 100.00f).ToString()+"%";
            }
            catch(DivideByZeroException e)
            {
                Debug.Log(e.Data.ToString());
                S_WinRate.text = "0%";
            }
    


            S_PurpleCardCount.text = (Server.BODY.profile.soccer.purple).ToString();
            S_BlueCardCount.text = (Server.BODY.profile.soccer.blue).ToString();
            S_GreenCardCount.text = (Server.BODY.profile.soccer.green).ToString();
            S_YellowCardCount.text = (Server.BODY.profile.soccer.yellow).ToString();

            B_WinCount.text = (Server.BODY.profile.billiard.win).ToString() + "/" + (Server.BODY.profile.billiard.total).ToString();

            try
            {
                B_WinRate.text = ((Server.BODY.profile.billiard.win / Server.BODY.profile.billiard.total) * 100.00f).ToString()+"%";
            }
            catch (DivideByZeroException e)
            {
                Debug.Log(e.Data.ToString());
                B_WinRate.text = "0%";
            }

           

            B_PurpleCardCount.text = (Server.BODY.profile.billiard.purple).ToString();
            B_BlueCardCount.text = (Server.BODY.profile.billiard.blue).ToString();
            B_GreenCardCount.text = (Server.BODY.profile.billiard.green).ToString();
            B_YellowCardCount.text = (Server.BODY.profile.billiard.yellow).ToString();
            Achivment_Init();

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
    }
}