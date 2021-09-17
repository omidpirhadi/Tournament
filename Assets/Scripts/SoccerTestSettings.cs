using System;

using UnityEngine;
using UnityEngine.UI;

public class SoccerTestSettings : MonoBehaviour
{
    public InputField MassMarble;
    public InputField ForceMarble;
    public InputField DragMarble;
    public InputField AngularDragMarble;
    public InputField AccelerationBall;
    public InputField MassBall;
    public InputField DragBall;
    public InputField AngularDragBall;
    public InputField BounceWallPhysic;
    public InputField SpeedThershold;
    public PhysicMaterial physicwall;
    public Button Set_Btn;
    private void OnEnable()
    {
        Set_Btn.onClick.AddListener(() => {
            Set();
        });


       
    }
    private void OnDisable()
    {


        Set_Btn.onClick.RemoveAllListeners();
    }

    public void Set()
    {
        var mass_marble = Convert.ToSingle(MassMarble.text);
        var force_marble = Convert.ToSingle(ForceMarble.text);
        var drag_marble = Convert.ToSingle(DragMarble.text);
        var angular_marble = Convert.ToSingle(AngularDragMarble.text);
        var acceleration_ball = Convert.ToSingle(AccelerationBall.text);
        var mass_ball = Convert.ToSingle(MassBall.text);
        var drag_ball = Convert.ToSingle(DragBall.text);
        var angulardrag_ball = Convert.ToSingle(AngularDragBall.text);
        var speedthershold = Convert.ToSingle(SpeedThershold.text);
        Handler_OnChangeSetting(mass_marble, force_marble, drag_marble,angular_marble, acceleration_ball, mass_ball, drag_ball, angulardrag_ball,speedthershold);
        Debug.Log("Change Setting");
    }
    public event Action<float, float, float, float, float, float,float,float,float> OnChangeSetting;

    public void Handler_OnChangeSetting(float MassMarble, float ForceMarble, float DragMarble, float AngularDragMarble, float AccelerationBall, float MassBall,float DragBall,float AngularDragBall,float Speedthershold)
    {
        if (OnChangeSetting != null)
        {
            OnChangeSetting(MassMarble, ForceMarble, DragMarble, AngularDragMarble, AccelerationBall, MassBall, DragBall, AngularDragBall, Speedthershold);
        }
    }
}
