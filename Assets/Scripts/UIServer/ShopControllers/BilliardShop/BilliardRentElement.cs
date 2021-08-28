using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Billiard
{
    public class BilliardRentElement : MonoBehaviour
    {
        public int ID;
        public Image CueImage;
        public Image PrograssBarForce;
        public Image PrograssBarAim;
        public Image PrograssBarSpin;
        public RectTransform RentPanel;
        public BilliardShopRentCoinOption RentCoinOption;
        public BilliardShopRentCoinAndGemOption RentCoinAndGemOption;

        public Button Btn_ShowRentPanel;

        public void Set(int id, Sprite teamImage, float force, float aim,float spin, List<BilliardShopRentOptionData> rentOptionDatas)
        {
            ID = id;
            CueImage.sprite = teamImage;
            PrograssBarForce.fillAmount = force;
            PrograssBarAim.fillAmount = aim;
            PrograssBarSpin.fillAmount = spin;
            CreateRentOption(rentOptionDatas);
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
        public void CreateRentOption(List<BilliardShopRentOptionData> rentOptionData)
        {
            for (int i = 0; i < rentOptionData.Count; i++)
            {
                if (rentOptionData[i].typeOption == 0)
                {
                    var rentoption = Instantiate(RentCoinOption, RentPanel);
                    rentoption.Set(ID, rentOptionData[i].rentId, rentOptionData[i].day, rentOptionData[i].coin);
                }
                else
                {
                    var rentoption2 = Instantiate(RentCoinAndGemOption, RentPanel);
                    rentoption2.Set(ID, rentOptionData[i].rentId, rentOptionData[i].day, rentOptionData[i].coin, rentOptionData[i].gem);
                }
            }
        }

        private void UseButtonClick()
        {
            Btn_ShowRentPanel.gameObject.SetActive(false);
            RentPanel.gameObject.SetActive(true);
            Debug.Log("Show Rent Panel");

        }
    }
}