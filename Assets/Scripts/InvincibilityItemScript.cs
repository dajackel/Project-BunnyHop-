using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItemScript : MonoBehaviour
{
    private GameObject effectedObject;
    [SerializeField] float duration;
    void Update()
    {
        if (effectedObject != null)
        {
            effectedObject.GetComponent<SpriteRenderer>().color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 1, 1), 1, 1));

            if (duration <= 0)
            {
                effectedObject.GetComponent<SpriteRenderer>().color = Color.white;
                Destroy(gameObject);
            }
            else
                duration -= Time.unscaledDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();
            StartCoroutine(disableAudio());
        }
        gameObject.GetComponent<Animator>().SetTrigger("Item PickUp");
        effectedObject = collision.gameObject;
        transform.parent = null;
    }
    private void hideSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().forceRenderingOff = true;
    }
    IEnumerator disableAudio()
    {
        var audio = gameObject.GetComponent<AudioSource>();
        yield return new WaitUntil(() => audio.isPlaying == false);
        audio.enabled = false;
    }
}
