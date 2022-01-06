using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballinbasket : MonoBehaviour
{
    public int BallID = -1;


    public bool Mute = false;
    public AudioClip clips;
    private new AudioSource audio;

    public Vector3 LastPosition;
    void Start()
    {

        audio = GetComponent<AudioSource>();

    }
    private void Update()
    {
        if(CheckMoveBall())
        {
            PlaySound();
        }
        if(!CheckMoveBall())
        {
            audio.Stop();
        }
        LastPosition = this.transform.position;
    }

    private bool EqeulPosition(Vector3 a, Vector3 b)
    {
        bool eqeul = false;
        var x = a.x - b.x;
        var z = a.z - b.z;
        if (x == 0.0f && z == 0.0f)
        {
            eqeul = true;
        }
        //  LastPosition = this.transform.position;
        return eqeul;

    }
    private bool CheckMoveBall()
    {
        var move = false;
        var domove = EqeulPosition(transform.position, LastPosition);
        

        if (domove == false )
        {

            move = true;
        }

        return move;
    }
   private void PlaySound()
    {
        if (!Mute)
        {
            if (!audio.isPlaying)
            {
                audio.clip = clips;
                audio.Play();
            }
        }
    }
}
