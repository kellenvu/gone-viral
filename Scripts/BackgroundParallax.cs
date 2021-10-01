using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxMultiplier = 1f;
    float cameraStartPosX;

    float startPosX;

    float backgroundWidth;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraStartPosX = cameraTransform.position.x;
        startPosX = transform.position.x;

        backgroundWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = cameraTransform.position.x - cameraStartPosX;
        //float distNotMoved = dist * (1 - parallaxMultiplier);
        transform.position = new Vector3(startPosX + dist * parallaxMultiplier, transform.position.y, transform.position.z);

        /*
        if(distNotMoved > startPosX + backgroundWidth)
        {
            startPosX += backgroundWidth;
        }
        else if(distNotMoved < startPosX - backgroundWidth)
        {
            startPosX -= backgroundWidth;
        }
        */

        if(cameraTransform.position.x - transform.position.x > backgroundWidth/2)
        {
            startPosX += backgroundWidth;
        }
        else if(cameraTransform.position.x - transform.position.x < -backgroundWidth/2)
        {
            startPosX -= backgroundWidth;
        }
    }
}
