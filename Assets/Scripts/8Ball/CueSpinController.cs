
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Diaco.EightBall.CueControllers
{
    
    public class CueSpinController : MonoBehaviour, IDragHandler, IEndDragHandler
    {

        // public Image CueSpinIndicator;
        
        public RectTransform Rect_CueSpin;
        public CueSpinSmallController CueSpinSmall;
        public float Min, Max;
        private HitBallController CueBall;

        private RectTransform WhiteCircle;
        public float radius =54.0f;
        public void Start()
        {
            CueBall = FindObjectOfType<HitBallController>();
            WhiteCircle = this.GetComponent<RectTransform>();
            CueBall.ResetCueSpin += CueBall_ResetCueSpin;
        }

        private void CueBall_ResetCueSpin()
        {
            Rect_CueSpin.anchoredPosition3D = new Vector3(0, 0, 0);

        }

        public void OnDrag(PointerEventData eventData)
        {
            
            Vector3 localpos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(Rect_CueSpin, eventData.position, eventData.pressEventCamera, out localpos))
            {

                Rect_CueSpin.position = localpos;
                Vector3 v = Rect_CueSpin.localPosition - WhiteCircle.localPosition;
                v = Vector3.ClampMagnitude(v, radius);
                Rect_CueSpin.localPosition = WhiteCircle.localPosition + v;
                Handler_OnDragSpin();
                CueSpinSmall.SetPositions(Spin());
               /// Debug.Log(Rect_CueSpin.localPosition);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Handler_OnChangeSpin(Spin());
           //Debug.Log("ChangeSpin");
        }

        private Vector2 Spin()
        {
            Vector2 spin;
            var x = Rect_CueSpin.anchoredPosition3D.x / Max;
            var y = Rect_CueSpin.anchoredPosition3D.y / Max;
            spin = new Vector2(x, y);
            
            Debug.Log("Spin :  " + spin);
            return spin;
        }
        #region Events

        public event Action<Vector2> OnChangeValueSpin;
        protected virtual void Handler_OnChangeSpin(Vector2 pos)
        {
            if(OnChangeValueSpin != null)
            {
                OnChangeValueSpin(pos);
            }
        }

        public event Action OnDragSpin;
        protected  void Handler_OnDragSpin()
        {
            if (OnDragSpin != null)
            {
                OnDragSpin();
            }
        }
        #endregion
    }
}