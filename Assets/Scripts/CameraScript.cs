using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Player, RLevelBound, LLevelBound;
    public Vector2 curVelocity;
    private float maxXDist, minXDist, minYDist;
    private float screenWidth, screenHeight, screenRatio;
    void Start()
    {
        //set player control limits to screen width
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        screenRatio = screenWidth / screenHeight; //this determines the screen ratio (3:4=iPad_Tall, 9:16=iPhone5_Tall, etc)

        maxXDist = RLevelBound.transform.position.x - RLevelBound.transform.localScale.x/2 - (screenRatio * Camera.main.orthographicSize * 2) / 2;//minus widthof camera
        minXDist = LLevelBound.transform.position.x + LLevelBound.transform.localScale.x/2+ (screenRatio * Camera.main.orthographicSize  * 2)/2;//plus widthofcamera

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(Player.transform.position.x,minXDist,maxXDist),
                                         Mathf.Clamp(Player.transform.position.y,minYDist, Player.transform.position.y),
                                         transform.position.z);
    }
}
