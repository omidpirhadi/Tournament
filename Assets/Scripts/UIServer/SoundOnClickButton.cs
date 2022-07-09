using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundOnClickButton : MonoBehaviour
{
    public SoundEffectControll soundEffectControll;
    private Button btn;
   
    void Awake()
    {
        btn = GetComponent<Button>();

    }
    void OnEnable()
    {
        soundEffectControll = GameObject.Find("SoundEffectlLayer2").GetComponent<SoundEffectControll>();
        btn = GetComponent<Button>();


        if (btn)
        {
            btn.onClick.AddListener(() =>
           {
               soundEffectControll.PlaySoundMenu(0);////click sound
              // Debug.Log("bbb";
           });
        }
    }

    void OnDisable()
    {

        if (btn)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}