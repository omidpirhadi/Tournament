using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    public class SoccerShopOwnerElement : MonoBehaviour
    {
        public int ID;
        public Image TeamImage;
        public Image PrograssBarForce;
        public Image PrograssBarAim;
        public Text RemainderTime;
        public Button Btn_Use;

        public void SetForTeamElement(int id, Sprite teamImage, float force, float aim, string time)
        {
            ID = id;
            TeamImage.sprite = teamImage;
            PrograssBarForce.fillAmount = force;
            PrograssBarAim.fillAmount = aim;
            RemainderTime.text = time;
            Btn_Use.onClick.AddListener(UseButtonClick);
        }
        public void SetForFormationElement(int id, Sprite teamImage, string time)
        {
            ID = id;
            TeamImage.sprite = teamImage;
            RemainderTime.text = time;
            Btn_Use.onClick.AddListener(UseButtonClick);
        }
        private void UseButtonClick()
        {
            Debug.Log("Use This Team:" + ID);

        }
    }
}