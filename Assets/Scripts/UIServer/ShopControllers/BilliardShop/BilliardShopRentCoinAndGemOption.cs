using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Billiard
{
    [RequireComponent(typeof(Button))]
    public class BilliardShopRentCoinAndGemOption : MonoBehaviour
    {
        public int cueId;
        public int rentId;
        public Text Day;
        public Text CostCoin;
        public Text CostGem;
        public void Set(int cueid, int rentid, int day, int costcoin, int costgem)
        {
            cueId = cueid;
            rentId = rentid;
            Day.text = day.ToString();
            CostCoin.text = costcoin.ToString();
            CostGem.text = costgem.ToString();
            GetComponent<Button>().onClick.AddListener(onClick);
        }
        private void onClick()
        {
            if (FindObjectOfType<Diaco.EightBall.Server.BilliardServer>())
            {
                FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().Emit_RentCue(cueId, rentId);
            }

            Debug.Log("Rent this Cue:" + cueId + "For:" + Day);
        }
    }
}