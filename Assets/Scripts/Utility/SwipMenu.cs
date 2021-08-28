using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;
[ExecuteInEditMode]
public class SwipMenu : MonoBehaviour
{
    public RectTransform GeneralRect;
    public RectTransform Center;
    public List<RectTransform> Element;
    public Scrollbar Scrollbar;
    public Vector2 ContentMinSize;
    public Vector2 ContentMaxSize;
    public float MinDistace, MaxDistance;
    public float PercentChangeSize;
    


   
    void Start()
    {
      
        PercentChangeSize = (MaxDistance - MinDistace) / 100.0f;
        
        Swipe();
    }

    void Update()

    {
      
        Swipe();
        //  print(Center.rect.center);
        ///  print(Screen.width / 2 + "::;;:" + Screen.height / 2);
        //////   print(RectTransformUtility.WorldToScreenPoint(Camera.main, Center.position)/2.5f);
        //print(Camera.main.WorldToScreenPoint(Center.position));
    }
    public void Swipe()
    {
        var pos_center = ConvertPositionElementToRect(GeneralRect, Center.position); 
        
        for (int i = 0; i < Element.Count; i++)
        {
            var pos_element = ConvertPositionElementToRect(GeneralRect, Element[i].position);
            var dis = Mathf.Abs(pos_element.x - pos_center.x);
            
            if (dis > MinDistace && dis < MaxDistance)
            {

                var change_size = CalculateSize(ContentMaxSize, ContentMinSize, Element[i].sizeDelta, PercentChangeSize);

                // Element[i].DOSizeDelta(change_size, 0.001f);
                Element[i].sizeDelta = change_size;
                Scrollbar.value = Scrollbar.value;
            }
            if (dis > MaxDistance)
            {

                var change_size = CalculateSize(ContentMaxSize, ContentMinSize, Element[i].sizeDelta, -PercentChangeSize);
                // Element[i].DOSizeDelta(change_size, 0.001f);
                Element[i].sizeDelta = change_size;
            }
        }
    }
    private Vector2 CalculateSize(Vector2 refrencesize_max, Vector2 refrencesize_min, Vector2 SizeElement, float percent)
    {


        var x = Mathf.Clamp(SizeElement.x + percent, refrencesize_min.x, refrencesize_max.x);
        var y = Mathf.Clamp(SizeElement.y + percent, refrencesize_min.y, refrencesize_max.y);
        var size = new Vector2(x, y);
        return size;
    }
    private Vector2 ConvertPositionElementToRect(RectTransform GeneralRect, Vector3 ElementWorldPosition)
    {
        var positioninrect = new Vector2();
        var screenpoint = Camera.main.WorldToScreenPoint(ElementWorldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GeneralRect, screenpoint, Camera.main, out positioninrect);
        return positioninrect;
    
    }
}
