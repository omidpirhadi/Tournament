using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.RoyalTournument
{

    
    
    public class CompetitionsPopUp : MonoBehaviour
    {
        public ServerUI server;
        public NavigationUI ui;
        public Button btn_Champion;
        public MatchCard ChampionCard;

        public Button btn_Epic;
        public MatchCard EpicCard;

        public Button btn_Royal;
        public MatchCard RoyalCard;

        public RectTransform Contact;

        public Button Exit_button;
        //public UnityEngine.Events.UnityEvent OnEnableAction;
        [SerializeField] private List<GameObject> ListMatchCard;


        private void OnEnable()
        {

            ListMatchCard = new List<GameObject>();
            server.OnReciveMatchs += Server_OnReciveMatchs;
            btn_Champion.onClick.AddListener(() => {
                server.RequestGetMatch( "champion");
            });
            btn_Epic.onClick.AddListener(() => {
                server.RequestGetMatch("epic");
            });
            btn_Royal.onClick.AddListener(() => {
                server.RequestGetMatch("royal");
            });
            Exit_button.onClick.AddListener(() => {
                ui.SwitchUI(ui.CurrentPage);
                Debug.Log(ui.CurrentPage);
            });
            server.RequestGetMatch("champion");////For Load Defult Page
            /// OnEnableAction.Invoke();
        }

       

        private void OnDisable()
        {
            btn_Champion.onClick.RemoveAllListeners();
            btn_Epic.onClick.RemoveAllListeners();
            btn_Royal.onClick.RemoveAllListeners();
            Clear();
        }
        private void Server_OnReciveMatchs(CompetitionsData data)
        {
            InitMatch(data);
        }
        private void InitMatch(CompetitionsData data)
        {
            Clear();
            for (int i = 0; i < data.competitions.Count; i++)
            {
                if (data.type == "champion")
                {
                    var card = Instantiate(ChampionCard, Contact);
                    card.Set(data.competitions[i].id, data.competitions[i].costType, data.competitions[i].cost, data.competitions[i].capacity, data.competitions[i].time);
                    ListMatchCard.Add(card.gameObject);
                }
                else if (data.type == "epic")
                {
                    var card = Instantiate(EpicCard, Contact);
                    card.Set(data.competitions[i].id, data.competitions[i].costType, data.competitions[i].cost, data.competitions[i].capacity, data.competitions[i].time);
                    ListMatchCard.Add(card.gameObject);
                }
                else if (data.type == "royal")
                {
                    var card = Instantiate(RoyalCard, Contact);
                    card.Set(data.competitions[i].id, data.competitions[i].costType, data.competitions[i].cost, data.competitions[i].capacity, data.competitions[i].time);
                    ListMatchCard.Add(card.gameObject);
                }
            }
            
        }
        private void Clear()
        {
            for (int i = 0; i < ListMatchCard.Count; i++)
            {
                Destroy(ListMatchCard[i].gameObject);
            }
            ListMatchCard.Clear();
        }
    }
    [Serializable]
    public struct Competition
    {
        public string id;
        public int costType; ///0 coin 1 gem
        public int  cost;
        public string capacity;
        public int time;
    }
    [Serializable]
    public struct CompetitionsData
    {
        public string type;//champion,epic,royal 
        public List<Competition> competitions;
    }
}