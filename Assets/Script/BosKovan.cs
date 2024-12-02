using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosKovan : MonoBehaviour
{
    AudioSource yereDusmeSesi;
    void Start()
    {
        yereDusmeSesi = GetComponent<AudioSource>();
        Destroy(gameObject,2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Yol"))
        {
            yereDusmeSesi.Play();
            if (!yereDusmeSesi.isPlaying) {
                Destroy(gameObject); 
                    }
          
        }   
    }
   
}
