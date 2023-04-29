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
        public Diaco.ImageContainerTool.ImageContainer ImageCards;
        public Diaco.ImageContainerTool.ImageContainer CostTypeImage_Container;
        public Image Cost_Image;
        public Text Cost_txt;
        public ElementsAward Award1st;
        public ElementsAward Award2nd;
        public ElementsAward Award3rd;

        private ServerUI server;
        private void OnEnable()
        {
            server = FindObjectOfType<ServerUI>();
            server.OnCompetitionAward += Server_OnCompetitionAward;
         //   Debug.Log("X1");
        }

        private void Server_OnCompetitionAward(AwardsData data)
        {
            PutAwardsInIndicator(data);
            ElementOfSlideNumberTurnOn(data);
           // Debug.Log("X2");
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
            Award1st.ImageCard.sprite = ImageCards.LoadImage(data.awards1.cardName);

            Award1st.Cup.text = (data.awards1.cup).ToString();
            Award1st.Xp.text = (data.awards1.xp).ToString();
            /////AWARD PERSON TWO
            Award2nd.Gem.text = (data.awards2.gem).ToString();
            Award2nd.Coin.text = (data.awards2.coin).ToString();

            Award2nd.Card.text = (data.awards2.card).ToString();
            Award2nd.ImageCard.sprite = ImageCards.LoadImage(data.awards2.cardName);

            Award2nd.Cup.text = (data.awards2.cup).ToString();
            Award2nd.Xp.text = (data.awards2.xp).ToString();
            /////AWARD PERSON THREE
            Award3rd.Gem.text = (data.awards3.gem).ToString();

            Award3rd.Coin.text = (data.awards3.coin).ToString();

            Award3rd.Card.text = (data.awards3.card).ToString();
            Award3rd.ImageCard.sprite = ImageCards.LoadImage(data.awards3.cardName);

            Award3rd.Cup.text = (data.awards3.cup).ToString();
            Award3rd.Xp.text = (data.awards3.xp).ToString();

            Cost_Image.sprite = CostTypeImage_Container.LoadImage(data.costType.ToString());
            Cost_txt.text = data.cost.ToString();
           
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
        public Image ImageCard;
        public Text Cup;
        public Text Xp;

    }
    [Serializable]
    public struct Award
    {
        public int gem;
        public int coin;
        public string cardName;
        public int card;
        public int cup;
        public int xp;
    }
    [Serializable]
    public struct AwardsData
    {
        public int capacity;
        public int active;
        public int costType;
        public int cost;
        public Award awards1;
        public Award awards2;
        public Award awards3;


    }
}