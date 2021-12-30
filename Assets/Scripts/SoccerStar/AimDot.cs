using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDot : MonoBehaviour
{
    [SerializeField] private AimCircle aimCircle;
    [SerializeField] private Transform StarPos,EndPos;
    public float DotPower = 4.0f;
    [SerializeField] private float StepMove = -0.67f;


    LineRenderer line;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        SetAimDot();
        line.SetPosition(0, StarPos.localPosition);
        line.SetPosition(1, EndPos.localPosition); 
    }
    void SetAimDot()
    {
        if(aimCircle.CurrentAimPower>3.5 )
        {
            EndPos.localPosition = new Vector3(0, 0, Mathf.Clamp(StepMove * DotPower, -1, 0));
            //Debug.Log("aimdot::" + StepMove * DotPower);
        }
        else if(aimCircle.CurrentAimPower < 3.5)
        {
            EndPos.localPosition = new Vector3(0, 0, 0);
           // Debug.Log("aimdot::0");
        }
    }
}
