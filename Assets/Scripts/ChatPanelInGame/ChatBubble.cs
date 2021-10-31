using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ChatBubble : MonoBehaviour
{
   
    public RectTransform background;
    public Text context;
    public Vector2 Padding;
    private void OnEnable()
    {
        if (FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>())
        {


            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().InComingMessage += ChatBubble_InComingMessage;
        }
        else if (FindObjectOfType<Diaco.EightBall.Server.BilliardServer>())
        {

            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().InComingMessage += ChatBubble_InComingMessage;
        }
    }
    private void OnDisable()
    {
        if (FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>())
        {


            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().InComingMessage -= ChatBubble_InComingMessage;
        }
        else if (FindObjectOfType<Diaco.EightBall.Server.BilliardServer>())
        {
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().InComingMessage -= ChatBubble_InComingMessage;

        }
    }
    private void ChatBubble_InComingMessage(string mess)
    {
        SetBubble(mess);
    }


    public void SetBubble(string mess)
    {
        context.text = mess;
        background.sizeDelta= new Vector2( context.preferredWidth, context.preferredHeight )+Padding;
        DOVirtual.Float(0, 1, 10, (x) => { }).OnComplete(() =>
        {
            SetBubble("");
        });

    }
}
