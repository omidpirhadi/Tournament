using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    [RequireComponent(typeof(Button))]
    public class SoccerShopRentCoinAndGemOption : MonoBehaviour
    {
        public string teamId;
        public string rentId;
        public TypeElement typeElement;
        public Text Day;
        public Text CostCoin;
        public Text CostGem;
        public void Set(string teamid, string rentid, int day, int costcoin, int costgem)
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

                if (typeElement == TypeElement.Formation)
                {
                    FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_RentFormation(rentId);
                    Debug.Log("Rent this Formation:" + teamId + "For:" + Day);
                }
            }
            else if (FindObjectOfType<ServerUI>())/// in ui
            {
                if (typeElement == TypeElement.Team)
                {
                    FindObjectOfType<ServerUI>().Emit_RentTeam(rentId);
                    Debug.Log("Rent this Team:" + teamId + "For:" + Day);
                }
                else
                {
                    FindObjectOfType<ServerUI>().Emit_RentFormation(rentId);
                    Debug.Log("Rent this Formation:" + teamId + "For:" + Day);
                }
            }
            
        }
    }
}