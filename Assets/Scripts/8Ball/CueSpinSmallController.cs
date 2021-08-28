using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueSpinSmallController : MonoBehaviour
{
    public RectTransform Rect_CueSpin;
    public float Min, Max;

    public void SetPositions(Vector2 pos)
    {
        Rect_CueSpin.anchoredPosition3D = new Vector3(pos.x * Max, pos.y * Max, 0.0f);
    }
}
