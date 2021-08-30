using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BallStoperInGoals : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "ball")
        {
            DOVirtual.Float(0, 1, 0.5f, (x) =>
            {
                collision.rigidbody.velocity *= x;
            });
            Debug.Log("IN END GOAL");
        }
    }
}
