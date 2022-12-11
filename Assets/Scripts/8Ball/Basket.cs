using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Basket : MonoBehaviour
{
    public Transform Parent;
    public Transform ConceptBall;
    public Vector3 PositionStart;
    public Material[] SkinBalls;
    // public Transform[] Path;
    public Queue<int> QueueBasket;
  
    public List<int> ballinbasket;
    public float Delay = 0.2f;
    public float Duration;
    //  public int IDBall;
    public bool InUse = false;
    public IEnumerator ExtractBall()
    {

        if (InUse == false)
        {
            while (QueueBasket.Count > 0)
            {
                InUse = true;
                var BallID = QueueBasket.Dequeue();
                var ball = Instantiate(ConceptBall, PositionStart, ConceptBall.rotation, Parent);
                ball.GetComponent<ballinbasket>().BallID = BallID;
                ball.GetComponent<MeshRenderer>().material = SkinBalls[BallID];
                yield return new WaitForSecondsRealtime(Delay);
            }
            InUse = false;
        }

    }
    public IEnumerator ExtractBallFast()
    {
        if (InUse == false)
        {
            Debug.Log(".....................................BASKET RELASE");
            while (QueueBasket.Count > 0)
            {
                InUse = true;
                var BallID = QueueBasket.Dequeue();
                var ball = Instantiate(ConceptBall, PositionStart, ConceptBall.rotation, Parent);
                ball.GetComponent<ballinbasket>().BallID = BallID;
                ball.GetComponent<MeshRenderer>().material = SkinBalls[BallID];
                yield return new WaitForSecondsRealtime(2f);
            }
            InUse = false;
        }
    }

    private Diaco.EightBall.Server.BilliardServer server;
    public void AddToQueue(int id)
    {
        if (!QueueBasket.Contains(id))
        {
            if (!ballinbasket.Contains(id))
            {


                QueueBasket.Enqueue(id);
                ballinbasket.Add(id);

            }
        }
        ///  Debug.Log("688888888");
    } 
    public void clearbasket()
    {
        ballinbasket.Clear();
    }
    void Start()
    {
        QueueBasket = new Queue<int>(15);
        ballinbasket = new List<int>();

    }

}
