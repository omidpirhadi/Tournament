
using UnityEngine;
using DG.Tweening;
public class Test_soccer : MonoBehaviour
{
    public Vector3 t1, t2;
    public int t;

    public LineRenderer lineRenderer;
    public new Rigidbody rigidbody;
    void Start()
    {
       
    }

    void FixedUpdate()
    {

        lineRenderer.positionCount = t;
        for (int i = 0; i < t; i++)
        {
            var point = Vector3.Slerp(t1, t2, t);
            lineRenderer.SetPosition(i, point);
        }
    }
    
}
