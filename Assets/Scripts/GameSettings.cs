using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.UI; 
namespace Diaco.Setting
{
    public class GameSettings : MonoBehaviour
    {
        
        [SerializeField] GameSettingData GameSetting;
        private void Awake()
        {
            
            if(ExistSettingFile("setting"))
            {
                GameSetting = LoadSetting("setting");
            }
            else
            {
                SaveSetting("setting", GameSetting);
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
            return settingData;
        }

        public void SaveSetting(string FileName, GameSettingData setting)
        {

            var set_json = JsonUtility.ToJson(setting);
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                File.Delete(Application.persistentDataPath + "//" + FileName + ".json");
            }
            File.WriteAllText(Application.persistentDataPath + "//" + FileName + ".json", set_json);

        }
    }
    [Serializable]
    public struct GameSettingData
    {
        public bool Sound;
        public bool vibration;
        public bool status;
        public bool reciveFriendRequest;
        public bool reciveMatchFriendRequest;
        public bool reciveLeagueRequest;
        public BilliardSetting billiardSetting;
        public SoccerSetting soccerSetting;
    }
    [Serializable]
    public struct BilliardSetting
    {
        public bool accuracyAimShow;
        public int powerWoodCueShow;
        public int powerWoodCuePosition;// 0 = horizen 1 = vertical
        public int speedRotateAimCue;// 0  slow , 1 = normal , 2 = fast;
    }
    [Serializable]
    public struct SoccerSetting
    {
        public int aimForward;
    }
}