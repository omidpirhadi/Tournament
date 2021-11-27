using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayInCollision : MonoBehaviour
{

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

        if(obj.transform.tag == "ball")
        {
            PlaySound(0, speedVolume);

        }
        else if(obj.transform.tag == "wall")
        {
            PlaySound(1, speedVolume);
        }
    }
    public void PlaySound(int index, float volume)
    {
        audio.volume = volume;
        audio.clip = clips[index];
        audio.Play();
    }
}
