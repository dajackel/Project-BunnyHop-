using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigidBody;
    private Animator animator;
    private AudioSource audioSource;
    public float speed = 3.0f,
        lifetime = 10.0f;
    private bool isAlive = true;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (lifetime <= 0)
            destroyObject();
        else
            lifetime -= Time.deltaTime;
    }
    private void destroyObject()
    {
        Destroy(gameObject);
    }

    public bool getIsAlive() { return isAlive; }
    public void setIsAlive(bool tf)
    {
        isAlive = tf;
        if (!isAlive)
        {
            animator.SetTrigger("DeathTrigger");
            rigidBody.velocity = Vector2.zero;
        }
    }
    public void playDeathAudioClip()
    {
        if (!getIsAlive())
            audioSource.Play();
    }
}
