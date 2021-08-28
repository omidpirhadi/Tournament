using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_Slider : MonoBehaviour
{
    public RectTransform Content;
    public Button GoLeftButton;
    public Button GoRightButton;

    public Vector3 Left;
    public Vector3 Right;
    void Start()
    {
        GoLeftButton.onClick.AddListener(SlideToLeft);
        GoRightButton.onClick.AddListener(SlideToRight);
    }

    void SlideToLeft()
    {
        Content.anchoredPosition3D = Left;
    }
    void SlideToRight()
    {
        Content.anchoredPosition3D = Right;
        
    }
}
