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
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (!waiting && transform.position == targetPosition)
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
