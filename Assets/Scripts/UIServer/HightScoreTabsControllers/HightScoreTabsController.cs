using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.HightScoreTab
{


    public class HightScoreTabsController : MonoBehaviour
    {
        public ServerUI Server;
        
        public bool SortByCUP;
        public HightScoreCard CardWithElementCUP;
        public RectTransform Content_CUPTab;
        public bool SortByGEM;
        public HightScoreCard CardWithElementGEM;
        public RectTransform Content_CUPGEM;

        public Toggle FilterByAllContactsToggle;
        public Toggle FilterByFriendsToggle;
        private List<HightScoreCard> temp_CardWithCup;
        private List<HightScoreCard> temp_CardWithGem;
        private void Awake()
        {
            Server.OnGetTopPlayers += Server_OnGetTopPlayers;


            FilterByAllContactsToggle.onValueChanged.AddListener((isOn) => {

                if (isOn)
                {
                    FilterByAllContacts();
                    Debug.Log("Filter By All Contact In HighScore");
                }
            });
            FilterByFriendsToggle.onValueChanged.AddListener((isOn) => {
                if (isOn)
                {
                    FilterByFriends();
                    Debug.Log("Filter By Friends In HighScore");
                }
            });
        }
        public void OnEnable()
        {
            temp_CardWithCup = new List<HightScoreCard>();
            temp_CardWithGem = new List<HightScoreCard>();
          
        }
        public void OnDisable()
        {

            clearCardCup();
            clearCardGem();
         //   Server.OnGetTopPlayers -= Server_OnGetTopPlayers;
        }
        private void Server_OnGetTopPlayers(HTTPBody.TopPlayers players)
        {
            if (SortByCUP)
            {
                initializeTopPlayerWithCUP(players);
            }
            else if (SortByGEM)
            {
                initializeTopPlayerWithGEM(players);
            }
        }

        public void  initializeTopPlayerWithCUP(HTTPBody.TopPlayers players)
        {
            ResetToggel();
            clearCardCup();
            for (int i = 0; i < players.players.Count; i++)
            {
                var card = Instantiate(CardWithElementCUP, Content_CUPTab);
                var image = Server.AvatarContainer.LoadImage(players.players[i].avatar);     
                card.SetCardCUP(players.players[i].rank, image, players.players[i].userName, players.players[i].star.ToString(), players.players[i].cup.ToString());
                temp_CardWithCup.Add(card);
            }
        }
        public void initializeTopPlayerWithGEM(HTTPBody.TopPlayers players)
        {
            ResetToggel();
            clearCardGem();
            for (int i = 0; i < players.players.Count; i++)
            {
                var card = Instantiate(CardWithElementGEM, Content_CUPGEM);
                var image = Server.AvatarContainer.LoadImage(players.players[i].avatar);
                card.SetCardGEM(players.players[i].rank, image, players.players[i].userName, players.players[i].star.ToString(), players.players[i].gem.ToString());
                temp_CardWithGem.Add(card);
            }
        }

        private void FilterByAllContacts()
        {
            if (SortByCUP)
            {
                for (int i = 0; i < temp_CardWithCup.Count; i++)
                {
                   temp_CardWithCup[i].gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < temp_CardWithGem.Count; i++)
                {
                    temp_CardWithGem[i].gameObject.SetActive(true);
                }
            }
        }
        public void ResetToggel()
        {
            FilterByAllContactsToggle.isOn = true;
        }
        private void FilterByFriends()
        {
            if (SortByCUP)
            {
                for (int i = 0;i< temp_CardWithCup.Count; i++)
                {
                    temp_CardWithCup[i].gameObject.SetActive(false);
                    for (int j = 0; j < Server.BODY.social.friends.Count; j++)
                    {
                        if(temp_CardWithCup[i].UserName.text == Server.BODY.social.friends[j].userName)
                        {
                            temp_CardWithCup[i].gameObject.SetActive(true);
                        }
                        

                    }
                }
            }
            else
            {
                for (int i = 0; i < temp_CardWithGem.Count; i++)
                {
                    temp_CardWithGem[i].gameObject.SetActive(false);
                    for (int j = 0; j < Server.BODY.social.friends.Count; j++)
                    {
                        if (temp_CardWithGem[i].UserName.text == Server.BODY.social.friends[j].userName)
                        {
                            temp_CardWithGem[i].gameObject.SetActive(true);
                        }


                    }
                }
            }
        }
        private void clearCardCup()
        {
            if (temp_CardWithCup.Count > 0)
            {
                for (int i = 0; i < temp_CardWithCup.Count; i++)
                {
                    Destroy(temp_CardWithCup[i].gameObject);
                }
                temp_CardWithCup.Clear();
            }
        }
        private void clearCardGem()
        {
            if (temp_CardWithGem.Count > 0)
            {
                for (int i = 0; i < temp_CardWithGem.Count; i++)
                {
                    Destroy(temp_CardWithGem[i].gameObject);
                }
                temp_CardWithGem.Clear();
            }
        }
    }
}