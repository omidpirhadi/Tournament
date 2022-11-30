using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.MatchRecord
{
    public class MatchRecordPlayerCard : MonoBehaviour
    {
        public Text rownumber;
        public Image Profile;
        public Text  Username;
        public Text  Star;
        public Text  Time;
        public Toggle[] Fail;

        public void  Set(Sprite profile,int row, string user, int bestrecord, string time,int failcount)

        {
            Fail[0].isOn = false;
            Fail[1].isOn = false;
            Fail[2].isOn = false;
            rownumber.text = row.ToString();
            Profile.sprite = profile;

            Username.text = user;

            Star.text = bestrecord.ToString();
            Time.text = time;
           /* for (int i = 0; i < failcount; i++)
            {
                Fail[i].isOn = true;
            }*/
        }
    }
}