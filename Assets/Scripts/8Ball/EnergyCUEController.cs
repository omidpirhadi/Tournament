using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Diaco.EightBall.CueControllers
{

    public class EnergyCUEController : MonoBehaviour
    {
        public HandlerSilder CueWoodEnergySlider;
        public float DurationShowOrHideEnergyBar = 0.3f;
        private RectTransform Rect;
        void Start()
        {
            CueWoodEnergySlider = GetComponentInChildren<HandlerSilder>();
            CueWoodEnergySlider.OnChange += CueWoodEnergySlider_OnChange;
            CueWoodEnergySlider.OnBegin += CueWoodEnergySlider_OnBegin;
            CueWoodEnergySlider.OnEnd += CueWoodEnergySlider_OnEnd;
            //GetEventSystem.currentSelectedGameObject
            Rect = GetComponent<RectTransform>();
        }



        private void CueWoodEnergySlider_OnBegin()
        {
            Handler_OnBeginChangeEnergy();
        }

        private void CueWoodEnergySlider_OnChange(float x)
        {
            Handler_OnChangeEnergy(x);
        }
        private void CueWoodEnergySlider_OnEnd(float x)
        {
            Handler_OnEnergyTouchEnd(x);
        }


        public void Show(bool show)
        {
            if (show == false)
            {
                Rect.DOAnchorPos(new Vector2(-80, 0), DurationShowOrHideEnergyBar);
            }
            else
            {
                Rect.DOAnchorPos(new Vector2(0, 0), DurationShowOrHideEnergyBar);
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