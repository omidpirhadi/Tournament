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
        Mute = setting.Setting.Sound;
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
        if (!Mute)
        {
            audio.clip = clips;
            audio.Play();
        }
    }
}
