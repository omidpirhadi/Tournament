using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using LeTai.TrueShadow;
public class TabPageController : MonoBehaviour
{

    public List<Canvas> Tabs;
    public List<Image> ImageTabs;
    public Color Enable;
    public Color Disable;

    public int MinOrderPage = 0;
    public int MaxOrderPage = 1;
    public int ShowTabsDefalt = 0;

    private void OnEnable()
    {
        ShowTabs(ShowTabsDefalt);
    }
    public void ShowTabs(int Index)
    {int i = 0;
        Tabs.ForEach((tab) => {
            
            if (Index == i)
            {
               
                tab.sortingOrder = MaxOrderPage;
               // tab.enabled = true;
                ImageTabs[i].DOColor(Enable, 0.1f);
                if (ImageTabs[i].GetComponent<TrueShadow>())
                    ImageTabs[i].GetComponent<TrueShadow>().enabled = true;
                // Debug.Log("Show");
            }
            else
            {
                
                tab.sortingOrder = MinOrderPage;
               // tab.enabled = false;
                ImageTabs[i].DOColor(Disable, 0.1f);
                if (ImageTabs[i].GetComponent<TrueShadow>())
                    ImageTabs[i].GetComponent<TrueShadow>().enabled = false;
            }
            i++;
        });
    }
}
