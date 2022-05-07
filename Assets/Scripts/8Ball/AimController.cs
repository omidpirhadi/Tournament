using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AimController : MonoBehaviour
{
    public float Step = 0.01f;
    public float DurationShowOrHideEnergyBar = 0.3f;
    private RectTransform Rect;
    private InfiniteScroll infiniteScroll;
    private ScrollRect scrollRect;
    private bool allowUse = false;
    private Diaco.Setting.GeneralSetting generalSetting;
    private void Start()
    {

        generalSetting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
        
        Rect = GetComponent<RectTransform>();
        infiniteScroll = GetComponent<InfiniteScroll>();
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(call => {
            if(infiniteScroll.positiveDrag)
            {
                if (allowUse)
                    Handler_OnChangeValueAimControll(Step * 1);
              //  Debug.Log("AimControllerChangeZ");
            }
            else
            {
                if (allowUse)
                    Handler_OnChangeValueAimControll(Step * -1);
               // Debug.Log("AimControllerChangez");
            }
        });
    }

    public void Show(bool show)
    {
        allowUse = false;
        if (show == false)
        {
            //if (generalSetting.Setting.billiardsettingdata.accuracyAimShow)
                Rect.DOAnchorPos(new Vector2(90, 0), DurationShowOrHideEnergyBar).OnComplete(() => { allowUse = true; });
        }
        else
        {
            if (generalSetting.Setting.billiardsettingdata.accuracyAimShow)
                Rect.DOAnchorPos(new Vector2(0, 0), DurationShowOrHideEnergyBar).OnComplete(() => { allowUse = true; });
        }
    }


    public event Action<float> OnChangeValueAimControll;

    public void Handler_OnChangeValueAimControll(float value)
    {
        if (OnChangeValueAimControll != null)
        {
            OnChangeValueAimControll(value);
        }
    }
}
