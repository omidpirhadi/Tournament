﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostSoundManager : MonoBehaviour
{
    public bool SoundEnable = true;
    public AudioClip[] audios;
    private AudioSource audioSource;

    void Start()
    {
        //audios = new AudioClip[2];
        audioSource = GetComponent<AudioSource>();

    }

    void OnCollisionEnter(Collision obj)
    {
      if (obj.gameObject.tag == "ball"  && SoundEnable)
        {
            audioSource.clip = audios[0];
            audioSource.Play();
        }
    }
}
