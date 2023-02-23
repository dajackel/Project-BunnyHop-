using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class verticalScrollBackgroundScript : MonoBehaviour
{
    public GameObject cam;
    private float startPos, length;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        startPos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = cam.transform.position.y * (1 - parallaxEffect);
        float dist = cam.transform.position.y * parallaxEffect;
        transform.position = new Vector3(transform.position.x, startPos + dist, transform.position.z);
        if (temp > startPos + length)
            startPos += length;
        else if(temp < startPos - length)
            startPos -= length;
        //auto scrolling
        //cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(cam.transform.position.x , cam.transform.position.y + 0.5f, cam.transform.position.z), Time.deltaTime * 0.2f);
    }
}
