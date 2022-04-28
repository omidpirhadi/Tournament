using System;

using UnityEngine;
using UnityEngine.UI;

public class SoccerTestSettings : MonoBehaviour
{
    public InputField MassMarble;
    public InputField ForceMarble;
    public InputField DragMarble;
    public InputField AngularDragMarble;
    public InputField AimPower;
    public InputField MassBall;
    public InputField DragBall;
    public InputField AngularDragBall;
    public InputField BounceWallPhysic;
    public InputField SpeedThershold;
    public InputField SensivityRotateFinger2;
    public AimDot aimDot;
    public PhysicMaterial physicwall;
    public TempPlayerControll PlayerControll;
    public Button Set_Btn;
    private void OnEnable()
    {
        Set_Btn.onClick.AddListener(() => {
            Set();
        });

        PlayerControll = FindObjectOfType<TempPlayerControll>();
       
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
        var aim_power = Convert.ToSingle(AimPower.text);
        var mass_ball = Convert.ToSingle(MassBall.text);
        var drag_ball = Convert.ToSingle(DragBall.text);
        var angulardrag_ball = Convert.ToSingle(AngularDragBall.text);
        var speedthershold = Convert.ToSingle(SpeedThershold.text);
        var bouncewall = Convert.ToSingle(BounceWallPhysic.text);
        var sensiviti = Convert.ToSingle(SensivityRotateFinger2.text);
    
        Handler_OnChangeSetting(mass_marble, force_marble, drag_marble,angular_marble, mass_ball, drag_ball, angulardrag_ball,speedthershold,bouncewall);

       /// physicwall.bounciness = bouncewall;
        PlayerControll.Sensiviti = sensiviti;
        aimDot.DotPower = aim_power;
        Debug.Log("Change Setting");
    }
    public event Action<float, float, float, float, float, float,float,float,float> OnChangeSetting;

    public void Handler_OnChangeSetting(float MassMarble, float ForceMarble, float DragMarble, float AngularDragMarble, float MassBall,float DragBall,float AngularDragBall,float Speedthershold,float BounceWall)
    {
        if (OnChangeSetting != null)
        {
            OnChangeSetting(MassMarble, ForceMarble, DragMarble, AngularDragMarble, MassBall, DragBall, AngularDragBall, Speedthershold,BounceWall);
        }
    }
}
