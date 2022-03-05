using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.PopupAvatar
{
    public class PopUpAvatarChange : MonoBehaviour
    {
        public ServerUI server;
        public ImageContainerTool.ImageContainer Avatars;
        public Image DefaultAvatar;
        public AvatarElement avatarElement;
        public Transform Grid;


        private List<GameObject> ListElements = new List<GameObject>();

        private void OnEnable()
        {
            server = FindObjectOfType<ServerUI>();
            initPopupAvatar(server.BODY.inventory.avatars);
        }
        public void initPopupAvatar(List<string> avatars)
        {
            if (ListElements.Count > 0)
                ClearShop();

            DefaultAvatar.sprite = Avatars.LoadImage(server.BODY.profile.avatar);
            
            for (int i = 0; i < avatars.Count; i++)
            {
                var element = Instantiate(avatarElement, Grid);
                if (server.BODY.profile.avatar == Avatars.imageContainers[i].name)
                    element.Set(Avatars.LoadImage(Avatars.imageContainers[i].name), true, true, Avatars.imageContainers[i].name);
                else
                    element.Set(Avatars.LoadImage(Avatars.imageContainers[i].name), true, false, Avatars.imageContainers[i].name);

                ListElements.Add(element.gameObject);
            }


            for (int i = 0; i < Avatars.imageContainers.Count; i++)
            {
                
                if(!avatars.Contains( Avatars.imageContainers[i].name))
                {
                    var element = Instantiate(avatarElement, Grid);
                    element.Set(Avatars.LoadImage(Avatars.imageContainers[i].name), false, false, Avatars.imageContainers[i].name);

                    ListElements.Add(element.gameObject);
                }
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