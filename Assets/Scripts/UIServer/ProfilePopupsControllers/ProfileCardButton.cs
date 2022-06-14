using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.Profile
{
    
    public class ProfileCardButton : MonoBehaviour
    {
        public string ID;
        private Button card;
        private void OnEnable()
        {
            card = GetComponent<Button>();
            card.onClick.AddListener(() =>
            {
                var server = FindObjectOfType<ServerUI>();
                server.Emit_Card(ID);
            });
        }
        private void OnDisable()
        {
            card.onClick.RemoveAllListeners();
        }
    }
}