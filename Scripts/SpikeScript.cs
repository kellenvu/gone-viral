using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private AudioSource myAudio;
    private List<AudioClip> screams = new List<AudioClip>();
    private AudioClip splashSound;

    private void Start() {
        myAudio = GetComponent<AudioSource>();
        screams.Add(Resources.Load("Sounds/scream-1") as AudioClip);
        screams.Add(Resources.Load("Sounds/scream-2") as AudioClip);
        screams.Add(Resources.Load("Sounds/scream-3") as AudioClip);

        //splashSound = Resources.Load("Sounds/Water Splash") as AudioClip;
        //myAudio.clip = splashSound;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            var player = other.gameObject.GetComponent<BallController>();

            if (player.TakesSpikeDamage) {
                player.die();
                myAudio.PlayOneShot(screams[Random.Range(0, screams.Count)], 0.2f);
                //myAudio.PlayOneShot(splashSound);
            }
        }
    }
}
