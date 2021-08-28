using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixrenderer : MonoBehaviour
{
    public Transform FixRenderer;

    void  LateUpdate()
    {
        FixRenderer.eulerAngles = Vector3.zero;

    }
}
