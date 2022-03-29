using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Diaco.Store.Billiard
{
    public class BilliardShop : MonoBehaviour
    {
        public bool InGame = false;


        public Diaco.ImageContainerTool.ImageContainer ContainerImages;
        public BilliardInuseElement InUseTeamElement;
        public BilliardOwnerElement OwnerTeamElement;
        public BilliardRentElement RentTeamElement;
        public RectTransform Grid;

     [SerializeField]   private List<GameObject> ListElementsShop;

        public void OnEnable()
        {
            ListElementsShop = new List<GameObject>();
            if (!InGame)
            {
                FindObjectOfType<ServerUI>().InitshopBilliard += BilliardShop_InitShop;
                FindObjectOfType<ServerUI>().Emit_BilliardShop();
            }
            else
            {
              
                    FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().InitShop += BilliardShop_InitShop;
                    FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().Emit_Shop();

            }
          
        }
        public void OnDisable()
        {
            if (InGame)
            {
                FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().InitShop -= BilliardShop_InitShop;
                ClearShop();
            }
            else if(!InGame)
            {
                FindObjectOfType<ServerUI>().InitshopBilliard -= BilliardShop_InitShop;
                ClearShop();
            }
        }
        public void OnDestroy()
        {
            if (InGame)
            {
                FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().InitShop -= BilliardShop_InitShop;
                ClearShop();
            }
            else if (!InGame)
            {
                FindObjectOfType<ServerUI>().InitshopBilliard -= BilliardShop_InitShop;
                ClearShop();
            }
        }
        private void BilliardShop_InitShop(BilliardShopDatas data)
        {
            initShop(data);
            
        }

        public void initShop(BilliardShopDatas shopData)
        {
            ///if (ListElementsShop.Count > 0)
                ClearShop();
            
            
            var data = shopData.billiardshopteamsData;
            for (int i = 0; i < data.Count; i++)
            {
                var image = ContainerImages.LoadImage(data[i].cueImage);
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
                        ownerElement.Set(id, image, force, aim, spin,data[i].time,InGame);
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
            Debug.Log("ShopLoaded");
        }

        private void ClearShop()
        {
            for (int i = 0; i < ListElementsShop.Count; i++)
            {
                Destroy(ListElementsShop[i]);
            }
            ListElementsShop.Clear();
            Debug.Log("ClearShopBiliard");
        }
    }



    [Serializable]
    public struct BilliardShopDatas
    {
        public List<BilliardShopData> billiardshopteamsData;
    }
    [Serializable]
    public struct BilliardShopData
    {
        public string id;
        public bool owner;
        public bool inUse;
        public List<BilliardShopRentOptionData> rentData;
        public string cueImage;
        public float force;
        public float aim;
        public float spin;
        public string time;
    }
    [Serializable]
    public struct BilliardShopRentOptionData
    {
        public string rentId;
        public int typeOption;//// 0 Coin,1 CoinWithGem
        public int day;//10-20-30
        public int coin;
        public int gem;
    }
}