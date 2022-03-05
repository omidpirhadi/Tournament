using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.PopupAvatar
{
    public class AvatarElement : MonoBehaviour
    {
        public string Name;
        public Image Avatar;
        public Button btn_Choise;

        public void Set(Sprite avatar, bool active, bool inUse, string name)
        {
            this.Name = name;
            Avatar.sprite = avatar;
            if (inUse)
            {
                btn_Choise.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("فعال", 255);
                btn_Choise.interactable = false;
            }
            else
            {
                btn_Choise.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("جایگذاری", 255);
                btn_Choise.interactable = true;
            }
            if (active)
                btn_Choise.onClick.AddListener(Choise);
            else
                btn_Choise.interactable = false;
        }
        private void Choise()
        {
            FindObjectOfType<ServerUI>().RequestEditAvatar(Name);
            Debug.Log("AvatarChanged");

        }
    }
}