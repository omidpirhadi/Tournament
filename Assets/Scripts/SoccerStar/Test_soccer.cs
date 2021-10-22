
using UnityEngine;
using DG.Tweening;
public class Test_soccer : MonoBehaviour
{
    public new Rigidbody rigidbody;
    public float StepRotateBall;
    public float DurationStep;

    public Ease EaseTypeRotate;


    void FixedUpdate()
    {
        RotateBall();
    }
    private void RotateBall()
    {
        Vector3 normal = new Vector3(0, 1, 0);
        Vector3 movement = rigidbody.velocity * Time.fixedDeltaTime;
        Vector3 aix = Vector3.Cross(normal, movement).normalized;

        float distance = movement.magnitude;
        float angle = distance * (180 / Mathf.PI) / StepRotateBall;
        /// Debug.Log(speed);
        transform.DOLocalRotateQuaternion(Quaternion.Euler(aix * angle) * transform.localRotation, DurationStep).SetEase(EaseTypeRotate);
        //transform.localRotation =;

        // IsRotateBall = true;

    }
}
