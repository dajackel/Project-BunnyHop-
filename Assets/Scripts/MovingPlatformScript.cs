using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float leftMost, rightMost;
    private Vector3 leftMostV, rightMostV;
    private Vector3 targetPosition;
    private bool waiting = false;
    // Start is called before the first frame update
    void Start()
    {
        leftMostV = new Vector3(leftMost, transform.position.y, transform.position.z);
        rightMostV = new Vector3(rightMost, transform.position.y, transform.position.z);
        targetPosition = rightMostV;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (!waiting && transform.position == targetPosition)
            switchDirection();
    }
    IEnumerator switchDirection()
    {
        waiting = true;
        print("switch called");
        targetPosition = (targetPosition == leftMostV) ? rightMostV : leftMostV;
        yield return new WaitForSeconds(0.5f);
        waiting = false;
    }
}
