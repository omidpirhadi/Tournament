using System.Collections;
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
    private Vector3 LastPosition;
    private Diaco.Setting.GeneralSetting setting;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        setting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
        Mute = setting.Setting.Sound;
        LastPosition = transform.position;
    }
    private void FixedUpdate()
    {
        LastPosition = this.transform.position;
    }

    private void OnCollisionEnter(Collision obj)
    {
        ///var speedVolume = rigidbody.velocity.magnitude / MaxSpeed;
        var speedVolume = (Vector3.Distance(transform.position, LastPosition) / Time.deltaTime)/MaxSpeed;

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
