using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Notification
{
    public class NotificationReminderTournument : MonoBehaviour
    {
        public PlayeAnimationUI animationUI;
        public Text Timer;
        public Button GoToTournumentbutton;

        void OnEnable()
        {
            GoToTournumentbutton.onClick.AddListener(GoToTurnument);

            animationUI.playanimation();
        }
        void OnDisable()
        {
            GoToTournumentbutton.onClick.AddListener(GoToTurnument);
        }
        void GoToTurnument()
        {

        }
    }
}