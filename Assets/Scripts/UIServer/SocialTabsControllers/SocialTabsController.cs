using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.SocialTabs
{

    public class SocialTabsController : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI navigationUi;
        public TabPageController SocialTabs;
        public GameObject MakeTeamTab, TeamsTab, ChatsTeamTab, StartGameTimerTab;

        private Button Social_btn;
        private bool onceShowHaveTeam = false;
        private bool onceShowDontHaveTeam = false;
        private void Start()
        {
          /*  Social_btn = GetComponent<Button>();
            Server.OnCreateTeamCompeleted += Server_OnCreateTeamCompeleted;
            Social_btn.onClick.AddListener(() =>
            {


                if (Server.BODY.social.team.teamId == "")
                {
                    MakeTeamTab.SetActive(true);
                    TeamsTab.SetActive(true);
                    ChatsTeamTab.SetActive(false);
                    StartGameTimerTab.SetActive(false);
                    SocialTabs.ShowTabsDefalt =0;
                    Debug.Log("HAVE NOT A TEAM" + Server.BODY.social.team.teamId.Length);
                }
                else
                {
                    ChatsTeamTab.SetActive(true);
                    StartGameTimerTab.SetActive(true);
                    MakeTeamTab.SetActive(false);
                    TeamsTab.SetActive(false);
                    SocialTabs.ShowTabsDefalt = 4;
                    Debug.Log(" HAVE A TEAM" + Server.BODY.social.team.teamId.Length);
                }
                navigationUi.SwitchUI("social");
                navigationUi.ColorButtonFix(Social_btn);
            });*/

        }
        private void OnEnable()
        {
            Social_btn = GetComponent<Button>();
            Server.OnCreateTeamCompeleted += Server_OnCreateTeamCompeleted;
            Social_btn.onClick.AddListener(() =>
            {


                if (Server.BODY.social.team.teamId == "")
                {
                    MakeTeamTab.SetActive(true);
                    TeamsTab.SetActive(true);
                    ChatsTeamTab.SetActive(false);
                    StartGameTimerTab.SetActive(false);
                    SocialTabs.ShowTabsDefalt = 0;
                    Debug.Log("HAVE NOT A TEAM" + Server.BODY.social.team.teamId.Length);
                }
                else
                {
                    ChatsTeamTab.SetActive(true);
                    StartGameTimerTab.SetActive(true);
                    MakeTeamTab.SetActive(false);
                    TeamsTab.SetActive(false);
                    SocialTabs.ShowTabsDefalt = 4;
                    Debug.Log(" HAVE A TEAM" + Server.BODY.social.team.teamId.Length);
                }
                navigationUi.SwitchUI("social");
                navigationUi.ColorButtonFix(Social_btn);
            });
        }
        private void OnDisable()
        {
            
            Server.OnCreateTeamCompeleted -= Server_OnCreateTeamCompeleted;
            Social_btn.onClick.RemoveAllListeners();
        }
        private void OnDestroy()
        {
            Server.OnCreateTeamCompeleted -= Server_OnCreateTeamCompeleted;
            Social_btn.onClick.RemoveAllListeners();
        }
        private void Server_OnCreateTeamCompeleted()
        {
            if (Server.BODY.social.team.teamId == "")
            {
                if (!onceShowDontHaveTeam)
                {
                    MakeTeamTab.SetActive(true);
                    TeamsTab.SetActive(true);
                    ChatsTeamTab.SetActive(false);
                    StartGameTimerTab.SetActive(false);
                    SocialTabs.ShowTabsDefalt = 1;
                    onceShowDontHaveTeam = true;
                    onceShowHaveTeam = false;
                } 
                //SocialTabs.ShowTabs(1);
                ///  Debug.Log("HAVE NOT A TEAM" + Server.BODY.social.team.teamId.Length);
            }
            else
            {
                if (!onceShowHaveTeam)
                {
                    ChatsTeamTab.SetActive(true);
                    StartGameTimerTab.SetActive(true);
                    MakeTeamTab.SetActive(false);
                    TeamsTab.SetActive(false);
                    SocialTabs.ShowTabsDefalt = 4;
                    //   Debug.Log(" HAVE A TEAM" + Server.BODY.social.team.teamId.Length);
                    SocialTabs.ShowTabs(5);
                    onceShowHaveTeam  = true;
                    onceShowDontHaveTeam = false;
                }
            }

            // navigationUi.SwitchUI("social");
            //  navigationUi.ColorButtonFix(Social_btn);
        }
    }
}