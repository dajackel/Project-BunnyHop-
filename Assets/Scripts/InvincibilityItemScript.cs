using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItemScript : MonoBehaviour
{
    private GameObject effectedObject;
    [SerializeField] float duration;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (effectedObject != null)
        {
            effectedObject.GetComponent<SpriteRenderer>().color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.unscaledTime * 1, 1), 1, 1));

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
        if (!audioSource.isPlaying&&audioSource.enabled)
        {
            audioSource.Play();
            StartCoroutine(disableAudio());
        }
        gameObject.GetComponent<Animator>().SetTrigger("Item PickUp");
        effectedObject = collision.gameObject;
        transform.parent = null;
    }
    private void HideSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().forceRenderingOff = true;
    }
    IEnumerator disableAudio()
    {
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        audioSource.enabled = false;
    }
}
