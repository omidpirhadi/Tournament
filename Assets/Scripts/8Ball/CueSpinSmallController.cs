using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.EightBall.CueControllers
{
    public class CueSpinSmallController : MonoBehaviour
    {
        public RectTransform Rect_CueSpin;
        public float Min, Max;
        private HitBallController CueBall;
      /// private Button btn;
        public void Start()
        {
            CueBall = FindObjectOfType<HitBallController>();
            //btn = GetComponent<Button>();
           /// btn.onClick.AddListener(() => { CueBall.TouchWorkInUI = true });
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