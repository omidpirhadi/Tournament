using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerMarble : MonoBehaviour
{
    
  
    private AudioSource audioSource;
    public bool Mute = false;
    public List<AudioClip> clips;
    public float MaxSpeed = 100;

    private new Rigidbody rigidbody;
    private new AudioSource audio;
    private Vector3 LastPosition;
    [SerializeField] private float speedVolume;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        LastPosition = transform.position;
    }
    private void FixedUpdate()
    {
       //speedVolume = Vector3.Distance(transform.position, LastPosition) / Time.deltaTime;
        
        LastPosition = this.transform.position;
    }
    private void OnCollisionEnter(Collision obj)
    {
        //var speedVolume = rigidbody.velocity.magnitude / MaxSpeed;
        var speedVolume = (Vector3.Distance(transform.position, LastPosition) / Time.deltaTime)/MaxSpeed;


        if (obj.transform.tag == "ball")
        {
            PlaySound(0, speedVolume);

        }
        else if (obj.transform.tag == "marble")
        {
            PlaySound(1, speedVolume);
        }
        else if (obj.transform.tag == "wall")
        {
            PlaySound(2, speedVolume);
        }

    }
    public void PlaySound(int index, float volume)
    {
       
        audio.volume = volume;
        audio.clip = clips[index];
        audio.Play();
    }
}
