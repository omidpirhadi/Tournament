using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueWood : MonoBehaviour
{

    // public Vector3 DefaultPosition = new Vector3(-0.5f, 0.0f, 0.0f);
    //  public Vector3 DefaultRotation = new Vector3(0.0f, 0.0f, 0.0f);

    public Diaco.EightBall.CueControllers.HitBallController whiteball;
    public Transform WoodRenderer;
    public RectTransform WoodInHud;
    public Vector2 Offset;
    public Canvas canvas;

    void Start()
    {
        whiteball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
        this.transform.position = whiteball.transform.position;
        WoodRenderer.localPosition = new Vector3(-7.8f, 1.0f, 0f);
    }
    private void OnDisable()
    {
        WoodInHud.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        whiteball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
        this.transform.position = whiteball.transform.position;
        WoodRenderer.localPosition = new Vector3(-7.8f, 1.0f, 0f);

        WoodInHud.gameObject.SetActive(true);
        this.transform.position = whiteball.transform.position;
        var pos = Camera.main.WorldToScreenPoint(WoodRenderer.position);
        float h = Screen.height;
        float w = Screen.width;
        float x = pos.x - (w / 2);
        float y = pos.y - (h / 2);
        float s = canvas.scaleFactor;
        WoodInHud.anchoredPosition = (new Vector2(x, y) / s) + Offset;
        WoodInHud.eulerAngles = new Vector3(0, 0, -this.transform.eulerAngles.y);
    }
    void LateUpdate()
    {
        this.transform.position = whiteball.transform.position;
        var pos = Camera.main.WorldToScreenPoint(WoodRenderer.position);
        float h = Screen.height;
        float w = Screen.width;
        float x = pos.x - (w / 2);
        float y = pos.y - (h / 2);
        float s = canvas.scaleFactor;
        WoodInHud.anchoredPosition = (new Vector2(x, y) / s) + Offset;
        WoodInHud.eulerAngles = new Vector3(0, 0, -this.transform.eulerAngles.y);

        // WoodRenderer.localPosition = new Vector3(-8.5f, 1.0f, 0.0f);
    }
}

