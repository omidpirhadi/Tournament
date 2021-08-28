using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Social.BadgesElement
{
    public class BadgeElement : MonoBehaviour
    {
        public Button btnbadge;
        public string IdBadge;
        public Image BadgeImage;
        public SelectBadgePopUpController Controller;
        public void SetBadge(Sprite image, string Id, SelectBadgePopUpController controller)
        {
            Controller = controller;
            IdBadge = Id;
            BadgeImage.sprite = image;
            btnbadge.onClick.AddListener(() => {
                SendIDBadgeToPopupController();
            });
        }
        public void SendIDBadgeToPopupController()
        {
            Controller.BadgeID = IdBadge;
            Controller.ClearBadges();
            Controller.navigationUI.ClosePopUp("selectbadge");
        }
    }
}