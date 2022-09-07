using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Diaco.EightBall.Structs;
public class AddressBall : MonoBehaviour
{
    public int IDPost = 0;


    public Ease ease = Ease.Linear;

    public Rigidbody rb;
    private Sequence sequence;

    private int lastpacket_index = -1;
    private Diaco.EightBall.Server.BilliardServer server;
    void Start()
    {
        server = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
        server.OnTurn += Server_OnTurn;
        rb = GetComponent<Rigidbody>();
        sequence = DOTween.Sequence();
    }

    private void Server_OnTurn(bool obj)
    {
        if (obj)
            lastpacket_index = -1;
    }

    public void MoveBall(Diaco_Billiard_Vec pos, Diaco_Billiard_Vec angularvelocity, Diaco_Billiard_Vec velocity, float speed, int TikPacket, int TikLoop)
    {



        rb.velocity = Vec3Helper.ToVector3(velocity);
        rb.position = Vec3Helper.ToVector3(pos);
      /*  if (TikPacket != lastpacket_index)
        {
            

        }

        lastpacket_index = TikPacket;*/

    }
    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}