using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[ExecuteInEditMode]
public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] Line Line;
    [SerializeField] Transform Target;
    
    void Start()
    {
       // Line = GetComponent<Line>();
    }
    void Update()
    {
        var t = transform.InverseTransformPoint(Target.position);
        Line.End = t;
    }
    private void OnEnable()
    {
        Line = GetComponent<Line>();
    }

}
