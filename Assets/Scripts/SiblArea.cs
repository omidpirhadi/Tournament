using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiblArea : MonoBehaviour
{
    private Sibl sibl;
    public int AreaIndex;
  // public SoundEffectControll soundEffect;
    void Start()
    {
        sibl = FindObjectOfType<Sibl>();
     //   soundEffect = GetComponent<SoundEffectControll>();

    }

    private void OnTriggerEnter(Collider WhiteBall)
    {

        if (WhiteBall.tag == "ball")
        {

            sibl.Area = AreaIndex;
            Debug.Log(gameObject.name);
        }
       
    }

  
    private void OnTriggerExit(Collider WhiteBall)
    {
        if ( WhiteBall.tag == "ball")
        {

            sibl.Area = AreaIndex + 1;
           /// Debug.Log(gameObject.name);
        }
    }
}
