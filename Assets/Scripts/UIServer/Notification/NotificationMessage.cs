using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Notification
{
    public class NotificationMessage : MonoBehaviour
    {
        public PlayeAnimationUI animationUI;
        public Text context;
        public Button Hidebutton;

        void OnEnable()
        {
            Hidebutton.onClick.AddListener(hide);

            animationUI.playanimation();
        }
        void OnDisable()
        {
            Hidebutton.onClick.AddListener(hide);
        }
        void hide()
        {
            animationUI.playanimation();
        }
    }
}