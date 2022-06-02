using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ChatBubble : MonoBehaviour
{
   
    public RectTransform background;
    public RTLTMPro.RTLTextMeshPro context;
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
    private void OnDestroy()
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

    [Button("Test",  ButtonSizes.Medium,  ButtonStyle.Box)]
    public void SetBubble(string mess = "سلام حالت چظوره؟",float duration =  3.0f)
    {
        timer.Kill(false);
        if (context)
            context.text = mess;
        if (background)
            background.sizeDelta = new Vector2(context.preferredWidth, context.preferredHeight) + Padding;
        timer = DOVirtual.Float(0, 1, duration, (x) => { }).OnComplete(() =>
        {
            SetBubble("", 0);
        });

    }

}
