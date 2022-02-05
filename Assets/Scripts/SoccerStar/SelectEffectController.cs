using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Diaco.SoccerStar.Marble
{
    public class SelectEffectController : MonoBehaviour
    {
        public SpriteRenderer EffectRenderer;

        public float SpeedRotate;
        public Tweener RotateTweener;
        public LoopType loopType;
        public Ease ease;

        public void OnEnable()
        {
            EffectRenderer = GetComponent<SpriteRenderer>();
            EffectRenderer.enabled = true;
            RotateEffect();
        }
        public void OnDisable()
        {


            EffectRenderer.enabled = false;
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
}