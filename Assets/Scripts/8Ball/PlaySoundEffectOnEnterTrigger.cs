using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffectOnEnterTrigger : MonoBehaviour
{
    public bool Mute = false;
    public AudioClip clips;
    private new AudioSource audio;
    private Diaco.Setting.GeneralSetting setting;
    void Start()
    {

        audio = GetComponent<AudioSource>();
        setting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
        setting.OnChangeSetting += Setting_OnChangeSetting;
        if (setting.Setting.Sound == true)
            Mute = false;
        else
            Mute = true;
        
    }

    private void Setting_OnChangeSetting()
    {
        if (setting.Setting.Sound == true)
            Mute = false;
        else
            Mute = true;
    }

    private void OnTriggerEnter(Collider Ball)
    {
        if (Ball.tag == "ball" || Ball.tag == "whiteball")
        {
            PlaySound();
           
            
        }
    } 

    
    public void PlaySound()
    {
        if (Mute== false)
        {
            audio.clip = clips;
            audio.Play();
        }
    }
}
