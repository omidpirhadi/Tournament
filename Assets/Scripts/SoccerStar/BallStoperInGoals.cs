
using UnityEngine;

public class BallStoperInGoals : MonoBehaviour
{
    public float Deceleration = 6;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "ball")
        {
            var speed = collision.rigidbody.velocity.magnitude;
            var dir = collision.rigidbody.velocity.normalized;
            collision.rigidbody.velocity = dir * (speed / Deceleration);
            Debug.Log("IN END GOAL");
        }
    }
}
