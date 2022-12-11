using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeyboard : MonoBehaviour
{
    public _GameLobby Game;
    private bool Isopen = false;
    TouchScreenKeyboard keyboard;



    public void Update()
    {

        if (Isopen)
        {
            if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                Ondone();
            }
        }

    }
    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, true, false, false, false, "", 50);
        Isopen = true;
    }
    void Ondone()
    {
        var context = "";
        context = keyboard.text;
        if (Game == _GameLobby.Soccer)
        {

            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_Message(context);

        }
        else if (Game == _GameLobby.Billiard)
        {
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().Emit_Message(context);
        }
        
        keyboard.text = "";
        Isopen = false;
        Debug.Log("SendMessage:" + keyboard.text);
    }
}
