using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.PopupAvatar
{
    public class PopUpAvatarChange : MonoBehaviour
    {
        public ImageContainerTool.ImageContainer Avatars;
        public Image DefaultAvatar;
        public AvatarElement avatarElement;
        public Transform Grid;
        

        private List<GameObject> ListElements;

        public void initPopupAvatar()
        {
            if (ListElements.Count > 0)
                ClearShop();
            ListElements= new List<GameObject>();
            
            for (int i = 0; i < Avatars.imageContainers.Count; i++)
            {
                var element = Instantiate(avatarElement, Grid);
                element.Set(Avatars.LoadImage(Avatars.imageContainers[i].name));
                ListElements.Add(element.gameObject);
            }
        }
        private void ClearShop()
        {
            for (int i = 0; i < ListElements.Count; i++)
            {
                Destroy(ListElements[i]);
            }
            Debug.Log("ClearpopupAvatar");
        }
    }
 
}