using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.PopupAvatar
{
    public class AvatarElement : MonoBehaviour
    {
        public Image Avatar;
        public Button btn_Choise;

        public void Set(Sprite avatar)
        {
            Avatar.sprite = avatar;
            btn_Choise.onClick.AddListener(Choise);
        }
        private void Choise()
        {
            Debug.Log("AvatarChanged");

        }
    }
}