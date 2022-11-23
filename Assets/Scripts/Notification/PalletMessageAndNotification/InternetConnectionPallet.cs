using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Diaco.Notification
{
    public class InternetConnectionPallet : MonoBehaviour
    {
        public RTLTMPro.RTLTextMeshPro Context_text;
        public void SetInternetDialog(Notification_Dialog_Body body  , bool show)
        {
            Context_text.text = body.context;
            ShowDialog(show);
        }
        private void ShowDialog(bool show)
        {

            this.gameObject.SetActive(show);
        

        }
    }
}