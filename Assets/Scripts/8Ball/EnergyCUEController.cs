using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Diaco.EightBall.CueControllers
{
  
    public class EnergyCUEController : MonoBehaviour, IPointerClickHandler, IEndDragHandler
    {

        public Slider CueWoodEnergySlider;
        public float DurationShowOrHideEnergyBar = 0.3f;
       [SerializeField] private  RectTransform Parent;
        void Start()
        {
            CueWoodEnergySlider.onValueChanged.AddListener((x) =>
            {

                Handler_OnChangeEnergy(x);
            });
            //GetEventSystem.currentSelectedGameObject
            Parent = transform.parent.GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Handler_OnBeginChangeEnergy();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Handler_OnEnergyTouchEnd(CueWoodEnergySlider.value);
            CueWoodEnergySlider.value = CueWoodEnergySlider.minValue;
           // Debug.Log("AAA");
        }

       public void Show(bool show)
        {
            if (show == false)
            {
                Parent.DOAnchorPos(new Vector2(-90, 0), DurationShowOrHideEnergyBar);
            }
            else
            {
                Parent.DOAnchorPos(new Vector2(0, 0), DurationShowOrHideEnergyBar);
            }
        }

        public event Action<float> OnChangeEnergy;
        protected void Handler_OnChangeEnergy(float x)
        {
            if (OnChangeEnergy != null)
            {
                OnChangeEnergy(x);

            }
        }
        public event Action<float> OnEnergyTouchEnd;
        protected void Handler_OnEnergyTouchEnd(float e)
        {
            if (OnEnergyTouchEnd != null)
            {
                OnEnergyTouchEnd(e);
            }
        }
        public event Action OnBeginChangeEnergy;
        protected void Handler_OnBeginChangeEnergy()
        {
            if (OnBeginChangeEnergy != null)
            {
                OnBeginChangeEnergy();
            }
        }
    }
}