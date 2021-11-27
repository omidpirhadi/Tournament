﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerSoccerBall : MonoBehaviour
{
    private AudioSource audioSource;
    public bool Mute = false;
    public List<AudioClip> clips;
    public float MaxSpeed = 100;

    private new Rigidbody rigidbody;
    private new AudioSource audio;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision obj)
    {
        var speedVolume = rigidbody.velocity.magnitude / MaxSpeed;


        if (obj.transform.tag == "wall")
        {
            if (!audio.isPlaying)
                PlaySound(0, speedVolume);
        }
        if (obj.transform.tag == "post")
        {
            if (!audio.isPlaying)
                PlaySound(1);
        }
    }
    public void PlaySound(int index, float volume)
    {
        audio.volume = volume;
        audio.clip = clips[index];
        audio.Play();
    }

    public void PlaySound(int index)
    {
        audio.volume = 1;
        audio.clip = clips[index];
        audio.Play();
    }
}
