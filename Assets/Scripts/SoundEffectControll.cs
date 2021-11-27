using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectControll : MonoBehaviour
{
    public bool Mute = false;
    public List<AudioClip> audios;
    private AudioSource audioSource;

    void Start()
    {
       // audios = new List<AudioClip>();
        audioSource = GetComponent<AudioSource>();
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
         audioSource.clip = audios[index];
         audioSource.Play();
    }
    public void PlaySoundSoccer(int index)
    {
        audioSource.clip = audios[index];
        audioSource.Play();
    }
    public void PlaySoundMenu(int index)
    {
        audioSource.clip = audios[index];
        audioSource.Play();
    }
}
