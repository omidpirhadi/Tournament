using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store
{
    public class Store : MonoBehaviour
    {

        public ServerUI Server;
        public NavigationUI navigationUI;

        public Button SoccerShopButton;
        public Button BilliardShopButton;

        public Text Special_1;
        public Text Special_2;

        [SerializeField]
        public AwardElement Award1;
        [SerializeField]
        public AwardElement Award2;
        [SerializeField]
        public List<GemElement> GreenGemElements;
        [SerializeField]
        public List<GemElement> BlueGemElements;
        private void Awake()
        {
            Server.OnShopLoaded += Server_OnShopLoaded;
        }
        private void OnEnable()
        {
          
        }

        private void Server_OnShopLoaded(HTTPBody.Shop shopdata)
        {
            InitializeShopPage(shopdata);
        }

        public void InitializeShopPage(HTTPBody.Shop data)
        {
            Special_1.text = data.special[0].name;
            Special_2.text = data.special[1].name;
            Award1.Set("data.Box"+"11:55:00");
            Award2.Set2(data.Box2.current, data.Box2.max);
           
            for(int i = 0; i<data.gemsProducts.Count; i++)
            {
                if (i < 3)
                    GreenGemElements[i].set(data.gemsProducts[i].price.ToString() + "T", ()=> { Debug.Log("clickedGreen"); });
                else
                    BlueGemElements[i].set(data.gemsProducts[i].price.ToString() + "T", () => { Debug.Log("clickedBlue"); });
            }

        }

    }
    [Serializable]
    public struct AwardElement
    {

        public Text RemainderTime;
        public Text IndicatorValue;
        public Image Prograssbar;
        public float CurrentValue;
        public float MaxValue;
        
        public void Set(string remainderTime)
        {

            RemainderTime.text = remainderTime;
        }
        public void Set2(float current , float max)
        {
            var value = current / max;
            Prograssbar.fillAmount = value;
            IndicatorValue.text = current + "/" + max;
        }
    }
    [Serializable]
    public struct GemElement
    {
        public Button Gems_btn;
        public Text Gem_text;
        public void set(string price , Action onclick)
        {
            Gem_text.text = price;
            Gems_btn.onClick.AddListener(() => { onclick(); });
        }
    }
}