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
    private Rigidbody2D rigidBody;
    public float speed, bounceHeight, currHeight;
    public int extraLives = 0;
    private bool grounded = true, invincible = false;
    public bool lose = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    private AudioSource audioSource;

    private float fallSpeedIncInterval = 15f;
    private float fallSpeedIncAmount = 0.1f;
    private float timeSinceFallSpeedInc = 0.0f;

    //extra stats

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        //increase fall speed based on how long the run has been going
        if (fallSpeedIncInterval - timeSinceFallSpeedInc <= 0)
        {
            timeSinceFallSpeedInc = 0;
            Time.timeScale = (Time.timeScale * fallSpeedIncAmount >= 2) ? Time.timeScale : Time.timeScale + fallSpeedIncAmount;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        timeSinceFallSpeedInc += Time.deltaTime;
        if (invincible)
            if (spriteRenderer.color == Color.white)
                invincible = false;

        setPlayerHeight((transform.position.y + coll.size.y < 0) ? 0 : transform.position.y + coll.size.y);
        if (rigidBody.velocity.y <= 0 && !grounded)
        { /*player is now fallinng*/
            animator.SetBool("Jumping", false);
            coll.enabled = true;
        }

        if (!grounded)
            movePlayerX();

        flipPlayerSprite();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //early exit for edge case of hitting platforms perfectly to stop early
        if (rigidBody.velocity.y > 0)
            return;
        //check overlap
        Vector2 pMinMax = new Vector2(coll.bounds.min.x - 0.05f, coll.bounds.max.x + -0.05f);
        Vector2 oMinMax = new Vector2(collision.collider.bounds.min.x, collision.collider.bounds.max.x);
        if (pMinMax.x < oMinMax.y && pMinMax.y > oMinMax.x && transform.position.y - 0.6f > collision.collider.bounds.max.y)
        {
            animator.SetBool("Charging", true);
            rigidBody.velocity = Vector2.zero;
            StartCoroutine(landed());
            grounded = true;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collTag = collision.tag;
        if (collTag == "Enemy")
        {
            if (grounded)
                return;
            if (rigidBody.velocity.y <= 0)
            {
                collision.GetComponent<enemyScript>().setIsAlive(false);
                jump(bounceHeight - 6);
            }
        }
        else if (collTag == "Item")
        {
            //items are prefabs - remove clone addendum
            string itemName = collision.name.Substring(0, collision.name.Length - 7);
            if (itemName == "InvincibilityPowerUp")
            {
                //to avoid race set color
                spriteRenderer.color = Color.gray;
                invincible = true;
            }
        }
        else
        {
            if (invincible)
                jump();
            else
                lose = true;
        }
    }
    private void movePlayerX()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(touch.position);
                targetPos = new Vector3(targetPos.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 2 * speed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y);
        }
    }

    IEnumerator landed()
    {
        audioSource.PlayScheduled(AudioSettings.dspTime+ (0.5f / Time.timeScale));
        yield return new WaitForSeconds(1);
        if (grounded)
        {
            //change sprites
            animator.SetBool("Charging", false);
            jump();
        }
    }
    public void jump(float ovrRideVal = 0)
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(audioSource.clip);
        animator.SetBool("Jumping", true);
        rigidBody.velocity = (ovrRideVal != 0) ? 
            new Vector2(rigidBody.velocity.x, ovrRideVal) : new Vector2(rigidBody.velocity.x, bounceHeight);
        grounded = false;
    }

    private void setPlayerHeight(float height) { currHeight = height; }
    public float getPlayerHeight() { return currHeight; }

    private void flipPlayerSprite()
    {
        if (Input.touchSupported)
        {

            Touch touch = Input.GetTouch(0);
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(touch.position);
            float posDifference = targetPos.x - transform.position.x;
            if (posDifference > 0) //touch occured to the right of the player
                spriteRenderer.flipX = true;
            else //touch occured to the left of the player
                spriteRenderer.flipX = false;
        }
        else
        {
            if (spriteRenderer.flipX) //looking left
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            else if (!spriteRenderer.flipX)//looking right
            {
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    spriteRenderer.flipX = !spriteRenderer.flipX;
            }
            //else no change needed skip functionality entirely
        }
    }

    public int getExtraLifeCount() { return extraLives; }
    public void useExtraLife()
    {
        extraLives--;
        jump();
    }
}
