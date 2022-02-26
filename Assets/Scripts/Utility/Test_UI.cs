using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[ExecuteInEditMode]
public class Test_UI : MonoBehaviour
{

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { Debug.Log ("Click"); });
    }
}
