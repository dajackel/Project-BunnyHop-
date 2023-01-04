using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Player, RLevelBound, LLevelBound;
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
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(/*Mathf.Clamp(Player.transform.position.x,minXDist,maxXDist)*/0.0f,
                                         Mathf.Clamp(Player.transform.position.y, minYDist, Player.transform.position.y),
                                         transform.position.z);
    }
}
