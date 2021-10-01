using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutodestroyParticles : MonoBehaviour
{
    ParticleSystem ps;

    private void Start() {
        ps = GetComponent<ParticleSystem>();
    }

    void Update() {
        if(ps) {
            if(!ps.IsAlive()) {
                Destroy(gameObject);
            }
        }
    }
}
