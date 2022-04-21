using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Ads
{
    public class DiacoAds : MonoBehaviour
    {
        public Button btn;
        public void OnEnable()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(() => {
                var server = FindObjectOfType<ServerUI>();
                server.RequestAds();
            });


        }
        public void OnDisable()
        {
            btn.onClick.RemoveAllListeners();
        }
        public void OnDestroy()
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}