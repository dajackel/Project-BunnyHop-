using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollBackgroundScript : MonoBehaviour
{
    public GameObject cam;
    private float startPos, length;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        if (temp > startPos + length)
            startPos += length;
        else if(temp < startPos - length)
            startPos -= length;
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(cam.transform.position.x + 0.5f, cam.transform.position.y, cam.transform.position.z),Time.deltaTime*0.2f);
    }
}
