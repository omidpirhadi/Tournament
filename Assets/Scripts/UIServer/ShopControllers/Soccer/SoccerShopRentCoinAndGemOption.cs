using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    [RequireComponent(typeof(Button))]
    public class SoccerShopRentCoinAndGemOption : MonoBehaviour
    {
        public int teamId;
        public int rentId;
        public Text Day;
        public Text CostCoin;
        public Text CostGem;
        public void Set(int teamid, int rentid, int day, int costcoin, int costgem)
        {
            teamId = teamid;
            rentId = rentid;
            Day.text = day.ToString();
            CostCoin .text = costcoin.ToString();
            CostGem.text = costgem.ToString();
            GetComponent<Button>().onClick.AddListener(onClick);
        }
        private void onClick()
        {
            Debug.Log("Rent this Team:" + teamId + "For:" + Day);
        }
    }
}