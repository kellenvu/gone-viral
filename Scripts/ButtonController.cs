using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject buttonTop;
    [SerializeField] float unpressedY;
    [SerializeField] float pressedY;

    float progress = 0f;

    public float Progress {
        get { return progress; }
    }


    int ballCount = 0;

    private void Update() {
        if(ballCount > 0) {
            PressedBehavior();
        } else {
            UnpressedBehavior();
        }

        float y = Mathf.Lerp(unpressedY, pressedY, progress);
        buttonTop.transform.position = new Vector2(transform.position.x, y + transform.position.y);

        //Debug.Log($"{gameObject.name}'s progress is {Progress}.");
    }

    void PressedBehavior() {
        progress = Mathf.Lerp(progress, 1f, 0.1f);
    }

    void UnpressedBehavior() {
        progress = Mathf.Lerp(progress, 0f, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            ballCount += 1;
            //Debug.Log("Added a ball.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            ballCount -= 1;
            Debug.Log("Removed a ball.");
        }
    }
}
