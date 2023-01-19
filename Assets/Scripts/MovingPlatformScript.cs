using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 startPos, endPos;
    private Vector3 targetPosition;
    private bool waiting = false, canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = endPos;
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0)
            canMove = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        canMove = true;
    }
    void FixedUpdate()
    {
        if (!canMove)
            return;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
        if (!waiting && transform.localPosition == targetPosition)
            StartCoroutine(switchDirection());
    }
    IEnumerator switchDirection()
    {
        waiting = true;
        targetPosition = (targetPosition == startPos) ? endPos : startPos;
        yield return new WaitForSeconds(0.2f);
        waiting = false;
    }
}
