using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.PopupTournumentRanking
{

    public class PopUpTournumentRanking : MonoBehaviour
    {
        private ServerUI server;
        public string ID;
        public Diaco.ImageContainerTool.ImageContainer imageProfile;
        
        public TournomentType TournomentType;
        public List<PlayerElement> PlayersElement;
        public ElementsAward Award1st;
        public ElementsAward Award2nd;
        public ElementsAward Award3rd;

        public Button btn_Leave;
        private void OnEnable()
        {
            GetComponent<SoundEffectControll>().PlaySoundMenu(0);
            server = FindObjectOfType<ServerUI>();
            server.OnStartTournument += Server_OnStartTournument;
            btn_Leave.onClick.AddListener(() => {
                server.RequestCompetitionCommand(ID, 3);
            });
        }


        private void OnDisable()
        {
            server.OnStartTournument -= Server_OnStartTournument;
            btn_Leave.onClick.RemoveAllListeners();
        }
        private void Server_OnStartTournument(TournomentData data)
        {
            initPopup(data);
        }

        public void initPopup(TournomentData data)
        {
            ID = data.id;
            for (int i = 0; i < data.table.Count; i++)
            {
                PlayersElement[i].Set(
                    imageProfile.LoadImage(data.table[i].avatar),
                    data.table[i].userName);
                    
            }
            PutAwardsInIndicator(data.award);
        }
        private void PutAwardsInIndicator(AwardsData data)
        {
            /////AWARD PERSON ONE
            Award1st.Gem.text = (data.awards1.gem).ToString();
            Award1st.Coin.text = (data.awards1.coin).ToString();
            Award1st.Card.text = (data.awards1.card).ToString();
            Award1st.Ticket.text = (data.awards1.ticket).ToString();
            Award1st.Xp.text = (data.awards1.xp).ToString();
            /////AWARD PERSON TWO
            Award2nd.Gem.text = (data.awards2.gem).ToString();
            Award2nd.Coin.text = (data.awards2.coin).ToString();
            Award2nd.Card.text = (data.awards2.card).ToString();
            Award2nd.Ticket.text = (data.awards2.ticket).ToString();
            Award2nd.Xp.text = (data.awards2.xp).ToString();
            /////AWARD PERSON THREE
            Award3rd.Gem.text = (data.awards3.gem).ToString();
            Award3rd.Coin.text = (data.awards3.coin).ToString();
            Award3rd.Card.text = (data.awards3.card).ToString();
            Award3rd.Ticket.text = (data.awards3.ticket).ToString();
            Award3rd.Xp.text = (data.awards3.xp).ToString();

        }
    }
    public enum TournomentType { Four, Eight,Sixteen}
    [Serializable]
    public struct PlayerElement
    {
        public Image profile;
        public Text name;
       // public Text level;
        public void Set(Sprite P, string N/*, string L*/)
        {
            profile.sprite = P;
            name.text = N;
            //level.text = L;
        }
    }
    [Serializable]
    public struct PlayerInTournument
    {
        public string avatar;
        public string userName;
       
    }
    [Serializable]
    public struct TournomentData
    {

        public int capacity;
        public string id;
        public List<PlayerInTournument> table;
        public AwardsData award;

    }

    [Serializable]
    public struct ElementsAward
    {
        public Text Gem;
        public Text Coin;
        public Text Card;
        public Text Ticket;
        public Text Xp;

    }
    [Serializable]
    public struct Award
    {
        public int gem;
        public int coin;
        public int card;
        public int ticket;
        public int xp;
    }
    [Serializable]
    public struct AwardsData
    {
       // public int capacity;
        //public int active;
        public Award awards1;
        public Award awards2;
        public Award awards3;


    }
}