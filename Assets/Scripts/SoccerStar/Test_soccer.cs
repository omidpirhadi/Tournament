
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Test_soccer : MonoBehaviour
{
    public Ease ease;
    public Diaco.SoccerStar.Server.ServerManager server;
    public float smoothTime = 0.05f;
    public float smoothA = 0.05f;
    public Button Go, GoWithVelocity;
    public int Frame = 0;
    public void Start()
    {
       /* Go.onClick.AddListener(() =>
        {
            this.Frame = 0;
            Physics.autoSimulation = false;
            InvokeRepeating("serverSimlator", 00, smoothTime);

        });
        GoWithVelocity.onClick.AddListener(() =>
        {
            this.Frame = 0;
            Physics.autoSimulation = false;
            InvokeRepeating("serverSimlatorv", 00, smoothTime);

        });*/
    }




    void serverSimlator()
    {

        /*  if (Frame < server.Frames.Count)
              transform.GetComponent<Rigidbody>().DOMove(server.Frames[Frame].Position, smoothA);
          else
              CancelInvoke("serverSimlator");
          Frame++;*/
    }
    void serverSimlatorv()
    {

        /* if (Frame < server.Frames.Count)
         {
             //transform.position = server.Frames[Frame].Position;
              transform.GetComponent<Rigidbody>().DOMove(server.Frames[Frame].Position, smoothA)
                   .SetEase(ease)
                   .OnComplete(() =>
                   {

                   });
             transform.GetComponent<Rigidbody>().MovePosition(server.Frames[Frame].Position);
             transform.GetComponent<Rigidbody>().velocity = server.Frames[Frame].velocity;
             Frame++;
             Physics.Simulate(smoothA);
             //transform.GetComponent<Rigidbody>().do
         }
         else
         {
             Physics.autoSimulation = true;
             CancelInvoke("serverSimlatorv");
         }*/

    }
}