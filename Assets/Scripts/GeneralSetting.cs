using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.UI;
namespace Diaco.Setting
{

    public enum _store { CafeBazzar = 0, Zarinpal= 1};
    public class GeneralSetting : MonoBehaviour
    {

        public string ServerAddress;
        public string LoginAPI;
        public GameSettingData Setting;
        public string Version;
        public string Description;
        public _store Store;

        private void Awake()
        {

            if (ExistSettingFile("setting"))
            {
                //   GameSetting = new GameSettingData();
                Setting = LoadSetting("setting");
            }
            else
            {
                Setting = new GameSettingData
                {
                    Sound = true,
                    vibration = true,
                    status = true,
                    reciveFriendRequest = true,
                    reciveLeagueRequest = true,
                    reciveMatchFriendRequest = true,
                    soccersettingdata = new SoccerSettingData { aimForward = false },
                    billiardsettingdata = new BilliardSettingData
                    {
                        accuracyAimShow = true,
                        //powerWoodCuePosition = PositionElemenInUi.Left,
                        //powerWoodCueShow = PositionElemenInUi.Right,
                        speedRotateAimCue = SpeedElemenInUi.Normal
                    }


                };
                SaveSetting();
            }
        }


        public bool ExistSettingFile(string FileName)
        {
            bool find = false;
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                find = true;
            }
            return find;
        }
        public GameSettingData LoadSetting(string FileName)
        {
            GameSettingData settingData = new GameSettingData();
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                var j_set = File.ReadAllText(Application.persistentDataPath + "//" + FileName + ".json");
                settingData = JsonUtility.FromJson<GameSettingData>(j_set);

            }
            Debug.Log("Setting Loaded!");

            return settingData;
        }

        public void SaveSetting()
        {
            var FileName = "setting";


            var set_json = JsonUtility.ToJson(Setting);
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                File.Delete(Application.persistentDataPath + "//" + FileName + ".json");
            }
            File.WriteAllText(Application.persistentDataPath + "//" + FileName + ".json", set_json);
            Handler_OnChangeSetting();


            Debug.Log("Setting Saved!");
        }


        private Action settingchange;
        public event Action OnChangeSetting
        {
            add { settingchange += value; }
            remove { settingchange -= value; }
        }
        protected void Handler_OnChangeSetting()
        {
            if (settingchange != null)
            {
                settingchange();

            }
        }
    }
    [Flags]
    public enum PositionElemenInUi : short { Left = 0, Right = 1, Up = 3, Bottom = 4 };
    [Flags]
    public enum SpeedElemenInUi : short { Slow = 0, Normal = 1, Fast = 2 };
    [Serializable]
    public struct GameSettingData
    {
        public bool Sound;
        public bool vibration;
        public bool status;
        public bool reciveFriendRequest;
        public bool reciveMatchFriendRequest;
        public bool reciveLeagueRequest;
        public BilliardSettingData billiardsettingdata;
        public SoccerSettingData soccersettingdata;
    }
    [Serializable]
    public struct GameSettingDataServer
    {

        public bool status;
        public bool reciveFriendRequest;
        public bool reciveMatchFriendRequest;
        public bool reciveLeagueRequest;

    }
    [Serializable]
    public struct BilliardSettingData
    {
        public bool accuracyAimShow;
        // public PositionElemenInUi powerWoodCueShow;
        //public PositionElemenInUi powerWoodCuePosition;
        public SpeedElemenInUi speedRotateAimCue;
    }
    [Serializable]
    public struct SoccerSettingData
    {
        public bool aimForward;
    }
}