using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public GameObject target;
    [SerializeField] Vector2 baseOffset;
    Vector2 offset;

    private void Start() {
        offset = baseOffset;
    }

    private void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        var pos = transform.position;
        pos = Vector2.Lerp(pos, (Vector2)target.transform.position + offset, 0.95f);
        transform.position = pos;

        var rotation = transform.rotation;
        rotation = Quaternion.Lerp(rotation, target.transform.rotation, 1f);
        transform.rotation = rotation;

        var scale = transform.localScale;
        scale = Vector3.Lerp(scale, target.transform.localScale, 1f);
        transform.localScale = scale;

        offset = transform.localScale.x * baseOffset;
    }
}
