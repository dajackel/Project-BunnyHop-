using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackgroumd : MonoBehaviour
{
    //target can be rigidbody2d component of a player or some object
    public Rigidbody2D target;
    //speed of scrolling
    public float speed;

    void FixedUpdate() {
        float targetVelocity = target.velocity.x;
        transform.Translate(new Vector3(-speed * targetVelocity, 0, 0) * Time.deltaTime);
    }

}
