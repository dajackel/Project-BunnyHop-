using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Player, RLevelBound, LLevelBound, BLevelBound;
    public Vector2 curVelocity;
    private float minYDist = 6.67f;
    void Start()
    {
        switch (Camera.main.aspect)
        {
            case 480.0f / 800.0f:
                Camera.main.orthographicSize = 17.87372f;
                //18.00567
                break;
            case 720.0f / 1280.0f:
                Camera.main.orthographicSize = 19.11508f;
                break;
            case 1440.0f / 2960.0f:
                Camera.main.orthographicSize = 22.14385f;
                break;
            case 1080.0f / 2160.0f:
                Camera.main.orthographicSize = 21.57014f;
                break;
            default:
                if (Camera.main.aspect - 0.5008292f < 0)
                    Camera.main.orthographicSize = 21.56672f;
                else
                    Camera.main.orthographicSize = 19.13686f;
                break;
        }
        if (Camera.main.pixelWidth == 720.0f)
        {
            Camera.main.orthographicSize = 23.96643f;
            transform.position = new Vector3(transform.position.x, 11.32f, transform.position.z);
            minYDist = 11.32f;
        }
        else if (Camera.main.pixelWidth == 1200.0f)
        {
            Camera.main.orthographicSize = 17.28737f;
            //transform.position = new Vector3(transform.position.x, 11.32f, transform.position.z);
            //minYDist = 11.32f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (minYDist < BLevelBound.transform.position.y + 18.0f)
            minYDist = BLevelBound.transform.position.y + 18.0f;
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Mathf.Clamp(Player.transform.position.y, minYDist, Player.transform.position.y), transform.position.z), Time.deltaTime);
        //transform.position = new Vector3(0.0f, Mathf.Clamp(Player.transform.position.y, minYDist, Player.transform.position.y), transform.position.z);
    }
}
