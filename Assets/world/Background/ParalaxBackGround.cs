using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackGround : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float paralaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float camX = cam.transform.position.x;

        float parallaxOffset = camX * paralaxEffect;
        float cameraOffset = camX * (1 - paralaxEffect);

        transform.position = new Vector3(startPos + parallaxOffset, transform.position.y, transform.position.z);

        if (cameraOffset > startPos + length)
        {
            startPos += length;
        }
        else if (cameraOffset < startPos - length)
        {
            startPos -= length;
        }
    }
}
