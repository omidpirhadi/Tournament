
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Test_soccer : MonoBehaviour
{

    public new Rigidbody rigidbody;
        public Vector3[] positions;
    public float duration = 0.03f;
    public LoopType loopType;
    public Ease ease;
    public int index = 0;
    public Tweener loop;
    public void Start()
    {
        posloop();
       
    }
    public void Update()
    {

    }
    private void posloop()
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < positions.Length; i++)
        {
            
            loop = rigidbody.DOMove(positions[index], duration);
            //sequence.Append(loop);

            index++;
        } ///       sequence.SetEase(ease);
          //  sequence.SetLoops(1, loopType);
    }
}