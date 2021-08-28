using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Diaco.EightBall.CueControllers
{
  
    public class EnergyCUEController : MonoBehaviour, IPointerClickHandler, IEndDragHandler
    {

        public Slider CueWoodEnergySlider;

        void Start()
        {
            CueWoodEnergySlider.onValueChanged.AddListener((x) =>
            {

                Handler_OnChangeEnergy(x);
            });
            //GetEventSystem.currentSelectedGameObject
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