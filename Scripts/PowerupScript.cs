using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    public enum Power {
        grow,
        shrink,
        clone,
        gravity,
        makeMetal,
        makeNormal,
    }

    public Power type;
    public Sprite[] spriteArray;

    private bool available = true;
    private SpriteRenderer bubbleSpriteRenderer;

    void Start() {
        bubbleSpriteRenderer = transform.Find("Bubble").GetComponent<SpriteRenderer>();
        switch(type) {
            case Power.grow:
                bubbleSpriteRenderer.sprite = spriteArray[2];
                break;
            case Power.shrink:
                bubbleSpriteRenderer.sprite = spriteArray[5];
                break;
            case Power.gravity:
                bubbleSpriteRenderer.sprite = spriteArray[0];
                break;
            case Power.clone:
                bubbleSpriteRenderer.sprite = spriteArray[1];
                break;
            case Power.makeMetal:
                bubbleSpriteRenderer.sprite = spriteArray[3];
                break;
            case Power.makeNormal:
                bubbleSpriteRenderer.sprite = spriteArray[4];
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && available) {
            BallController playerScript = other.gameObject.GetComponent<BallController>();
            switch(type) {
                /*
                case Power.speedUp:
                    playerScript.increaseSpeed();
                    break;
                case Power.speedDown:
                    playerScript.decreaseSpeed();
                    break;
                */
                case Power.grow:
                    playerScript.increaseSize();
                    break;
                case Power.shrink:
                    playerScript.decreaseSize();
                    break;
                case Power.gravity:
                    playerScript.reverseGravity();
                    break;
                case Power.clone:
                    playerScript.clone();
                    break;
                case Power.makeMetal:
                    playerScript.updateState(BallController.state.metallic);
                    break;
                case Power.makeNormal:
                    playerScript.updateState(BallController.state.normal);
                    break;
            }
            playerScript.PlayPowerupSound();
            available = false;
            Destroy(gameObject);
        }
    }
}
