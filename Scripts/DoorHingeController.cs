using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHingeController : MonoBehaviour
{
    Vector2 startingPosition;
    float startingZRotation;

    [SerializeField] Vector2 deltaPosition;
    // [SerializeField] float endingZRotation;
    [SerializeField] ButtonController associatedButton;

    private void Start() {
        startingPosition = transform.position;
        // startingZRotation = transform.rotation.z;
    }

    private void Update() {
        transform.position = Vector2.Lerp(startingPosition, startingPosition + deltaPosition, associatedButton.Progress);
        // transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(startingZRotation, endingZRotation, associatedButton.Progress));

        //Debug.Log(associatedButton.Progress);
    }
}
