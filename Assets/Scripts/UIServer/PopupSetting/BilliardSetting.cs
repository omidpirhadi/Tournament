using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
namespace Diaco.Setting
{
    public class BilliardSetting : MonoBehaviour
    {
        [SerializeField] private GeneralSetting generalSetting;
        public bool AutoSave = false;
        public ToggleGroup s;
        public Toggle AccuracyAimShowOn_Toggel;
        public Toggle AccuracyAimShowOff_Toggel;

        public Toggle SpeedRotateAimSlow_Toggel;

        public Toggle SpeedRotateAimNormal_Toggel;
        public Toggle SpeedRotateAimFast_Toggel;
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
            

            if(generalSetting.Setting.billiardsettingdata.accuracyAimShow == true)
            {
                
                AccuracyAimShowOn_Toggel.isOn = true;
            }
            else
            {
                AccuracyAimShowOff_Toggel.isOn = true;
            }

            if (generalSetting.Setting.billiardsettingdata.speedRotateAimCue == SpeedElemenInUi.Slow)
            {
                SpeedRotateAimSlow_Toggel.isOn = true;
                

            }
            else if (generalSetting.Setting.billiardsettingdata.speedRotateAimCue == SpeedElemenInUi.Normal)
            {
                SpeedRotateAimNormal_Toggel.isOn = true;
            }
            else if (generalSetting.Setting.billiardsettingdata.speedRotateAimCue == SpeedElemenInUi.Fast)
            {
                SpeedRotateAimFast_Toggel.isOn = true;
            }
        }
        private void SetEventToggels()
        {
            AccuracyAimShowOn_Toggel.onValueChanged.AddListener(state =>
            {
                if (state)
                    generalSetting.Setting.billiardsettingdata.accuracyAimShow = true;
            });
            AccuracyAimShowOff_Toggel.onValueChanged.AddListener(state =>
            {
                if (state)
                    generalSetting.Setting.billiardsettingdata.accuracyAimShow = false;
            });
            SpeedRotateAimSlow_Toggel.onValueChanged.AddListener(state =>
            {
                if (state)
                {
                    generalSetting.Setting.billiardsettingdata.speedRotateAimCue = SpeedElemenInUi.Slow;
                }
                    
            });
            SpeedRotateAimNormal_Toggel.onValueChanged.AddListener(state =>
            {
                if (state)
                {
                    generalSetting.Setting.billiardsettingdata.speedRotateAimCue = SpeedElemenInUi.Normal;
                }

            });
            SpeedRotateAimFast_Toggel.onValueChanged.AddListener(state =>
            {
                if (state)
                {
                    generalSetting.Setting.billiardsettingdata.speedRotateAimCue = SpeedElemenInUi.Fast;
                }

            });
        }
        private void RemoveEventToggels()
        {
            AccuracyAimShowOn_Toggel.onValueChanged.RemoveAllListeners();
            AccuracyAimShowOff_Toggel.onValueChanged.RemoveAllListeners();
            SpeedRotateAimSlow_Toggel.onValueChanged.RemoveAllListeners();
            SpeedRotateAimNormal_Toggel .onValueChanged.RemoveAllListeners();
            SpeedRotateAimFast_Toggel.onValueChanged.RemoveAllListeners();
        }
    }
}
