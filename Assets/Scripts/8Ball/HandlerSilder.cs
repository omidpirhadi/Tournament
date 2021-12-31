using System;

using UnityEngine;
using UnityEngine.EventSystems;
public class HandlerSilder : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public float SliderMin = 0, SliderMax = 100;
    public float CurrentValueSlider = 0;
    public float Anchermin = -200, Anchermax = 200;

    private RectTransform Handler;
    [SerializeField] private Vector2 startPosition;
    private float x;
    private void Start()
    {
        Handler = GetComponent<RectTransform>();
        startPosition = Handler.anchoredPosition; 
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Handler_OnBegin();
        x = Handler.anchoredPosition.x;
        //  Debug.Log("begin");
    }

    public void OnDrag(PointerEventData eventData)
    {
     
        Vector3 localpos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(Handler, eventData.position, eventData.pressEventCamera, out localpos))
        {
            Handler.position = new Vector3(Handler.position.x, localpos.y, Handler.position.z);
           /// Debug.Log("pos" + localpos);
        }
        Handler.anchoredPosition = new Vector2(x, Mathf.Clamp(Handler.anchoredPosition.y, Anchermin, Anchermax));
        Handler_OnChange(ConvertSpan(Handler.anchoredPosition.y));
        Debug.Log(Handler.anchoredPosition.x);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Handler_OnEnd(ConvertSpan(Handler.anchoredPosition.y));
        Handler.anchoredPosition = startPosition;
        
        //Debug.Log("end");
    }

    private float ConvertSpan(float input)
    {
        float e = (input - Anchermin) * (SliderMax - SliderMin) / (Anchermax - Anchermin);
        CurrentValueSlider = Mathf.Abs(e - SliderMax);
        return CurrentValueSlider;
    }

    public event Action<float> OnChange;
    protected void Handler_OnChange(float x)
    {
        if (OnChange != null)
        {
            OnChange(x);

        }
    }
    public event Action OnBegin;
    protected void Handler_OnBegin()
    {
        if (OnBegin != null)
        {
            OnBegin();

        }
    }
    public event Action<float> OnEnd;
    protected void Handler_OnEnd(float  x)
    {
        if (OnEnd != null)
        {
            OnEnd(x);

        }
    }
}
