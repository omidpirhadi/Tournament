using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Profile
{
    public class ProfileAchivment : MonoBehaviour
    {
        private Button achive_button;
        void OnEnable()
        {
           var server=  FindObjectOfType<ServerUI>();
            achive_button = GetComponent<Button>();

            
            achive_button.onClick.AddListener(() => { server.Emit_AchivementDescription(gameObject.name); });
            
        }
        void OnDisable()

        {
            achive_button.onClick.RemoveAllListeners();
        }
    }
}