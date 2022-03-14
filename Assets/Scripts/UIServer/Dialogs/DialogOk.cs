using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogOk : MonoBehaviour
{
    public Text Context;
    public Button OkButton;


    public void OnEnable()
    {
        OkButton.onClick.AddListener(() =>
        {
            Handler_OnClickOk();
            CloseDialog();
        });
    }
    public void OnDisable()
    {
        OkButton.onClick.RemoveAllListeners();
    }
    public void ShowDialog(string Message)
    {

        Context.text = Message;
        this.gameObject.SetActive(true);
    }
    public void ShowDialog()
    {
        this.gameObject.SetActive(true);
    }
    public void CloseDialog()
    {
        this.gameObject.SetActive(false);
    }
    private Action clickok;
    public event Action OnClickOk
    {
        add { clickok += value; }
        remove { clickok += value; }
    }
    protected void Handler_OnClickOk()
    {
        if (clickok != null)
        {
            clickok();
        }
    }

}
