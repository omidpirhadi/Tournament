using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.Setting
{
    public class MainMenuSetting : MonoBehaviour
    {
       [SerializeField] private GeneralSetting generalSetting;
        public Toggle Sound_Toggel;
        public Toggle Vibration_Toggel;
        public Toggle ReciveFriendRequest_Toggel;
        public Toggle ReciveLeagueRequest_Toggel;
        public Toggle ReciveMatchFriend_Toggel;
        public Toggle ShowOnlineStatus_Toggel;
        void OnEnable()
        {
            ///setting emit to server;
            generalSetting = FindObjectOfType < GeneralSetting>();

            Set();
            SetEventToggels();
        }
        void OnDisable()
        {
            RemoveEventToggels();
            generalSetting.SaveSetting();
        }
        private void Set()
        {
            Sound_Toggel.isOn = generalSetting.Setting.Sound;
            Vibration_Toggel.isOn = generalSetting.Setting.vibration;
            ShowOnlineStatus_Toggel.isOn = generalSetting.Setting.status;
            ReciveFriendRequest_Toggel.isOn = generalSetting.Setting.reciveFriendRequest;
            ReciveLeagueRequest_Toggel.isOn = generalSetting.Setting.reciveLeagueRequest;
            ReciveMatchFriend_Toggel.isOn = generalSetting.Setting.reciveMatchFriendRequest;


        }
        private void SetEventToggels()
        {
            Sound_Toggel.onValueChanged.AddListener(state => {
                generalSetting.Setting.Sound = state;
            });
            Vibration_Toggel.onValueChanged.AddListener(state => {
                generalSetting.Setting.vibration = state;
            });
            ShowOnlineStatus_Toggel.onValueChanged.AddListener(state => {
                generalSetting.Setting.status = state;
            });
            ReciveFriendRequest_Toggel.onValueChanged.AddListener(state => {
                generalSetting.Setting.reciveFriendRequest = state;
            });
            ReciveLeagueRequest_Toggel.onValueChanged.AddListener(state => {
                generalSetting.Setting.reciveLeagueRequest = state;
            });
            ReciveMatchFriend_Toggel.onValueChanged.AddListener(state => {
                generalSetting.Setting.reciveMatchFriendRequest = state;
            });
        }
        private void RemoveEventToggels()
        {
            Sound_Toggel.onValueChanged.RemoveAllListeners();
            Vibration_Toggel.onValueChanged.RemoveAllListeners();
            ShowOnlineStatus_Toggel.onValueChanged.RemoveAllListeners();
            ReciveFriendRequest_Toggel.onValueChanged.RemoveAllListeners();
            ReciveLeagueRequest_Toggel.onValueChanged.RemoveAllListeners();
            ReciveMatchFriend_Toggel.onValueChanged.RemoveAllListeners();
        }
    }
}
