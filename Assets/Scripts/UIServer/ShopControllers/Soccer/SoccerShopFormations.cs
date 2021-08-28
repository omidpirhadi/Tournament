using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    public class SoccerShopFormations : MonoBehaviour
    {
        // Start is called before the first frame update
        public Diaco.ImageContainerTool.ImageContainer ContainerImages;
        public SoccerShopInUseElement InUseTeamElement;
        public SoccerShopOwnerElement OwnerTeamElement;
        public SoccerShopRentElement RentTeamElement;
        public RectTransform Grid;

        private List<GameObject> ListElementsShop;
        public void initFormationShop(SoccerShopDatas teamsData)
        {
            ListElementsShop = new List<GameObject>();
            var data = teamsData.soccershopteamsData;
            for (int i = 0; i < data.Count; i++)
            {
                var image = ContainerImages.LoadImage(data[i].teamImage);
                var id = data[i].id;
             

                if (data[i].owner)
                {
                    if (data[i].inUse)
                    {

                        InUseTeamElement.SetForFormationElement(id, image, data[i].time);
                    }
                    else
                    {
                        var ownerElement = Instantiate(OwnerTeamElement, Grid);
                        ownerElement.SetForFormationElement(id, image, data[i].time);
                        ListElementsShop.Add(ownerElement.gameObject);
                    }
                }
                else
                {
                    var rentElement = Instantiate(RentTeamElement, Grid);
                    rentElement.SetForFormationElement(id, image, data[i].rentData);
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
}