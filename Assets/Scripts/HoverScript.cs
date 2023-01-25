using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    private Vector3 startPos, endPos;
    private Vector3 targetPos;
    private float speed=2;
    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);
        targetPos = endPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.unscaledDeltaTime);
        if (transform.position == targetPos)
            StartCoroutine(switchDirection());
    }

    IEnumerator switchDirection()
    {
        targetPos = (targetPos == startPos) ? endPos : startPos;
        yield return null;
    }
}
