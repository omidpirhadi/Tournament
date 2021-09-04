﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diaco.EightBall.CueControllers
{
    public class CueSpinSmallController : MonoBehaviour
    {
        public RectTransform Rect_CueSpin;
        public float Min, Max;
        private HitBallController CueBall;
        public void Start()
        {
            CueBall = FindObjectOfType<HitBallController>();
            CueBall.ResetCueSpin += CueBall_ResetCueSpin;
        }
        private void CueBall_ResetCueSpin()
        {
            Rect_CueSpin.anchoredPosition3D = new Vector3(0, 0, 0);

        }
        public void SetPositions(Vector2 pos)
        {
            Rect_CueSpin.anchoredPosition3D = new Vector3(pos.x * Max, pos.y * Max, 0.0f);
        }
    }
}