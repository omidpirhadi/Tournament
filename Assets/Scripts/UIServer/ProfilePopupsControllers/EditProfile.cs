using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
namespace Diaco.UI.Profile
{
    [Obsolete]
    public class EditProfile : MonoBehaviour
    {
        public ServerUI Server;
        public InputField Name;
        public InputField Family;
        public InputField Email;
        public Dropdown Sex;
        public InputField Password;
        public InputField ConfrimPassword;
        public InputField Birthday;
        public InputField PhoneNumber;
        public Button SendEditButton;
        public Button LogoutButton;

        private ProfileEdited Profileedited;
        public void OnEnable()
        {
            Profileedited = new ProfileEdited();
            
            Sex.onValueChanged.AddListener((x) =>
            {
                Profileedited.gender = x;
               

            });
            SendEditButton.onClick.AddListener(() => {
                SendProfile();
            });
            InitializePage();
        }
        public void InitializePage()
        {
            Profileedited.gender = 0;

            Name.text = Server.BODY.profile.information.firstName;
            Family.text = Server.BODY.profile.information.lastName;
            Email.text = Server.BODY.profile.information.email;
            Sex.value = Server.BODY.profile.information.gender;
            Password.text = "----";
            ConfrimPassword.text = "----";
            Birthday.text = (Server.BODY.profile.information.birthyear).ToString();
            PhoneNumber.text = (Server.BODY.profile.information.phone);
        }
        public void SendProfile()
        {
            if (CheckFields())
            {
                Profileedited.firstName = Name.text;
                Profileedited.lastName = Family.text;
                Profileedited.email = Email.text;
                Profileedited.password = Password.text;
                Profileedited.confirmPassword = ConfrimPassword.text;
                Profileedited.birthyear = Birthday.text;
                Profileedited.phone = PhoneNumber.text;

                Server.SendEditedProfile(Profileedited);
            }
        }
        private bool CheckFields()
        {
            bool Fill = true;
            if (Name.text == "")
            {
                Fill = false;
            }
            if (Family.text == "")
            {
                Fill = false;
            }
            if (Email.text == "")
            {
                Fill = false;
            }
            if (Password.text == "")
            {
                Fill = false;
            }
            if (ConfrimPassword.text == "")
            {
                Fill = false;
            }
            if (Birthday.text == "")
            {
                Fill = false;
            }
            if (PhoneNumber.text == "")
            {
                Fill = false;
            }
            if (Password.text != ConfrimPassword.text)
            {
                Fill = false;
            }
            return Fill;
        }
    }
    

}