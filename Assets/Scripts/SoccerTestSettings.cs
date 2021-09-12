using System;

using UnityEngine;
using UnityEngine.UI;

public class SoccerTestSettings : MonoBehaviour
{
    public Slider MassMarble;
    public Slider ForceMarble;
    public Slider DragMarble;
    public Slider AngularDragMarble;
    public Slider AccelerationBall;
    public Slider MassBall;
    public Slider DragBall;
    public Slider AngularDragBall;
    public Button Set_Btn;
    private void OnEnable()
    {
        Set_Btn.onClick.AddListener(() => {
            Set();
        });


        MassMarble.onValueChanged.AddListener(x =>
        {
            MassMarble.GetComponentInChildren<Text>().text = "MassMarble:" + x.ToString();
        });

        ForceMarble.onValueChanged.AddListener(x =>
        {
            ForceMarble.GetComponentInChildren<Text>().text = "ForceMarble:" + x.ToString();
        });
     
        DragMarble.onValueChanged.AddListener(x =>
        {
            DragMarble.GetComponentInChildren<Text>().text = "DragMarble:" + x.ToString();
        });

        AngularDragMarble.onValueChanged.AddListener(x =>
        {
            AngularDragMarble.GetComponentInChildren<Text>().text = " AngularDragMarble:" + x.ToString();
        });

        AccelerationBall.onValueChanged.AddListener(x =>
        {
            AccelerationBall.GetComponentInChildren<Text>().text = "AccelerationBallAfterHit:" + x.ToString();
        });

        MassBall.onValueChanged.AddListener(x =>
        {
            MassBall.GetComponentInChildren<Text>().text = "MassBall:" + x.ToString();
        });
        DragBall.onValueChanged.AddListener(x =>
        {
            DragBall.GetComponentInChildren<Text>().text = "DragBall:" + x.ToString();
        });
        AngularDragBall.onValueChanged.AddListener(x =>
        {
            MassBall.GetComponentInChildren<Text>().text = "AngularDragBall:" + x.ToString();
        });
    }
    private void OnDisable()
    {
        MassMarble.onValueChanged.RemoveAllListeners();
        ForceMarble.onValueChanged.RemoveAllListeners();
        DragMarble.onValueChanged.RemoveAllListeners();
        AccelerationBall.onValueChanged.RemoveAllListeners();
        AngularDragMarble.onValueChanged.RemoveAllListeners();
        MassBall.onValueChanged.RemoveAllListeners();
        DragBall.onValueChanged.RemoveAllListeners();
        AngularDragBall.onValueChanged.RemoveAllListeners();
        Set_Btn.onClick.RemoveAllListeners();
    }

    public void Set()
    {
        Handler_OnChangeSetting(MassMarble.value, ForceMarble.value, DragMarble.value, AngularDragMarble.value, AccelerationBall.value, MassBall.value, DragBall.value, AngularDragBall.value);
        Debug.Log("Change Setting");
    }
    public event Action<float, float, float, float, float, float,float,float> OnChangeSetting;

    public void Handler_OnChangeSetting(float MassMarble, float ForceMarble, float DragMarble, float AngularDragMarble, float AccelerationBall, float MassBall,float DragBall,float AngularDragBall)
    {
        if (OnChangeSetting != null)
        {
            OnChangeSetting(MassMarble, ForceMarble, DragMarble, AngularDragMarble, AccelerationBall, MassBall, DragBall, AngularDragBall);
        }
    }
}
