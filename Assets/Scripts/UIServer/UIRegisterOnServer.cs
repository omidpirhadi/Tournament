using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class UIRegisterOnServer : MonoBehaviour
{
    [FoldoutGroup("HeaderUIs")]
    public Image   ImageUser_inPageSelectGame;
    [FoldoutGroup("HeaderUIs")]
    public Text UserName_inPageSelectGame;
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

    public void initTournmentCard(List< Diaco.HTTPBody.TOURNOMENTS> data)
    {
        SetIdleTounumentCard();
        for(int i = 0; i<data.Count; i++)
        {
            TournumentCards[i].gameObject.SetActive(true);
            TournumentCards[i].Set(data[i].id, data[i].name, data[i].time, data[i].type);
            
        }
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
