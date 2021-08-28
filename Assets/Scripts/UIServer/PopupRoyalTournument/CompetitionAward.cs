using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.RoyalTournument
{

    public class CompetitionAward : MonoBehaviour
    {

        // public Diaco.HTTPBody.AwardsName awardsname;
        // public int Capacity = 0;

        public GameObject SlideNumber_8;
        public List<Toggle> Element_SlideNumber_8;
        public GameObject SlideNumber_16;
        public List<Toggle> Element_SlideNumber_16;

        public ElementsAward Award1st;
        public ElementsAward Award2nd;
        public ElementsAward Award3rd;

        private ServerUI server;
        private void OnEnable()
        {
            server = FindObjectOfType<ServerUI>();
            server.OnCompetitionAward += Server_OnCompetitionAward;
            Debug.Log("X1");
        }

        private void Server_OnCompetitionAward(AwardsData data)
        {
            PutAwardsInIndicator(data);
            ElementOfSlideNumberTurnOn(data);
            Debug.Log("X2");
        }

        private void OnDisable()
        {
            server.OnCompetitionAward -= Server_OnCompetitionAward;
        }
        private void PutAwardsInIndicator(AwardsData data)
        {
            /////AWARD PERSON ONE
            Award1st.Gem.text = (data.awards1.gem).ToString();
            Award1st.Coin.text = (data.awards1.coin).ToString();
            Award1st.Card.text = (data.awards1.card).ToString();
            Award1st.Ticket.text = (data.awards1.ticket).ToString();
            Award1st.Xp.text = (data.awards1.xp).ToString();
            /////AWARD PERSON TWO
            Award2nd.Gem.text = (data.awards2.gem).ToString();
            Award2nd.Coin.text = (data.awards2.coin).ToString();
            Award2nd.Card.text = (data.awards2.card).ToString();
            Award2nd.Ticket.text = (data.awards2.ticket).ToString();
            Award2nd.Xp.text = (data.awards2.xp).ToString();
            /////AWARD PERSON THREE
            Award3rd.Gem.text = (data.awards3.gem).ToString();
            Award3rd.Coin.text = (data.awards3.coin).ToString();
            Award3rd.Card.text = (data.awards3.card).ToString();
            Award3rd.Ticket.text = (data.awards3.ticket).ToString();
            Award3rd.Xp.text = (data.awards3.xp).ToString();
          
        }

        private void ElementOfSlideNumberTurnOn(AwardsData data)
        {
            if (data.capacity == 4)
            {
                SlideNumber_8.SetActive(false);
                SlideNumber_16.SetActive(false);


            }
            else if (data.capacity == 8)
            {
                SlideNumber_8.SetActive(true);
                SlideNumber_16.SetActive(false);
            }
            else if (data.capacity == 16)
            {
                SlideNumber_8.SetActive(false);
                SlideNumber_16.SetActive(true);
            }

            if (data.capacity == 16)
            {
                Element_SlideNumber_16[data.active].isOn = true;
            }
            else if (data.capacity == 8)
            {

                Element_SlideNumber_8[data.active].isOn = true;
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
    [Serializable]
    public struct Award
    {
        public int gem;
        public int coin;
        public int card;
        public int ticket;
        public int xp;
    }
    [Serializable]
    public struct AwardsData
    {
        public int capacity;
        public int active;
        public Award awards1;
        public Award awards2;
        public Award awards3;


    }
}