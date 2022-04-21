using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialButton : MonoBehaviour
{

    public Button btn;
    public void OnEnable()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => {
            var server = FindObjectOfType<ServerUI>();
            server.RequestShowTutorialInWebview();
        });


    }
    public void OnDisable()
    {
        btn.onClick.RemoveAllListeners();
    }
    public void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }
}
