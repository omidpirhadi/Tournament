using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.PopupAwardLeague
{
    public class PopUpAwardLeague : MonoBehaviour
    {
        public Diaco.ImageContainerTool.ImageContainer Avatars;
        public Diaco.ImageContainerTool.ImageContainer ImageCards;
        [SerializeField]
        public ElementsAward ElementsPopup;
        public Button AcceptButton;
        public void Set(AwardData data)
        {
            ElementsPopup.Avatar.sprite = Avatars.LoadImage(data.avatar);
            ElementsPopup.UserName.text = data.userName;
            ElementsPopup.Rank.text = (data.rank).ToString() ;
            ElementsPopup.Gem.text = (data.gem).ToString();
            ElementsPopup.Coin.text = (data.coin).ToString();

            ElementsPopup.Card.text = (data.card).ToString();
            ElementsPopup.ImageCard.sprite = ImageCards.LoadImage(data.cardName);

            ElementsPopup.Cup.text = (data.cup).ToString();
            ElementsPopup.Xp.text = (data.xp).ToString();
            AcceptButton.onClick.AddListener(() => {

                
                var pageranking = FindObjectOfType<Diaco.UI.PopupTournumentRanking.PopUpTournumentRanking>();
                if(pageranking)
                {
                    pageranking.gameObject.SetActive(false);
                }
                this.gameObject.SetActive(false);

            });
        }
        private void OnDisable()
        {
            AcceptButton.onClick.RemoveAllListeners();
        }
        
    }
    [Serializable]
    public struct ElementsAward
    {
        public Image Avatar;
        public RTLTMPro.RTLTextMeshPro UserName;
        public Text Rank;
        public Text Gem;
        public Text Coin;
        public Text Card;
        public Image ImageCard;
        public Text Cup;
        public Text Xp;

    }
    [Serializable]
    public struct AwardData
    {
        public string avatar;
        public string userName;
        public int rank;
        public int gem;
        public int coin;
        public int card;
        public string cardName;
        public int cup;
        public int xp;
    }
}