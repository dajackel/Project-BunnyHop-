using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Camera cam;
    public float minYVal, maxYVal;
    public void Start(){
        cam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        cam.GetComponent<CameraScript>().minYDist = minYVal;
        cam.GetComponent<CameraScript>().maxYDist = maxYVal;
    }
}
