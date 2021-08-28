using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.HightScoreTab
{
    public class HightScoreCard : MonoBehaviour
    {

        public Image Avatar;
        public Text UserName;
        public Text StarIndicator;
        public Text CUPIndicator;
        public Text GEMIndicator;

        public void SetCardCUP(Sprite avatar, string user, string starcount, string cupcount )
        {
            Avatar.sprite = avatar;
            UserName.text = user;
            StarIndicator.text = starcount;
            CUPIndicator.text = cupcount;

        }
        public void SetCardGEM(Sprite avatar, string user, string starcount, string gemcount)
        {
            Avatar.sprite = avatar;
            UserName.text = user;
            StarIndicator.text = starcount;
            GEMIndicator.text = gemcount;
        }

    }
}