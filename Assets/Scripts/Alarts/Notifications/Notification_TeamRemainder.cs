using System;


using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Diaco.Alart.Notifications
{
    [Obsolete]
    public class Notification_TeamRemainder : MonoBehaviour
    {
        public RectTransform Panel;
        public float MoveOffset = 110.0f;
        public float MoveDuration = 1.0f;
        public float ShowDuration = 1.0f;

        
        public Text Message;
        public Text Time;
        public Image GameLogo;
        public string Command;
        public Button AcceptRequestButton;
        ///public Button IgnorRequestButton;


        public void Set( string meessage,string time, Sprite logo, string command)
        {
            
            this.Message.text = meessage;
            this.Time.text = time;
            this.GameLogo.sprite = logo;
            this.Command = command;
            AcceptRequestButton.onClick.AddListener(accept_click);

        }
        private void Show()
        {

            Panel.DOAnchorPos(new Vector2(0, 0), MoveDuration).OnComplete(() =>
            {

                DOVirtual.DelayedCall(ShowDuration, () =>
                {
                    Panel.DOAnchorPos(new Vector2(0, 110), MoveDuration / 2).OnComplete(() =>
                    {
                        AcceptRequestButton.onClick.RemoveAllListeners();
                    });
                });

            });
        }

        private void accept_click()
        {

        }
    }
}
