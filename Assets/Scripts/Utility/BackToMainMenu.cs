using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class BackToMainMenu : MonoBehaviour
{
    public _GameLobby GameType;
    private Diaco.SoccerStar.Server.ServerManager seversoccer;
    private Diaco.EightBall.Server.BilliardServer severbilliard;
    private GameLuncher luncher;
    private NavigationUI ui;
    private Button btn;
    public void OnEnable()
    {
        seversoccer = FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>();
        severbilliard = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
        luncher = FindObjectOfType<GameLuncher>();
        ui = FindObjectOfType<NavigationUI>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => {

            if (GameType == _GameLobby.Soccer)
            {//
                ui.CloseAllPopUp();
                seversoccer.Emit_LeftGame();
               luncher.BackToMenu();
               
            
                Debug.Log("VCVVVVV");
            }
            else if(GameType == _GameLobby.Billiard)
            {
                ui.CloseAllPopUp();
                severbilliard.Emit_LeftGame();
                
              luncher.BackToMenu();
              //  
              //  
                Debug.Log("DDDDDDDDDDDDD");
            }

        });
       
    }
    public void OnDisable()
    {
        btn.onClick.RemoveAllListeners();
    }
    public void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }
}
