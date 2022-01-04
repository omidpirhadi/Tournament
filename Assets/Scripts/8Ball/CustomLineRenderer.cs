using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
public class CustomLineRenderer : MonoBehaviour
{
    [SerializeField] Line Line;
    [SerializeField] Transform CueBall, GhostBall;
    [SerializeField] Transform StartPos, EndPos;
    void Start()
    {
        Line = GetComponent<Line>();
    }
    private void OnEnable()
    {
        Line = GetComponent<Line>();
    }
    // Update is called once per frame
    void Update()
    {

        SetPosition();
    }

    public void SetPosition()
    {
        StartPos.position = CueBall.position;
        EndPos.position = GhostBall.position;

        Line.Start = StartPos.localPosition;
        Line.End = EndPos.localPosition;
  
    }
}
