using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    public class SoccerShopTeams : MonoBehaviour
    {
        public Diaco.ImageContainerTool.ImageContainer ContainerImages;
        public SoccerShopInUseElement InUseTeamElement;
        public SoccerShopOwnerElement OwnerTeamElement;
        public SoccerShopRentElement RentTeamElement;
        public RectTransform Grid;

        private List<GameObject> ListElementsShop;
        public void initTeamsShop(SoccerShopDatas teamsData)
        {
            ListElementsShop = new List<GameObject>();
            var data = teamsData.soccershopteamsData;
            for (int i = 0; i < data.Count; i++)
            {
                var image = ContainerImages.LoadImage(data[i].teamImage);
                var id = data[i].id;
                var force = data[i].force;
                var aim = data[i].aim;

                if (data[i].owner)
                {
                    if(data[i].inUse)
                    {

                        InUseTeamElement.SetForTeamElement(id, image, force, aim, data[i].time);
                    }
                    else
                    {
                        var ownerElement = Instantiate(OwnerTeamElement, Grid);
                        ownerElement.SetForTeamElement(id, image, force, aim, data[i].time);
                        ListElementsShop.Add(ownerElement.gameObject);
                    }
                }
                else
                {
                    var rentElement = Instantiate(RentTeamElement, Grid);
                    rentElement.SetForTeamElement(id, image, force, aim,data[i].rentData);
                    ListElementsShop.Add(rentElement.gameObject);

                }
                
            }
        }

        private void ClearShopTeams()
        {
            for (int i = 0; i < ListElementsShop.Count; i++)
            {
                Destroy(ListElementsShop[i]);
            }
        }
        
    }

    [Serializable]
     public struct  SoccerShopRentOptionData
    {
        public int rentId;
        public int typeOption;//// 0 Coin,1 CoinWithGem
        public int day;
        public int coin;
        public int gem;
    }
    [Serializable]
    public struct SoccerShopData
    {
        public int id;
        public bool owner;
        public bool inUse;
        public List<SoccerShopRentOptionData> rentData;
        public string teamImage;
        public float force;
        public float aim;
        public string time;
    }
    [Serializable]
    public struct SoccerShopDatas
    {
        public List<SoccerShopData> soccershopteamsData;
    }
}