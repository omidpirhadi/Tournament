using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SelectEffectController : MonoBehaviour
{

    public float SpeedRotate;
    public Tweener RotateTweener;
    public LoopType loopType;
    public Ease ease;
    public void Start()
    {
      
    }
    public void Update()
    {
        
        

    }
    public void OnEnable()
    {
        RotateEffect();
    }
    public void OnDisable()
    {
      RotateTweener.Kill(false);
    }
    public void RotateEffect()
    {

        RotateTweener = DOVirtual.Float(0, 360, SpeedRotate, (x) =>
       {
           transform.eulerAngles = new Vector3(90.0f, 0.0f, transform.eulerAngles.z + x);
           // transform.RotateAround(transform.position, Vector3.up, x);
       });
        RotateTweener.SetEase(ease);
        RotateTweener.SetLoops(-1, loopType);
    }
}
