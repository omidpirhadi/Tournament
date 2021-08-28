using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Diaco.Store.Billiard
{
    public class BilliardShop : MonoBehaviour
    {
        public Diaco.ImageContainerTool.ImageContainer ContainerImages;
        public BilliardInuseElement InUseTeamElement;
        public BilliardOwnerElement OwnerTeamElement;
        public BilliardRentElement RentTeamElement;
        public RectTransform Grid;

        private List<GameObject> ListElementsShop;
        public void initTeamsShop(BilliardShopDatas shopData)
        {
            ListElementsShop = new List<GameObject>();
            var data = shopData.billiardshopteamsData;
            for (int i = 0; i < data.Count; i++)
            {
                var image = ContainerImages.LoadImage(data[i].teamImage);
                var id = data[i].id;
                var force = data[i].force;
                var aim = data[i].aim;
                var spin = data[i].spin;

                if (data[i].owner)
                {
                    if (data[i].inUse)
                    {

                        InUseTeamElement.Set(id, image, force, aim,spin, data[i].time);
                    }
                    else
                    {
                        var ownerElement = Instantiate(OwnerTeamElement, Grid);
                        ownerElement.Set(id, image, force, aim, spin,data[i].time);
                        ListElementsShop.Add(ownerElement.gameObject);
                    }
                }
                else
                {
                    var rentElement = Instantiate(RentTeamElement, Grid);
                    rentElement.Set(id, image, force, aim,spin, data[i].rentData);
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
    public struct BilliardShopRentOptionData
    {
        public int rentId;
        public int typeOption;//// 0 Coin,1 CoinWithGem
        public int day;
        public int coin;
        public int gem;
    }
    [Serializable]
    public struct BilliardShopData
    {
        public int id;
        public bool owner;
        public bool inUse;
        public List<BilliardShopRentOptionData> rentData;
        public string teamImage;
        public float force;
        public float aim;
        public float spin;
        public string time;
    }
    [Serializable]
    public struct BilliardShopDatas
    {
        public List<BilliardShopData> billiardshopteamsData;
    }
}