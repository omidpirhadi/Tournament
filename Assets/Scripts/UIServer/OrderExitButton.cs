using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OrderExitButton : MonoBehaviour
{

    public ServerUI Server;
    public NavigationUI navigationUi;
    public Button ExitButton;
    private void Awake()
    {
        Server = FindObjectOfType<ServerUI>();
        navigationUi = FindObjectOfType<NavigationUI>();
        ExitButton = GetComponent<Button>();
        ExitButton.onClick.AddListener(() => {

            navigationUi.ClosePopUp("profilefromteam");
            if (navigationUi.LastTeamInfoChecked != "")
            {

                navigationUi.ShowPopUp("teaminfo");
                navigationUi.loadTeaminfoWithLastTeamID();
            }
            navigationUi.LastTeamInfoChecked = "";

        });

    }
    private void OnEnable()
    {
     //   Server = FindObjectOfType<ServerUI>();
      //  navigationUi = FindObjectOfType<NavigationUI>();
      //  ExitButton = GetComponent<Button>();
      
    }
    private void OnDisable()
    {
       // ExitButton.onClick.RemoveAllListeners();
    }
}
