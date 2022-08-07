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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sequence = DOTween.Sequence();
    }

    public void MoveBall(Diaco_Billiard_Vec pos, Diaco_Billiard_Vec angularvelocity, Diaco_Billiard_Vec velocity, float speed, int TikPacket, int TikLoop)
    {

        //* sequence.Append(transform.DOMove(pos, speed)).SetEase(ease);
        //* sequence.Join(transform.DORotate(angularvelocity, speed)).SetEase(ease);

        // transform.DORotate(angularvelocity, speed).SetEase(ease);

        rb.velocity = Vec3Helper.ToVector3(velocity);
        rb.position = Vec3Helper.ToVector3(pos);
        /// if (TikPacket == TikLoop)

        // rb.angularVelocity = angularvelocity;
    }
    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}