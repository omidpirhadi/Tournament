using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
namespace Diaco.UI
{
    public class UIRegisterOnServer : MonoBehaviour
    {
        [FoldoutGroup("HeaderUIs")]
        public Image ImageUser_inPageSelectGame;
        [FoldoutGroup("HeaderUIs")]
        public RTLTMPro.RTLTextMeshPro UserName_inPageSelectGame;
        [FoldoutGroup("HeaderUIs")]
        public Text CurrentXpLevel_text;
        [FoldoutGroup("HeaderUIs")]
        [SerializeField] public Prograssbar XpPrograssbar;
        [FoldoutGroup("HeaderUIs")]
        public Text Cupbilliard_inPageSelectGame;
        [FoldoutGroup("HeaderUIs")]
        public Text Cupsoccer_inPageSelectGame;
        [FoldoutGroup("HeaderUIs")]
        public Text Coin_inPageSelectGame;
        [FoldoutGroup("HeaderUIs")]
        public Text Gem_inPageSelectGame;
        [FoldoutGroup("FooterUIs")]
        public List<GameObject> IdleTournuments;
        [FoldoutGroup("FooterUIs")]
        public List<Diaco.UI.TournumentCard.TournumentCard> TournumentCards;

        public void initTournmentCard(List<Diaco.HTTPBody.TOURNOMENTS> data)
        {
            SetIdleTounumentCard();
            if (data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    TournumentCards[i].gameObject.SetActive(true);
                    TournumentCards[i].Set(data[i].id, data[i].name, data[i].time, data[i].type);

                }
            }
        }

        public void SetXpPrograssBar(int currentxplevel,  int currentvalue, int totalvalue)
        {
            float step = 1.0f / totalvalue;
            XpPrograssbar.TargetGraphic.fillAmount = 0.0f;
            CurrentXpLevel_text.text = currentxplevel.ToString();
            XpPrograssbar.TargetGraphic.DOFillAmount(currentvalue * step, 0.5f);
            XpPrograssbar.TextPrograss.text = currentvalue + "/" + totalvalue;  
        }
        public  void ResetXpPrograssbar()
        {
            CurrentXpLevel_text.text = "";
            XpPrograssbar.TargetGraphic.fillAmount = 1f;
            XpPrograssbar.TextPrograss.text = "";
        }
        private void SetIdleTounumentCard()
        {
            for (int i = 0; i < IdleTournuments.Count; i++)
            {
                IdleTournuments[i].SetActive(true);
                TournumentCards[i].gameObject.SetActive(false);
            }
        }


        
    }
    [Serializable]
    public struct Prograssbar
    {
        public Image TargetGraphic;
        public Text TextPrograss;
    }
}