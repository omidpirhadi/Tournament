using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Setting
{
    public class SoccerSetting : MonoBehaviour
    {
        [SerializeField] private GeneralSetting generalSetting;
        public bool AutoSave = false;
        public Toggle ForwardAimOn_Toggel;
        public Toggle ForwardAimOff_Toggel;


        void OnEnable()
        {
            generalSetting = FindObjectOfType<GeneralSetting>();

            Set();
            SetEventToggels();
        }
        void OnDisable()
        {
            RemoveEventToggels();
            if (AutoSave)
                generalSetting.SaveSetting();
        }
        private void Set()
        {
           // ForwardAimOn_Toggel.isOn = generalSetting.Setting.soccersettingdata.aimForward;
            if(generalSetting.Setting.soccersettingdata.aimForward == true)
            {
                ForwardAimOn_Toggel.isOn = true;
            }
            else
            {
                ForwardAimOn_Toggel.isOn = false;
            }

        }
        private void SetEventToggels()
        {
            ForwardAimOn_Toggel.onValueChanged.AddListener(state => {
                if (state)
                    generalSetting.Setting.soccersettingdata.aimForward = true;
            });
            ForwardAimOff_Toggel.onValueChanged.AddListener(state => {
                if (state)
                    generalSetting.Setting.soccersettingdata.aimForward = false;
            });
        }
        private void RemoveEventToggels()
        {
            ForwardAimOn_Toggel.onValueChanged.RemoveAllListeners();
            ForwardAimOff_Toggel.onValueChanged.RemoveAllListeners();
        }
    }
}
