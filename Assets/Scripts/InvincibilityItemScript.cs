using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItemScript : MonoBehaviour
{
    private GameObject effectedObject;
    float duration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (duration <= 0){

        }
        else{
            duration-=Time.deltaTime;
        }
    }
}
