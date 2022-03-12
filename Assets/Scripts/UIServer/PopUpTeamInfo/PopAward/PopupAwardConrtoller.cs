using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.TeamInfo
{
    public class PopupAwardConrtoller : MonoBehaviour
    {
        public ServerUI Server;
        public string TeamTag = "";
       // public Diaco.HTTPBody.AwardsName awardsname;
       // public int Capacity = 0;

        public GameObject SlideNumber_8;
        public List<Toggle> Element_SlideNumber_8;
        public GameObject SlideNumber_16;
        public List<Toggle> Element_SlideNumber_16;

        public ElementsAward Award1st;
        public ElementsAward Award2nd;
        public ElementsAward Award3rd;

        private void OnEnable()
        {
           // Server.GetAwardsTeam(TeamTag);
           /// Server.OnGetAward += Server_OnGetAward;
        }

        public void initAward(HTTPBody.AwardsName AwardName)
        {
            ElementOfSlideNumberTurnOn(AwardName);
            PutAwardsInIndicator(AwardName);
        }

        private void PutAwardsInIndicator(HTTPBody.AwardsName awardsname)
        {
            /////AWARD PERSON ONE
            Award1st.Gem.text = Convert.ToString(awardsname.awards1.gem);
            Award1st.Coin.text = Convert.ToString(awardsname.awards1.coin);
            Award1st.Card.text = Convert.ToString(awardsname.awards1.card);
            Award1st.Ticket.text = Convert.ToString(awardsname.awards1.ticket);
            Award1st.Xp.text = Convert.ToString(awardsname.awards1.xp);
            /////AWARD PERSON TWO
            Award2nd.Gem.text = Convert.ToString(awardsname.awards2.gem);
            Award2nd.Coin.text = Convert.ToString(awardsname.awards2.coin);
            Award2nd.Card.text = Convert.ToString(awardsname.awards2.card);
            Award2nd.Ticket.text = Convert.ToString(awardsname.awards2.ticket);
            Award2nd.Xp.text = Convert.ToString(awardsname.awards2.xp);
            /////AWARD PERSON THREE
            Award3rd.Gem.text = Convert.ToString(awardsname.awards3.gem);
            Award3rd.Coin.text = Convert.ToString(awardsname.awards3.coin);
            Award3rd.Card.text = Convert.ToString(awardsname.awards3.card);
            Award3rd.Ticket.text = Convert.ToString(awardsname.awards3.ticket);
            Award3rd.Xp.text = Convert.ToString(awardsname.awards3.xp);
        }

        private void ElementOfSlideNumberTurnOn(HTTPBody.AwardsName awardsname)
        {
            if (awardsname.capacity == 4)
            {
                SlideNumber_8.SetActive(false);
                SlideNumber_16.SetActive(false);


            }
            else if (awardsname.capacity == 8)
            {
                SlideNumber_8.SetActive(true);
                SlideNumber_16.SetActive(false);
            }
            else if (awardsname.capacity == 16)
            {
                SlideNumber_8.SetActive(false);
                SlideNumber_16.SetActive(true);
            }

            if (awardsname.capacity == 16)
            {
                Element_SlideNumber_16[awardsname.active].isOn = true;
            }
            else if (awardsname.capacity == 8)
            {

                Element_SlideNumber_8[awardsname.active].isOn = true;
            }


        }
    }
    [Serializable]
    public struct ElementsAward
    {
        public Text Gem;
        public Text Coin;
        public Text Card;
        public Text Ticket;
        public Text Xp;

    }

}