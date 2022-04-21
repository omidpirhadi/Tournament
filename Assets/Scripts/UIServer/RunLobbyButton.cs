using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RunLobbyButton : MonoBehaviour
{
  //  public int Game;
    public _GameLobby gameLobby;
    public _SubGame SubGame;
    private Button submit;
    public void OnEnable()
    {
        submit = GetComponent<Button>();
        var Server = FindObjectOfType<ServerUI>();
        var uinavigation = FindObjectOfType<NavigationUI>();



        submit.onClick.AddListener(() =>
        {
            uinavigation.GameLobby = gameLobby;
            uinavigation.SubGame = SubGame;
            Server.RequestFindOpponent((short)gameLobby, (short)SubGame);


        });
    }

    public void OnDisable()
    {
        submit.onClick.RemoveAllListeners();
    }
}
