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
        public Diaco.ImageContainerTool.ImageContainer ImageCards;
        public Diaco.ImageContainerTool.ImageContainer CostTypeImage_Container;
        public Image Cost_Image;
        public Text Cost_txt;
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

        public void initAward(HTTPBody.AwardsName data)
        {
            ElementOfSlideNumberTurnOn(data);
            PutAwardsInIndicator(data);
        }

        private void PutAwardsInIndicator(HTTPBody.AwardsName data)
        {
            /////AWARD PERSON ONE
            Award1st.Gem.text = Convert.ToString(data.awards1.gem);
            Award1st.Coin.text = Convert.ToString(data.awards1.coin);
            Award1st.Card.text = Convert.ToString(data.awards1.card);
            Award1st.ImageCard.sprite = ImageCards.LoadImage(data.awards1.cardName);
            Award1st.Cup.text = Convert.ToString(data.awards1.cpu);
            Award1st.Xp.text = Convert.ToString(data.awards1.xp);
            /////AWARD PERSON TWO
            Award2nd.Gem.text = Convert.ToString(data.awards2.gem);
            Award2nd.Coin.text = Convert.ToString(data.awards2.coin);
            Award2nd.Card.text = Convert.ToString(data.awards2.card);
            Award2nd.ImageCard.sprite = ImageCards.LoadImage(data.awards2.cardName);
            Award2nd.Cup.text = Convert.ToString(data.awards2.cpu);
            Award2nd.Xp.text = Convert.ToString(data.awards2.xp);
            /////AWARD PERSON THREE
            Award3rd.Gem.text = Convert.ToString(data.awards3.gem);
            Award3rd.Coin.text = Convert.ToString(data.awards3.coin);
            Award3rd.Card.text = Convert.ToString(data.awards3.card);
            Award3rd.ImageCard.sprite = ImageCards.LoadImage(data.awards3.cardName);
            Award3rd.Cup.text = Convert.ToString(data.awards3.cpu);
            Award3rd.Xp.text = Convert.ToString(data.awards3.xp);

            Cost_Image.sprite = CostTypeImage_Container.LoadImage(data.costType.ToString());
            Cost_txt.text = data.cost.ToString();

            Debug.Log(data.awards1.cardName + data.awards2.cardName + data.awards3.cardName);
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
        public Image ImageCard;
        public Text Cup;
        public Text Xp;

    }

}