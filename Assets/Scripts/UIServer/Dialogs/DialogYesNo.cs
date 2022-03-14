using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogYesNo : MonoBehaviour
{
    public Text Context;
    public Button YESButton;
    public Button NoButton;
 
    private void OnEnable()
    {
        YESButton.onClick.AddListener(() => { Handler_OnClickYes(); CloseDialog(); });
        NoButton.onClick.AddListener(() => { Handler_OnClickNo(); CloseDialog(); });
    }
    public void OnDisable()
    {
        YESButton.onClick.RemoveAllListeners();
       NoButton.onClick.RemoveAllListeners();
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
    private Action clickyesno;
    public event Action OnClickYes
    {
        add { clickyesno += value; }
        remove { clickyesno -= value; }
    }

    protected void Handler_OnClickYes()
    {
        if(clickyesno != null)
        {
            clickyesno();
        }
    }
    private Action clickno;
    public event Action OnClickNo
    {
        add { clickno += value; }
        remove { clickno -= value; }
    }

    protected void Handler_OnClickNo()
    {
        if (clickno != null)
        {
            clickno();
        }
    }

}
