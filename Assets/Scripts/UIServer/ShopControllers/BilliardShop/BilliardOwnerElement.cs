using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Billiard
{
    public class BilliardOwnerElement : MonoBehaviour
    {
        public bool InGame = false;
        public _GameLobby Lobby;

        public string ID;
        public Image CueImage;
        public Image PrograssBarForce;
        public Image PrograssBarAim;
        public Image PrograssBarspin;
        public Text RemainderTime;
        public Button Btn_Use;

        public void Set(string id, Sprite teamImage, float force, float aim,float spin, string time,bool ingame)
        {
            ID = id;
            CueImage.sprite = teamImage;
            PrograssBarForce.fillAmount = force;
            PrograssBarAim.fillAmount = aim;
            PrograssBarspin.fillAmount = spin;
            RemainderTime.text = time;
            InGame = ingame;
            
            Btn_Use.onClick.AddListener(UseButtonClick);
        }
     
        private void UseButtonClick()
        {
            if(!InGame)
            {
                FindObjectOfType<ServerUI>().Emit_UseCue(ID);
            }
            else
            {
                FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().Emit_UseCue(ID);
            }

        }
    }
}