
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
namespace Diaco.Notification
{

    public class NotificationPallet : MonoBehaviour
    {
        [SerializeField] private string NotificationEvent = "";
        [SerializeField] private string EventData = "";
        [SerializeField] private int TypeAction;
        public RTLTMPro.RTLTextMeshPro Context_text;
        public Image Notification_Image;
        public List<Button> NotificationButtons;
        public RectTransform PalletRectTransform;
        public float SpeedAnimation;
        public float DurationShow;
        public Vector2 Down, Up;
        public void SetNotification( Notification_Dialog_Body body)
        {
            ClearPallet();
            NotificationEvent = body.eventName;
            EventData = body.eventData;
            TypeAction = body.actionButton;
            if (body.notificationType== 0)/// with two button without image
            {
                NotificationButtons[0].gameObject.SetActive(true);
                NotificationButtons[0].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = body.greenButtonText;
                NotificationButtons[1].gameObject.SetActive(true);
                NotificationButtons[1].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = body.redButtonText;
            }
            else if(body.notificationType == 1)/// with accept button  and  with image
            {
                Notification_Image.gameObject.SetActive(true);
                NotificationButtons[0].gameObject.SetActive(true);

                NotificationButtons[0].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = body.greenButtonText;
                Notification_Image.sprite = ConvertImageToSprite(body.image);
            }
            Context_text.text = body.context;

            NotificationButtons[0].onClick.AddListener(AcceptButton);
            NotificationButtons[1].onClick.AddListener(RejectButton);
            ShowNotification();
        }
        [Button("ShowTest", ButtonSizes.Medium, ButtonStyle.Box)]
        public void ShowNotification()
        {
            PalletRectTransform.DOAnchorPos(Down, SpeedAnimation).OnComplete(() => {

                DOVirtual.DelayedCall(DurationShow, () => {
                    PalletRectTransform.DOAnchorPos(Up, SpeedAnimation);
                });
            });


        }
        private void AcceptButton()
        {
            var server = FindObjectOfType<ServerUI>();
            if (TypeAction == 0)
            {
                if (NotificationEvent == "store")
                {
                    var ui = FindObjectOfType<NavigationUI>(); 

                    ui.SwitchUI("shop");
                    server.RequestItemShop();
                }
            }
            else
            {

                SetServerForEmitData();
            }
            PalletRectTransform.DOAnchorPos(Up, SpeedAnimation*2);
        }
        private void RejectButton()
        {
            PalletRectTransform.DOAnchorPos(Up, SpeedAnimation * 2);
        }
        private void ClearPallet()
        {
            NotificationButtons[0].gameObject.SetActive(false);
            NotificationButtons[0].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = "";
            NotificationButtons[1].gameObject.SetActive(false);
            NotificationButtons[1].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = "";
            Notification_Image.gameObject.SetActive(false);
            Notification_Image.sprite = null;
            Context_text.text = "";
            NotificationButtons[0].onClick.RemoveListener(AcceptButton);
            NotificationButtons[1].onClick.RemoveListener(RejectButton);
        }
        private void SetServerForEmitData()
        {
            var server_ui = FindObjectOfType<ServerUI>();
            var server_billiard = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
            var server_soccer = FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>();
            if (server_ui)
                server_ui.Emit_DialogAndNotification(NotificationEvent, EventData);
            else if (server_billiard)
                server_billiard.Emit_DialogAndNotification(NotificationEvent, EventData);
            else if (server_soccer)
                server_soccer.Emit_DialogAndNotification(NotificationEvent, EventData);
        }
        private Sprite ConvertImageToSprite(string image)
        {

            var image_byte = System.Convert.FromBase64String(image);
            Texture2D texture = new Texture2D(512, 512, TextureFormat.DXT5, false);
            texture.LoadImage(image_byte);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}