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
    public float speed, bounceHeight, currHeight;
    private int extraLives = 0;
    private bool grounded = false, invincible = false;
    public bool lose = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] GameObject JumpEffect;
    private BoxCollider2D coll;
    private AudioSource audioSource;
    [SerializeField] AudioClip bounceSFX;
    //extra stats
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        if (invincible)
            if (spriteRenderer.color == Color.white)
                invincible = false;

        setPlayerHeight((transform.position.y + coll.size.y < 0) ? 0 : transform.position.y + coll.size.y);
        if (rigidbody.velocity.y <= 0)
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
        if (rigidbody.velocity.y > 0)
            return;
        //check overlap
        Vector2 pMinMax = new Vector2(coll.bounds.min.x - 0.05f, coll.bounds.max.x + -0.05f);
        Vector2 oMinMax = new Vector2(collision.collider.bounds.min.x, collision.collider.bounds.max.x);
        if (pMinMax.x < oMinMax.y && pMinMax.y > oMinMax.x && transform.position.y - 0.6f > collision.collider.bounds.max.y)
        {
            animator.SetBool("Charging", true);
            rigidbody.velocity = Vector2.zero;
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
            if (rigidbody.velocity.y <= 0)
            {
                collision.GetComponent<enemyScript>().setIsAlive(false);
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, bounceHeight - 6);//less height than normal
                animator.SetBool("Jumping", true);  //change sprites
                                                    //play sfx
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
        }
        else if (collTag == "Item")
        {
            string itemName = collision.name;
            if (itemName == "InvincibilityPowerUp")
                invincible = true;
            if (itemName == "ExtraLifePowerUp")
                extraLives++;
        }
        else
        {
            if (invincible)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, bounceHeight);//less height than normal
                animator.SetBool("Jumping", true);  //change sprites
                                                    //play sfx
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
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
                transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                rigidbody.velocity = new Vector2(-speed, rigidbody.velocity.y);
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                rigidbody.velocity = new Vector2(0.0f, rigidbody.velocity.y);
        }
    }

    IEnumerator landed()
    {
        yield return new WaitForSeconds(1);
        if (grounded)
        {
            //play sfx
            if (!audioSource.isPlaying)
                audioSource.Play();
            //jump
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, bounceHeight);
            //change sprites
            animator.SetBool("Charging", false);
            animator.SetBool("Jumping", true);
            grounded = false;
        }
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
        if (extraLives == 0)
            Debug.DebugBreak();
        else
        {
            extraLives--;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, bounceHeight);
            animator.SetBool("Jumping", true);  //change sprites
                                                //play sfx
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}
