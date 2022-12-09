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
    public float speed, bounceHeight, timeScale = 1;
    public int movedir = 0;
    private bool grounded = false, charging = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] GameObject JumpEffect;
    private ParticleSystem particle;
    private AudioSource audioSource;
    [SerializeField] AudioClip bounceSFX;
    //extra stats
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity.y <= 0)//player is now falling
            animator.SetBool("Jumping", false);
        if (charging && grounded&& !particle.isEmitting)
        {
                particle.Play();
        }//count charge time and start particles


        #region FLIP PLAYER SPRITE
        if (Input.GetKey(KeyCode.A) && spriteRenderer.flipX || movedir == 1 && spriteRenderer.flipX)
            spriteRenderer.flipX = !spriteRenderer.flipX;

        else if (Input.GetKey(KeyCode.D) && !spriteRenderer.flipX || movedir == 2 && !spriteRenderer.flipX)
            spriteRenderer.flipX = !spriteRenderer.flipX;
        #endregion
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float minx = collision.collider.bounds.min.x;
        float maxx = collision.collider.bounds.max.x;
        float playerx= transform.position.x;
        var pcoll = GetComponent<BoxCollider2D>();
        if(transform.position.y>collision.transform.position.y)
            if((playerx-0.5f*pcoll.bounds.max.x)<maxx|| (playerx + 0.5f * pcoll.bounds.max.x) < maxx)
            {
                rigidbody.velocity = Vector2.zero;
                grounded = true;
                if (!charging)
                    StartCoroutine(landed());
            }
        else
        {
            bool leftorright = collision.transform.position.x > transform.position.x;
            if (leftorright) transform.position = new Vector3(transform.position.x - 0.05f, transform.position.y); 
            else transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y);
        }
        if (charging)
            animator.SetBool("Charging", true);
    }
    private void movePlayerX()
    {
        if (Input.GetKey(KeyCode.A) || movedir == 1)
            rigidbody.velocity = new Vector2(-speed, rigidbody.velocity.y);
        else if (Input.GetKey(KeyCode.D) || movedir == 2)
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    }
    public void startChargeJump()
    {
        StopCoroutine(landed());
        charging = true;
    }
    public void stopChargeJump()
    {
        charging = false;
        particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        if (grounded)
            StartCoroutine(landed());
    }

    public void setMoveDir(int x)
    {
        movedir = x;
    }
    IEnumerator landed()
    {
        animator.SetBool("Charging", true);
        yield return new WaitForSeconds(0.5f);
        if (!charging && grounded)
        {
            animator.SetBool("Charging", false);
            animator.SetBool("Jumping", true);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, bounceHeight);
            grounded = false;
            if (audioSource.clip != bounceSFX) 
                audioSource.clip = bounceSFX;
            audioSource.Play();
            movePlayerX();
        }
    }
}
