using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifeItemScript : MonoBehaviour
{
   private AudioSource audioSource;
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            StartCoroutine(destroyObject());
        }
        gameObject.GetComponent<SpriteRenderer>().forceRenderingOff = true;
    }
    IEnumerator destroyObject()
    {
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        Destroy(gameObject);
    }
}
