using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogContentEdit : MonoBehaviour
{

    public Text Context;
    public InputField input;
    public Button SaveButton;

    public void OnEnable()
    {

        SaveButton.onClick.AddListener(() => { Handler_OnClickSave(); CloseDialog(); });
    }
    public void OnDisable()
    {
        SaveButton.onClick.RemoveAllListeners();
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
    public event Action OnClickSave;
    protected void Handler_OnClickSave()
    {
        if (OnClickSave != null)
        {
            OnClickSave();
        }
    }

}
