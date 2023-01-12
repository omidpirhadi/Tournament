using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticShop : MonoBehaviour
{
    private ServerUI server;

    public Text[] PriceText;
    void OnEnable()
    {
        server = FindObjectOfType<ServerUI>();
        server.Emit_StaticShopUpdate();

    }
    void OnDisable()
    {
       
    }
    public void Set(ItemPriceData data)
    {
        for (int i = 0; i < data.price.Count; i++)
        {
            PriceText[i].text = data.price[i].ToString();
        }
    }

    public struct ItemPriceData
    {
        public List<int> price;
    }
}
