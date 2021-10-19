using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Fixrenderer : MonoBehaviour
{
    public Transform FixRenderer;
   // public Transform Flag;
  
    void  Update()
    {
      
       
        FixRenderer.eulerAngles = new Vector3(0, 0, 0);

    }
}
