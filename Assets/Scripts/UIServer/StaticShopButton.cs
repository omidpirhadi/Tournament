using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticShopButton : MonoBehaviour
{
    public string Name;
    public int Price;
    private ServerUI server;
    private Button btn;
    void OnEnable()
    {
        server = FindObjectOfType<ServerUI>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => {

            server.Emit_ShopT2Button(Name, Price);
           
        });
    }
    void OnDisable()
    {
        btn.onClick.RemoveAllListeners();
    }

}

