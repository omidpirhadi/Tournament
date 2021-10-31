using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SendMessageInGame : MonoBehaviour
{

    public InputField Input;
    public Button Send;

    void OnEnable()
    {
        Send.onClick.AddListener(onclick);
    }
    void OnDisable()
    {
        Send.onClick.RemoveAllListeners();
    }
    void onclick()
    {
        if (FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>())
        {

            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_Message(Input.text);

        }
        else if (FindObjectOfType<Diaco.EightBall.Server.BilliardServer>())
        {
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().Emit_Message(Input.text);
        }
        Debug.Log("SendMessage:" + Input.text);
    }
}

