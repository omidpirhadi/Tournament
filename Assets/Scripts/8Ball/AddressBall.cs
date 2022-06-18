using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AddressBall : MonoBehaviour
{
  public int IDPost = 0;


    public Ease ease = Ease.Linear;

    public  Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveBall(Vector3 pos,Vector3 angularvelocity, Vector3 velocity, float speed)
    {
        // transform.DOMove(pos, speed).SetEase(ease);
        // transform.DORotate(rotate, speed).SetEase(ease);
        rb.position = pos;
        rb.velocity = velocity;
        rb.angularVelocity = angularvelocity;
    }
    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
