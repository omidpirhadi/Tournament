using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    [RequireComponent(typeof(Button))]
    public class SoccerShopRentCoinOption : MonoBehaviour
    {
        public int teamId;
        public int rentId;
        public TypeElement typeElement;

        public Text Day;
        public Text Cost;
        public void Set(int teamid, int rentid, int day, int cost)
        {
            teamId = teamid;
            rentId = rentid;
            Day.text = day.ToString();
            Cost.text = cost.ToString();
            GetComponent<Button>().onClick.AddListener(onClick);
        }
        private void onClick()
        {
            if (FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>())/// in game
            {
                if(typeElement == TypeElement.Team)
                {
                    //// do nothing 
                }
                else
                {
                    FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_RentFormation(teamId, rentId);
                }
            }
            else if (FindObjectOfType<ServerUI>())/// in ui
            {
                if (typeElement == TypeElement.Team)
                {

                }
                else
                {

                }
            }
        } 
    }
}