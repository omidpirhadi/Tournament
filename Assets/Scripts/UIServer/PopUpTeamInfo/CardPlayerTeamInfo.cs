using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.TeamInfo
{
    public class CardPlayerTeamInfo : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI NavigationUi;

        public string TeamID = "";
        public Button OpenProfileButton;
        public Image AdminIndicator;
        public Image ProfileImage;
        public Text UserName;
        public string userid;
        public Text Cup;

        public void OnEnable()
        {
            Server = FindObjectOfType<ServerUI>();
            NavigationUi = FindObjectOfType<NavigationUI>();
            OpenProfileButton.onClick.AddListener(() =>
            {

                NavigationUi.LastTeamInfoChecked = TeamID;

                
                Server.GetProfilePerson(userid);
                
              //  Debug.Log("OK");
            });
        }
        public void OnDisable()
        {

            OpenProfileButton.onClick.RemoveAllListeners();
          
        }
        public void SetCard(bool Admin , Sprite profileimage, string username, string userid, string cup ,string Team)
        {
            if(Admin)
            {
                AdminIndicator.enabled = true;
            }
            else
            {
                AdminIndicator.enabled = false;
            }
            ProfileImage.sprite = profileimage;
            UserName.text = username;
            this.userid = userid;
            Cup.text = cup;
            TeamID = Team;
        }
    }
}