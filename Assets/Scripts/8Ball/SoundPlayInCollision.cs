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
    private Vector3 LastPosition;

    private Diaco.Setting.GeneralSetting setting;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        setting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
        LastPosition = transform.position;
        Mute = setting.Setting.Sound;
    }
    private void FixedUpdate()
    {
        LastPosition = this.transform.position;
    }




    private void OnCollisionEnter(Collision obj)
    {
        //var speedVolume = rigidbody.velocity.magnitude / MaxSpeed;
        var speedVolume = (Vector3.Distance(transform.position, LastPosition) / Time.deltaTime) / MaxSpeed;
        if (obj.transform.tag == "ball")
        {
            PlaySound(0, speedVolume);

        }
        else if (obj.transform.tag == "wall")
        {
            PlaySound(1, speedVolume);
        }
    }
    public void PlaySound(int index, float volume)
    {
        if (!Mute)
        {
            audio.volume = volume;
            audio.clip = clips[index];
            audio.Play();
        }
    }
}
