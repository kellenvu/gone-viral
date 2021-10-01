using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour {
    public bool lockCamera = false;
    
    public GameObject groupOfTargets;
    public float panSmooth; // Smaller value will reach the target faster
    public float zoomSmooth; // Smaller value is faster
    public float minZoom;
    public float maxZoom;
    public float maxDistance;

    private List<Transform> targets = new List<Transform>();
    private Vector3 velocity; // This is a dummy variable for SmoothDamp
    private Camera cam;

    public float shiftSize = 1f;
    public Vector2 shiftPosition = new Vector2(0, 0);

    void Start() {
        cam = GetComponent<Camera>();
    }

    void LateUpdate() {
        targets.Clear();
        foreach(Transform child in groupOfTargets.transform) {
            targets.Add(child);
        }
        if(targets.Count == 0) {
            return;
        }

        if(Input.GetKey(KeyCode.LeftShift) || lockCamera) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(shiftPosition.x, shiftPosition.y, -1), 0.1f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, shiftSize, 0.1f);
        } else {
            Pan();
            Zoom();
        }
    }

    void Pan() {
        Vector3 centerPoint = GetCenterPoint();
        transform.position = Vector3.SmoothDamp(transform.position, centerPoint, ref velocity, panSmooth);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    void Zoom() {
        float t = Mathf.InverseLerp(0f, maxDistance, GetGreatestDistance());
        float newZoom = Mathf.Lerp(maxZoom, minZoom, t);
        cam.orthographicSize = Mathf.Lerp(newZoom, cam.orthographicSize, zoomSmooth); // Set the new zoom, with smoothing
    }

    Vector3 GetCenterPoint() {
        if(targets.Count == 1) {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++) {
            if(targets[i].tag == "Player") {
                bounds.Encapsulate(targets[i].position);
            }
        }

        return bounds.center;
    }

    // Returns the greatest distance that exists between the targets
    float GetGreatestDistance() {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++) {
            if(targets[i].tag == "Player") {
                bounds.Encapsulate(targets[i].position);
            }
        }

        return bounds.size.magnitude;
    }
}
