using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillardTestSetting : MonoBehaviour
{
    public Slider CueBallPower;
    public Slider Drag;
    public Slider AnglurDrag;
    public Slider MaxAngularDrag;
    public Slider SpeedThershold;
    public Slider SensivityRotate;
    public Button Set_Btn;
    void Start()
    {
       
       
    }
    private void OnEnable()
    {
        Set_Btn.onClick.AddListener(() => {
            Set();
        });


        CueBallPower.onValueChanged.AddListener(x =>
        {
            CueBallPower.GetComponentInChildren<Text>().text = "CueBallPower:" + x.ToString();
        });
        Drag.onValueChanged.AddListener(x =>
        {
            Drag.GetComponentInChildren<Text>().text = "Drag:" + x.ToString();
        });
        MaxAngularDrag.onValueChanged.AddListener(x =>
        {
            MaxAngularDrag.GetComponentInChildren<Text>().text = "MaxAngularDrag:" + x.ToString();
        });
        AnglurDrag.onValueChanged.AddListener(x =>
        {
            AnglurDrag.GetComponentInChildren<Text>().text = "AnglurDrag:" + x.ToString();
        });
        SpeedThershold.onValueChanged.AddListener(x =>
        {
            SpeedThershold.GetComponentInChildren<Text>().text = "SpeedThershold:" + x.ToString();
        });
        SensivityRotate.onValueChanged.AddListener(x =>
        {
            SpeedThershold.GetComponentInChildren<Text>().text = "SensivityRotate:" + x.ToString();
        });
    }
    private void OnDisable()
    {
        CueBallPower.onValueChanged.RemoveAllListeners();
        Drag.onValueChanged.RemoveAllListeners();
        AnglurDrag.onValueChanged.RemoveAllListeners();
        SpeedThershold.onValueChanged.RemoveAllListeners();
        MaxAngularDrag.onValueChanged.RemoveAllListeners();
        SensivityRotate.onValueChanged.RemoveAllListeners();
        Set_Btn.onClick.RemoveAllListeners();
    }

    public void Set()
    {
        Handler_OnChangeSetting(CueBallPower.value, Drag.value, AnglurDrag.value,MaxAngularDrag.value, SpeedThershold.value,SensivityRotate.value);
        Debug.Log("Change Setting");
    }
    public event Action<float, float, float,float, float,float> OnChangeSetting;

    public void Handler_OnChangeSetting(float CuePower,float Drag, float AngularDrag,float MaxAngular, float SpeedThershold, float sensivityrotate)
    {
        if (OnChangeSetting != null)
        {
            OnChangeSetting(CuePower, Drag, AngularDrag, MaxAngular, SpeedThershold, sensivityrotate);
        }
    }

}
