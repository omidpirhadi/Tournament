using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayeAnimationUI : MonoBehaviour
{
    public new Animation animation;
    public bool AutoMoveUpInNotifications = false;
    public float TimeShowNotification = 5.0f;
    [SerializeField] public bool UP = false;
    [SerializeField] public bool Down = false;

    public void Start()
    {
        animation = GetComponent<Animation>();
    }
    void OnEnable()
    {
        animation = GetComponent<Animation>();
    }
    public void playanimation()
    {
       /// Debug.Log("UCCCCC");
        if (UP == false && Down == true)
        {

            animation.Play("Up");
            Down = false;
            UP = true;
           // Debug.Log("UP");
        }
        else if (UP == true && Down == false)
        {
            animation.Play("Down");

            if(AutoMoveUpInNotifications)
            {

                DOVirtual.Float(0, 1, TimeShowNotification, (x) =>
                {

                }).OnComplete(() => {
                    if (UP == false)
                    {
                        animation.Play("Up");
                        Down = false;
                        UP = true;
                    //    Debug.Log("UPzzzz");
                    }
                });
            }
            Down = true;
            UP = false;
           /// Debug.Log("Down");
        }
        else if (UP == false && Down == false)
        {
            animation.Play("Up");
            UP = true;
            //Debug.Log("CCC");
        }
    }
}
