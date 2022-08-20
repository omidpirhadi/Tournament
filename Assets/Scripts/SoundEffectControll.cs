using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectControll : MonoBehaviour
{
    public bool Mute = false;
    public List<AudioClip> audios;
   [SerializeField] private AudioSource audioSource;
    private Diaco.Setting.GeneralSetting setting;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
       // audios = new List<AudioClip>();
        audioSource = GetComponent<AudioSource>();
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
    /* void OnCollisionEnter(Collision obj)
     {
         if (obj.gameObject.tag == "marble" && Mute)
         {
             //  audioSource.clip = audios[0];
             // audioSource.Play();
         }
     }*/

    public void PlaySound(int index)
    {
        if (Mute  == false)
        {
            if (audioSource)
            {
                audioSource.clip = audios[index];
                audioSource.Play();
            }
        }
    }
    public void PlaySoundSoccer(int index)
    {
        if (Mute== false)
        {
            if (audioSource)
            {
                audioSource.clip = audios[index];
                audioSource.Play();
            }
        }
    }
    public void PlaySoundMenu(int index)
    {
        if (Mute== false)
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
            audioSource.clip = audios[index];
            audioSource.Play();
        }
    }
}
