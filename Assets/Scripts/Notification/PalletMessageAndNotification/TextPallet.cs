using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Diaco.Notification
{


    public class TextPallet : MonoBehaviour
    {
        public RTLTMPro.RTLTextMeshPro TextPallet_text;
        public Image background;
        public float SpeedAnimation;
        public float DurationShow;

        public void SetTextPallet(Notification_Dialog_Body body)
        {
            TextPallet_text.text = "";
            TextPallet_text.text = body.context;
            ShowTextPallet();
        }
        [Button("ShowTest", ButtonSizes.Medium, ButtonStyle.Box)]
        public void ShowTextPallet()
        {

            background.DOFade(1.0f, SpeedAnimation).OnComplete(() => {

                DOVirtual.DelayedCall(DurationShow, () => {
                    background.DOFade(0.0f, SpeedAnimation);
                });
            });
            TextPallet_text.DOFade(0.6f,SpeedAnimation).OnComplete(() => {

                DOVirtual.DelayedCall(DurationShow, () => {
                    TextPallet_text.DOFade(0.0f, SpeedAnimation);
                });
            });

        }
    }
}