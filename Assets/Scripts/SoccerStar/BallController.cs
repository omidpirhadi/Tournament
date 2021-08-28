using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    
    public float speedRotation = 1;
    public float Acceleration = 00.00f;
    public Vector3 Velocity;

    private new Rigidbody rigidbody;

    void Start()

    {
        rigidbody = GetComponent<Rigidbody>();
       /// Physics.gravity = Vector3.zero;
        
    }
    void Update()
    {
        Acceleration = rigidbody.velocity.magnitude;
        if (Acceleration > 0.0f)
        {
            
           Velocity = rigidbody.velocity.normalized;
            RotationBall();
            manageBallFriction();
        }
    
    }

    private void RotationBall()
    {
        // rigidbody.DORotate(new Vector3(speedRotation, 0.00f, speedRotation), 0.01f);
        ///    var rotate = Vector3.Lerp(Vector3.zero, new Vector3(360.0f, 0, 360.0f), Time.deltaTime * speedRotation);

        transform.Rotate(Velocity * speedRotation * Acceleration / 10, Space.World);
    }
    void manageBallFriction()
    {
       
        //print("Ball Speed: " + rigidbody.velocity.magnitude);
        if (Acceleration < 0.003f)
        {
            //forcestop the ball
           
          // DOTween.To(() => rigidbody.velocity, x => rigidbody.velocity = x, Vector3.zero, 0.5f);
           // DOTween.To(() => rigidbody.angularVelocity, x => rigidbody.angularVelocity = x, Vector3.zero, 0.5f);
         //   rigidbody.velocity = Vector3.zero;
          // rigidbody.angularVelocity = Vector3.zero;
           GetComponent<Rigidbody>().drag = 2;
            Debug.Log("SSASD");
        }
        else
        {
            //let it slide
            GetComponent<Rigidbody>().drag = 0.75f;
            Debug.Log("SSASD");
        }
    }

}
