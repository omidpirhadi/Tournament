using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.PopupEditUsername
{
    public class PopupEditUsername : MonoBehaviour
    {
        private ServerUI server;
        public TMPro.TMP_InputField Username_Text;
        public Text Dialog_text;
        // public bool AllowUsername = false;

        public Button Confirm_Button;
        void Start()
        {


        }
        private void OnEnable()
        {
            server = FindObjectOfType<ServerUI>();
            server.OnchangeUsername += PopupEditUsername_OnchangeUsername;

            Username_Text.onValueChanged.AddListener((x) =>
            {
                Dialog_text.text = "";

            });

            Confirm_Button.onClick.AddListener(() =>
            {

                if (Username_Text.text.Length > 0)
                    CheckUsername(Username_Text.text);
            });
        }


        private void OnDisable()
        {
            server.OnchangeUsername -= PopupEditUsername_OnchangeUsername;
            Username_Text.onValueChanged.RemoveAllListeners();
            Confirm_Button.onClick.RemoveAllListeners();
        }
        private void PopupEditUsername_OnchangeUsername(string user)
        {
            CheckAnswerRequest(user);
        }

        public void CheckAnswerRequest(string context)
        {

            Dialog_text.color = Color.red;
            Dialog_text.text = PersianFix.Persian.Fix(context, 255);

        }
        void CheckUsername(string user)
        {
            server.RequestEditUserName(user);
        }
    }
}