using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.PopupEditUsername
{
    public class PopupEditUsername : MonoBehaviour
    {
        public InputField Username_Text;
        public Text Dialog_text;
        public bool AllowUsername = false;

        public Button Confirm_Button;
        void Start()
        {

            Username_Text.onEndEdit.AddListener((u) =>
            {
               var user =  PersianFix.Persian.Fix(Username_Text.text, 255);
                Username_Text.text = user;
                CheckUsername(user);
            });

        }


        void CheckUsername(string user)
        {
            if(AllowUsername)
            {
                Dialog_text.color = Color.green;
                Dialog_text.text = PersianFix.Persian.Fix( "نام کاربری تایید شد.", 255); 
            }
            else
            {
                Dialog_text.color = Color.red;
                Dialog_text.text = PersianFix.Persian.Fix("نام کاربری قبلا استفاده شده.", 255);
            }
        }
    }
}