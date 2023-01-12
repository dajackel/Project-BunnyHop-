using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigidBody;
    private Animator animator;
    private float speed = 3.0f,
        lifetime = 10.0f;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rigidBody.velocity = new Vector2(speed, Random.Range(-2, 2));
    }
    private void Update()
    {
        if (lifetime <= 0)
            animator.SetTrigger("DeathTrigger");
        else
            lifetime -= Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidBody.velocity = Vector2.zero;
        animator.SetTrigger("DeathTrigger");
    }
    private void destroyObject()
    {
        print("destroy called");
        Destroy(gameObject);
    }
}
