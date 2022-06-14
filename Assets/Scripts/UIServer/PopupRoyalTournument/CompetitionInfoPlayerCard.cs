using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.RoyalTournument
{
    public class CompetitionInfoPlayerCard : MonoBehaviour
    {
        public ServerUI Server;
        public NavigationUI NavigationUi;

        public string UserID = "";
        public Image Admin;
        public Image ImageProfile;
        public Text Username;
        public Text Cup;


        public Button OpenProfileButton;


        public void OnEnable()
        {
            Server = FindObjectOfType<ServerUI>();
            NavigationUi = FindObjectOfType<NavigationUI>();
            OpenProfileButton = GetComponent<Button>();
            OpenProfileButton.onClick.AddListener(() =>
            {

               


                Server.GetProfilePerson(UserID);

                //  Debug.Log("OK");
            });
        }
        public void OnDisable()
        {

            OpenProfileButton.onClick.RemoveAllListeners();

        }
        public void Set(bool admin, string id, Sprite profile, string user, int cup)
        {
            /*  if(!admin)
              {
                  Admin.enabled = false;
              }*/
            UserID = id;
            ImageProfile.sprite = profile;
            Username.text = user;
            Cup.text = cup.ToString();

        }
    }
}