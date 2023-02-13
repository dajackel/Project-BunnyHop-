using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceShroomScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerScript playerscript = collision.gameObject.GetComponent<PlayerScript>();
        if (playerscript != null&&collision.gameObject.GetComponent<Rigidbody2D>().velocity.y<=0)
            playerscript.jump();
    }
}
