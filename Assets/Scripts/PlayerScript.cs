using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct float2
{
    public float2(float xval, float yval) : this()
    {
        float x = xval;
        float y = yval;
    }
    public float x { get; set; }
    public float y { get; set; }
}
public class PlayerScript : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    public float speed, bounceHeight;
    public int movedir = 0;
    private bool grounded = false, charging = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] GameObject JumpEffect;
    private BoxCollider2D coll;
    private ParticleSystem particle;
    private AudioSource audioSource;
    [SerializeField] AudioClip bounceSFX;
    //extra stats
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity.y <= 0)
        { /*player is now fallinng*/
            animator.SetBool("Jumping", false);
            coll.enabled = true;
        }
        #region FLIP PLAYER SPRITE
        if (Input.GetKey(KeyCode.A) && spriteRenderer.flipX || movedir == 1 && spriteRenderer.flipX)
            spriteRenderer.flipX = !spriteRenderer.flipX;

        else if (Input.GetKey(KeyCode.D) && !spriteRenderer.flipX || movedir == 2 && !spriteRenderer.flipX)
            spriteRenderer.flipX = !spriteRenderer.flipX;
        #endregion
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.position.y > collision.transform.position.y)
        {
            animator.SetBool("Charging", true);
            rigidbody.velocity = Vector2.zero;
            StartCoroutine(landed());
            grounded = true;
        }
    }
    private void movePlayerX()
    {
        if (Input.GetKey(KeyCode.A) || movedir == 1)
            rigidbody.velocity = new Vector2(-speed, rigidbody.velocity.y);
        else if (Input.GetKey(KeyCode.D) || movedir == 2)
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    }

    public void setMoveDir(int x)
    {
        movedir = x;
    }
    IEnumerator landed()
    {
        yield return new WaitForSeconds(1);
        if (!charging && grounded)
        {
            //jump
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, bounceHeight);
            //change sprites
            animator.SetBool("Charging", false);
            animator.SetBool("Jumping", true);
            grounded = false;
            //play sfx
            if (audioSource.clip != bounceSFX)
                audioSource.clip = bounceSFX;
            audioSource.Play();
            movePlayerX();
        }
    }
}
