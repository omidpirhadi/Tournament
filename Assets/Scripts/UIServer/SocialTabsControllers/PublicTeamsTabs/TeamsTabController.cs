using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Social
{


    public class TeamsTabController : MonoBehaviour
    {

        public ServerUI Server;
        public Diaco.Social.MessageCards.MessagesTabCardInviteToTeam CardInviteToTeam;
        public NavigationUI navigationUI;
      

        public RectTransform Content;
        public TMPro.TMP_InputField SearchInput;
        public Button SearchButton;

        public Button FilterByGameButton;
        public Button AdvanceSearchButton;
        public Button TabLeague_Button;
        private List<Diaco.Social.MessageCards.MessagesTabCardInviteToTeam> TemplistCard;

        public void OnEnable()
        {
            TemplistCard = new List<MessageCards.MessagesTabCardInviteToTeam>();
            Server.OnGetTeams += Server_OnGetTeams;
            SearchButton.onClick.AddListener(() =>
            {
                Server.SearchLeague(SearchInput.text);

            });
            TabLeague_Button.onClick.AddListener(() => {

                SearchInput.text = "";
                Server.GetLeague();

            });
            Server.GetLeague();
        }
        private void OnDisable()
        {
            Server.OnGetTeams -= Server_OnGetTeams;
            SearchButton.onClick.RemoveAllListeners();
            TabLeague_Button.onClick.RemoveAllListeners();
            ClearListCard();
        }
        private void Server_OnGetTeams(Diaco.HTTPBody.Teams listTeams)
        {
            InitializTeams(listTeams);
           // Debug.Log("Initializ");

        }

        public void InitializTeams(Diaco.HTTPBody.Teams Teams)
        {
            ClearListCard();

            for (int i = 0; i < Teams.teams.Count; i++)
            {
                var card = Instantiate(CardInviteToTeam, Content);
                var image = Server.LeagueFlagsContainer.LoadImage(Teams.teams[i].avatar);
                card.SetCard(
                    (Diaco.Social.MessageCards.MessagesTabCardInviteToTeam.TypeGame)Teams.teams[i].game,
                    image,
                    Teams.teams[i].from,
                    Teams.teams[i].capacity,
                    Teams.teams[i].remainingTime,
                    (Diaco.Social.MessageCards.MessagesTabCardInviteToTeam.TypeCost)Teams.teams[i].costType,
                    Teams.teams[i].cost.ToString(),
                    Teams.teams[i].teamId,
                    () => 
                    {
                       // navigationUI.ShowPopUp("teaminfo");
                        Server.GetLeagueInfo(card.TeamID);

                    }
                    );

                TemplistCard.Add(card);

                // Debug.Log("RunInitializ");
            }
            
        }
        public void ClearListCard()
        {
            for (int i = 0; i < TemplistCard.Count; i++)
            {
                Destroy(TemplistCard[i].gameObject);
            }
            TemplistCard.Clear();
        }
    }
}