using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxSpeed;

    private Camera camera;
    private float startX;
    private float length;

    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = camera.transform.position.x * (1 - parallaxSpeed);
        float distance = camera.transform.position.x * parallaxSpeed;

        transform.position = new Vector3(startX + distance, transform.position.y, transform.position.z);

        if (temp > startX + length) startX += length;
        else if (temp < startX - length) startX -= length;
    }
}
