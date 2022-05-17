using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExitAppPopup : MonoBehaviour
{
    public Button close_btn;
    public Button exit_btn;
    public void OnEnable()
    {
        
        exit_btn.onClick.AddListener(() => {
            Application.Quit(); 
        });
        close_btn.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });

    }
    public void OnDisable()
    {
        exit_btn.onClick.RemoveAllListeners();
        close_btn.onClick.RemoveAllListeners();
    }
    public void OnDestroy()
    {
        exit_btn.onClick.RemoveAllListeners();
        close_btn.onClick.RemoveAllListeners();
    }
}
