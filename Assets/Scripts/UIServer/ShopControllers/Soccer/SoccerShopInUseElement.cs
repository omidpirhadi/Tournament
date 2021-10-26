using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    public class SoccerShopInUseElement : MonoBehaviour
    {
      public string ID;
      public Image TeamImage;
      public Image PrograssBarForce;
      public Image PrograssBarAim;
      public Text RemainderTime;
        public void SetForTeamElement(string id, Sprite teamImage, float force, float aim, string time)
        {
            ID = id;
            TeamImage.sprite = teamImage;
            PrograssBarForce.fillAmount = force;
            PrograssBarAim.fillAmount = aim;

            RemainderTime.text = time;
        }
        public void SetForFormationElement(string id, Sprite teamImage, string time)
        {
            ID = id;
            TeamImage.sprite = teamImage;

            RemainderTime.text = time;
        }
    }
}