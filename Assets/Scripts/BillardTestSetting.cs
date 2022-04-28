using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillardTestSetting : MonoBehaviour
{
    public InputField CueBallPower;
    public InputField Drag;
    public InputField AnglurDrag;
    public InputField MaxAngularDrag;
    public InputField SpeedThershold;
    public InputField SensivityRotate;///
    public InputField Powerspin;
    public InputField Powerbounce;
    public Button Set_Btn;
    void Start()
    {
       
       
    }
    private void OnEnable()
    {
        Set_Btn.onClick.AddListener(() => {
            Set();
        });


        /*CueBallPower.onValueChanged.AddListener(x =>
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
            SensivityRotate.GetComponentInChildren<Text>().text = "SensivityRotate:" + x.ToString();
        });
        Powerbounce.onValueChanged.AddListener(x =>
        {
            Powerbounce.GetComponentInChildren<Text>().text = "PowerBounceWall:" + x.ToString();
        });*/
    }
    private void OnDisable()
    {
      /*  CueBallPower.onValueChanged.RemoveAllListeners();
        Drag.onValueChanged.RemoveAllListeners();
        AnglurDrag.onValueChanged.RemoveAllListeners();
        SpeedThershold.onValueChanged.RemoveAllListeners();
        MaxAngularDrag.onValueChanged.RemoveAllListeners();
        SensivityRotate.onValueChanged.RemoveAllListeners();
        Powerbounce.onValueChanged.RemoveAllListeners();*/
        Set_Btn.onClick.RemoveAllListeners();
    }

    public void Set()
    {
        var powercue = Convert.ToSingle(CueBallPower.text);
        var dragball = Convert.ToSingle(Drag.text);
        var angulardrag = Convert.ToSingle(AnglurDrag.text);
        var maxangular = Convert.ToSingle(MaxAngularDrag.text);
        var speedthershold = Convert.ToSingle(SpeedThershold.text);
        var sencivityrotate = Convert.ToSingle(SensivityRotate.text);
        var powerbounce = Convert.ToSingle(Powerbounce.text);
        var spin = Convert.ToSingle(Powerspin.text);
        Handler_OnChangeSetting(powercue, dragball, angulardrag, maxangular, speedthershold, sencivityrotate, powerbounce,spin);
        Debug.Log("Change Setting");
    }
    public event Action<float, float, float,float, float,float, float,float> OnChangeSetting;

    public void Handler_OnChangeSetting(float CuePower,float Drag, float AngularDrag,float MaxAngular, float SpeedThershold, float sensivityrotate,float powbounce,float spin)
    {
        if (OnChangeSetting != null)
        {
            OnChangeSetting(CuePower, Drag, AngularDrag, MaxAngular, SpeedThershold, sensivityrotate,powbounce,spin);
        }
    }

}
