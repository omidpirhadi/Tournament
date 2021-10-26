using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
namespace Diaco.Store.Soccer
{
    public class SoccerShopRentElement : MonoBehaviour
    {

        public string ID;
        public Image TeamImage;
        public Image PrograssBarForce;
        public Image PrograssBarAim;
        public RectTransform RentPanel;
        public RectTransform InfoPanel;
        public SoccerShopRentCoinOption RentCoinOption;
        public SoccerShopRentCoinAndGemOption RentCoinAndGemOption;

        public Button Btn_ShowRentPanel;

        public void SetForTeamElement(string  id, Sprite teamImage, float force, float aim , List<SoccerShopRentOptionData> rentOptionDatas )
        {
            ID = id;
            TeamImage.sprite = teamImage;
            PrograssBarForce.fillAmount = force;
            PrograssBarAim.fillAmount = aim;
            CreateRentOption(rentOptionDatas, TypeElement.Team);
            Btn_ShowRentPanel.onClick.AddListener(UseButtonClick);
        }
        public void SetForFormationElement(string id, Sprite teamImage , List<SoccerShopRentOptionData> rentOptionDatas)
        {
            ID = id;
            TeamImage.sprite = teamImage;

            CreateRentOption(rentOptionDatas, TypeElement.Formation);
            Btn_ShowRentPanel.onClick.AddListener(UseButtonClick);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeOption"> 0 = RentWithCoin, 1 =  RentWithCoinAndGem </param>
        /// <param name="rentid">RentID</param>
        /// <param name="day"> Day</param>
        /// <param name="costcoin">Cost</param>
        /// <param name="costgem">Cost</param>
        public void CreateRentOption(List<SoccerShopRentOptionData>  rentOptionData , TypeElement typeElement )
        {
            for (int i = 0; i < rentOptionData.Count; i++)
            {
                if (rentOptionData[i].typeOption == 0)
                {
                    var rentoption = Instantiate(RentCoinOption, RentPanel);
                    rentoption.typeElement = typeElement;
                    rentoption.Set(ID, rentOptionData[i].rentId, rentOptionData[i].day, rentOptionData[i].coin);
                    

                }
                else
                {
                    var rentoption2 = Instantiate(RentCoinAndGemOption, RentPanel);

                    rentoption2.typeElement = typeElement;
                    rentoption2.Set(ID, rentOptionData[i].rentId, rentOptionData[i].day, rentOptionData[i].coin, rentOptionData[i].gem);
                    
                }
            }
        }

        private void UseButtonClick()
        {
            Btn_ShowRentPanel.gameObject.SetActive(false);
            InfoPanel.gameObject.SetActive(false);
            RentPanel.gameObject.SetActive(true);
            Debug.Log("Show Rent Panel");

        }
    }
}