using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.RoyalTournument
{
    public class CompetitionInfoPlayerCard : MonoBehaviour
    {
        public Image Admin;
        public Image ImageProfile;
        public Text Username;
        public Text Cup;

        public void Set(bool admin , Sprite profile, string user, int cup)
        {
          /*  if(!admin)
            {
                Admin.enabled = false;
            }*/
            ImageProfile.sprite = profile;
            Username.text = user;
            Cup.text = cup.ToString();

        }
    }
}