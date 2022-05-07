using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[ExecuteInEditMode]
public class Test_UI : MonoBehaviour
{
    public ScrollRect scrollRect;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(x => {

            Debug.Log($"min H {scrollRect.minHeight} , min W{scrollRect.minWidth}, Perf w{ scrollRect.preferredWidth}, Perf H{ scrollRect.preferredHeight}");
        });

    }
}
