using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_Slider : MonoBehaviour
{
    public RectTransform Content;
    public Button GoLeftButton;
    public Image GoLeft_image;

    public Button GoRightButton;
    public Image GoRight_image;

    public Vector3 Left;
    public Vector3 Right;

    void OnEnable()
    {
        GoLeftButton.onClick.AddListener(SlideToLeft);
        GoRightButton.onClick.AddListener(SlideToRight);
    }
    void OnDisable()
    {
        GoLeftButton.onClick.AddListener(SlideToLeft);
        GoRightButton.onClick.AddListener(SlideToRight);
    }
    void SlideToLeft()
    {
        Content.anchoredPosition3D = Left;

        GoLeftButton.interactable = false;
        GoLeft_image.color = new Color(1, 1, 1, 0.2f);
        GoRightButton.interactable = true;
        GoRight_image.color = new Color(1, 1, 1, 1f);
    }
    void SlideToRight()
    {
        Content.anchoredPosition3D = Right;


        GoLeftButton.interactable = true;
        GoLeft_image.color = new Color(1, 1, 1, 1f);
        GoRightButton.interactable = false;
        GoRight_image.color = new Color(1, 1, 1, 0.2f);
    }
}
