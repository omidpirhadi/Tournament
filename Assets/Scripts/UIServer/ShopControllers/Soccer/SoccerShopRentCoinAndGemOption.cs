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
        public TypeElement typeElement;
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
            if (FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>())/// in game
            {
                if (typeElement == TypeElement.Team)
                {
                    //// do nothing 
                }
                else
                {
                    FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_RentFormation(teamId, rentId);
                    Debug.Log("Rent this Formation:" + teamId + "For:" + Day);
                }
            }
            else if (FindObjectOfType<ServerUI>())/// in ui
            {
                if (typeElement == TypeElement.Team)
                {
                    Debug.Log("Rent this Team:" + teamId + "For:" + Day);
                }
                else
                {
                    Debug.Log("Rent this Formation:" + teamId + "For:" + Day);
                }
            }
            
        }
    }
}