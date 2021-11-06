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
    
    public Tweener timer;
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
    private void ChatBubble_InComingMessage(string mess,float duration)
    {
       
        timer.Kill(false);
        SetBubble(mess,duration);
    }

    
    public void SetBubble(string mess,float duration)
    {
        context.text = mess;
        background.sizeDelta= new Vector2( context.preferredWidth, context.preferredHeight )+Padding;
        timer = DOVirtual.Float(0, 1, duration, (x) => { }).OnComplete(() =>
        {
            SetBubble("", 0);
        });

    }

}
