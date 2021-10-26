using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{

    public enum TypeElement { Team, Formation }
    public class SoccerShopFormations : MonoBehaviour
    {
        public bool InGame = false;
        public Diaco.ImageContainerTool.ImageContainer ContainerImages;
        public SoccerShopInUseElement InUseFormationElement;
        public SoccerShopOwnerElement OwnerFormationElement;
        public SoccerShopRentElement RentFormationElement;
        public RectTransform Grid;

        private List<GameObject> ListElementsShop;

        public void OnEnable()
        {
            ListElementsShop = new List<GameObject>();
            if (!InGame)
            {

            }
            else
            {

                FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().InitShop += Shopformation_InitShop;
                FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_Shopformation();

            }

        }
        public void OnDisable()
        {
            if (InGame)
            {
                FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().InitShop -= Shopformation_InitShop;
                ClearShop();
            }
        }
        private void Shopformation_InitShop(SoccerShopDatas data)
        {
            initFormationShop(data);

        }
        public void initFormationShop(SoccerShopDatas Data)
        {
            if (ListElementsShop.Count > 0)
                ClearShop();
            ListElementsShop = new List<GameObject>();
            var data = Data.soccershopteamsData;
            for (int i = 0; i < data.Count; i++)
            {
                Debug.Log(data[i].teamImage);
                var image = ContainerImages.LoadImage(data[i].teamImage);
                var id = data[i].id;
             

                if (data[i].owner)
                {
                    if (data[i].inUse)
                    {

                        InUseFormationElement.SetForFormationElement(id, image, data[i].time);
                    }
                    else
                    {
                        var ownerElement = Instantiate(OwnerFormationElement, Grid);
                        ownerElement.SetForFormationElement(id, image, data[i].time);
                        ListElementsShop.Add(ownerElement.gameObject);
                    }
                }
                else
                {
                    var rentElement = Instantiate(RentFormationElement, Grid);
                    rentElement.SetForFormationElement(id, image, data[i].rentData);
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
            Debug.Log("ClearShopBiliard");
        }
    }
}