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
    public float speed, bounceHeight, jumpHeight, timeScale = 1, elapsedtime = 0.0f;
    public int totalbounces, totaljumps, movedir = 0;
    public bool paused = false;
    private bool grounded = false, charging = false, runTimer = true;
    private float chargetime = 0.0f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] GameObject JumpEffect;
    private ParticleSystem particle;
    private AudioSource audioSource;
    [SerializeField] AudioClip bounceSFX, jumpSFX;
    public float2 lastsafepos;
    //extra stats
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("InvokeTimer", 0, 0.01f);
    }
    void InvokeTimer()
    {
        if (runTimer)
            elapsedtime = elapsedtime + 0.01f;
    }
    public bool getRunTimer()
    {
        return runTimer;
    }
    public void setRunTimer(bool t)
    {
        runTimer = t;
    }
    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity.y <= 0)//player is now falling
            animator.SetBool("Jumping", false);
        if (charging && grounded)
        {
            chargetime += Time.deltaTime;

            if (!particle.isEmitting)
                particle.Play();
        }//count charge time and start particles


        #region FLIP PLAYER SPRITE
        if (Input.GetKey(KeyCode.A) && spriteRenderer.flipX || movedir == 1 && spriteRenderer.flipX)
            spriteRenderer.flipX = !spriteRenderer.flipX;

        else if (Input.GetKey(KeyCode.D) && !spriteRenderer.flipX || movedir == 2 && !spriteRenderer.flipX)
            spriteRenderer.flipX = !spriteRenderer.flipX;
        #endregion

        #region KEYBOARD MOVEMENT
        if (Input.GetKeyDown(KeyCode.Space))
        {//stop bounce start charging
            StopCoroutine(landed());
            charging = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && charging && grounded)
            chargeJump();

        else if (Input.GetKeyUp(KeyCode.Space))//they didn't meet the requirements but stopped holding space
            charging = false;
        #endregion
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 plat = collision.transform.position,
                player = transform.position;
        print(Vector3.Dot(plat,player));
        if (collision.gameObject.tag == "Ground")
        {
            rigidbody.velocity = Vector2.zero;
            grounded = true;
            if (!charging)
                StartCoroutine(landed());
        }
        if (charging)
            animator.SetBool("Charging", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            rigidbody.velocity = Vector2.zero;
            grounded = true;
            if (!charging)
                StartCoroutine(landed());
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
    public void chargeJump()
    {
        if (!grounded || !charging)
        {
            charging = false;
            return;
        }
        totaljumps++;
        if (audioSource.clip != jumpSFX) 
            audioSource.clip = jumpSFX;
        audioSource.Play();
        animator.SetBool("Charging", false); animator.SetBool("Jumping", true);
        particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        Instantiate(JumpEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), transform.rotation);
        if (chargetime >= 2.5f)
        { //highest / lvl 3
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpHeight * 2.0f); print("level 3");
        }
        else if (chargetime >= 1.5f)
        { //sec highest / lvl 2
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpHeight * 1.7f); print("level 2");
        }
        else if (chargetime >= 0.5f)
        { //first charge / lvl 1
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpHeight * 1.5f); print("level 1");
        }
        else
        { //no charge / lvl 0
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpHeight); print("level 0");
        }
        charging = grounded = false;
        movePlayerX();
        chargetime = 0;
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
        chargetime = 0.0f;
        if (grounded)
            StartCoroutine(landed());
    }

    public void setMoveDir(int x)
    {
        movedir = x;
    }
    IEnumerator landed()
    {
        totalbounces++;
        lastsafepos = new float2(transform.position.x, transform.position.y + 0.2f);
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
