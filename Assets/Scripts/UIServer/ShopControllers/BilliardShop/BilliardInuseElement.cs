using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Billiard
{
    public class BilliardInuseElement : MonoBehaviour
    {
        public string ID;
        public Image TeamImage;
        public Image PrograssBarForce;
        public Image PrograssBarAim;
        public Image PrograssBarSpin;
        public RTLTMPro.RTLTextMeshPro RemainderTime;
        public void Set(string id, Sprite teamImage, float force, float aim,float spin, string time)
        {
            ID = id;
            TeamImage.sprite = teamImage;
            PrograssBarForce.fillAmount = force;
            PrograssBarAim.fillAmount = aim;
            PrograssBarAim.fillAmount = spin;
            RemainderTime.text = time;
        }
        
    }
}