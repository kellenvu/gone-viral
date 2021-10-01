using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlagScript : MonoBehaviour
{
    // public GameObject groupOfPlayers;
    public int numRemaining;
    [SerializeField] TextMeshProUGUI myLabel;
    [SerializeField] SpriteRenderer myCheck;

    private AudioSource myAudio;
    private AudioClip dingSound;

    private void Start() {
        myCheck.enabled = false;
        myAudio = GetComponent<AudioSource>();
        dingSound = Resources.Load("Sounds/Ding") as AudioClip;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player") && numRemaining > 0) {
            numRemaining -= 1;
            Destroy(collision.gameObject);
            myAudio.PlayOneShot(dingSound, 0.1f);
        }
    }

    private void Update() {
        if (numRemaining == 0) {
            myLabel.text = "";
        } else {
            myLabel.text = numRemaining.ToString();
        }

        if (numRemaining == 0 && !myCheck.enabled) {
            myCheck.enabled = true;
        }
    }



    /*
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && CountNearbyPlayers() >= minToWin) {
            TransitionController.instance.Transition("SampleScene");
        }
    }

    int CountNearbyPlayers() {
        int count = 0;
        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, winRadius);
        foreach (Collider2D collider in nearbyColliders) {
            if (collider.gameObject.tag == "Player") {
                count++;
            }
        }
        return count;
    }
    */
}
