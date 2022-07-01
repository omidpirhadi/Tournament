using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyButtonInheader : MonoBehaviour
{
    private Button btn;
    private ServerUI server;
    public new string name = "";
   void OnEnable()

    {
        server = FindObjectOfType<ServerUI>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            server.Emit_CurrncyButton(name);
        });
    }
    void OnDisable()
    {
        btn.onClick.RemoveAllListeners();
    }
    void  OnDestory()
    {
        btn.onClick.RemoveAllListeners();
    }
}
