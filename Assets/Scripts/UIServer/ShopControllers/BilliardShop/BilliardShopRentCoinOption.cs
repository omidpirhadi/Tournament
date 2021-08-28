using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Billiard
{
    [RequireComponent(typeof(Button))]
    public class BilliardShopRentCoinOption : MonoBehaviour
    {
        public int cueId;
        public int rentId;
        public Text Day;
        public Text Cost;
        public void Set(int cueid, int rentid, int day, int cost)
        {
            cueId = cueid;
            rentId = rentid;
            Day.text = day.ToString();
            Cost.text = cost.ToString();
            GetComponent<Button>().onClick.AddListener(onClick);
        }
        private void onClick()
        {
            Debug.Log("Rent this Team:" + cueId + "For:" + Day);
        }
    }
}