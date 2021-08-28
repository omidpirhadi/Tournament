using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    [RequireComponent(typeof(Button))]
    public class SoccerShopRentCoinOption : MonoBehaviour
    {
        public int  teamId;
        public int  rentId;
        public Text Day;
        public Text Cost;
        public void Set(int teamid, int rentid, int day,int cost)
        {
            teamId = teamid;
            rentId = rentid;
            Day.text = day.ToString();
            Cost.text = cost.ToString();
            GetComponent<Button>().onClick.AddListener(onClick);
        }
        private void onClick()
        {
            Debug.Log("Rent this Team:" + teamId + "For:" + Day);
        }
    }
}