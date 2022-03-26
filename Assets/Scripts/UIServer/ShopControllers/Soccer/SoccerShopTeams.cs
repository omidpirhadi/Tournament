using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    public class SoccerShopTeams : MonoBehaviour
    {
        public bool InGame = false;
        public Diaco.ImageContainerTool.ImageContainer ContainerImages;
        public SoccerShopInUseElement InUseTeamElement;
        public SoccerShopOwnerElement OwnerTeamElement;
        public SoccerShopRentElement RentTeamElement;
        public RectTransform Grid;

       [SerializeField] private List<GameObject> ListElementsShop;

        public void OnEnable()
        {
            ListElementsShop = new List<GameObject>();
            if (!InGame)
            {
                FindObjectOfType<ServerUI>().InitSoccerTeamShop += ShopTeam_InitShop;
                FindObjectOfType<ServerUI>().Emit_SoccerShopTeam();
            }
            

        }
        public void OnDisable()
        {
           
            if(!InGame)
            {
                FindObjectOfType<ServerUI>().InitSoccerFormationShop -= ShopTeam_InitShop;
                ClearShop();
            }
        }
        private void ShopTeam_InitShop(SoccerShopDatas data)
        {
            initTeamsShop(data);

        }
        public void initTeamsShop(SoccerShopDatas teamsData)
        {
            if (ListElementsShop.Count > 0)
                ClearShop();
            ListElementsShop = new List<GameObject>();
            var data = teamsData.soccershopteamsData;
            for (int i = 0; i < data.Count; i++)
            {
               // Debug.Log("ShopTeam 1");
                var image = ContainerImages.LoadImage(data[i].teamImage);
                var id = data[i].id;
                var force = data[i].force;
                var aim = data[i].aim;

                if (data[i].owner)
                {
                    if(data[i].inUse)
                    {

                        InUseTeamElement.SetForTeamElement(id, image, force, aim, data[i].time);
                     //   Debug.Log("ShopTeam 2");
                    }
                    else
                    {
                        var ownerElement = Instantiate(OwnerTeamElement, Grid);
                        ownerElement.SetForTeamElement(id, image, force, aim, data[i].time);
                        ListElementsShop.Add(ownerElement.gameObject);
                     //   Debug.Log("ShopTeam 3");
                    }
                   // Debug.Log("ShopTeam 4");
                }
                else
                {
                    var rentElement = Instantiate(RentTeamElement, Grid);
                    rentElement.SetForTeamElement(id, image, force, aim,data[i].rentData);
                    ListElementsShop.Add(rentElement.gameObject);
                //    Debug.Log("ShopTeam 5");
                }
                
            }
            Debug.Log("ShopTeam Show");
        }

        private void ClearShop()
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
        public string rentId;
        public int typeOption;//// 0 Coin,1 CoinWithGem
        public int day;
        public int coin;
        public int gem;
    }
    [Serializable]
    public struct SoccerShopData
    {
        public string id;
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