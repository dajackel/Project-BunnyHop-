using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MainMenuCameraScript
{
    // Start is called before the first frame update
    [SerializeField] GameObject Player, RLevelBound, LLevelBound, BLevelBound;
    public Vector2 curVelocity;
    private float minYDist = 6.67f;

    // Update is called once per frame
    void Update()
    {
        if (minYDist < BLevelBound.transform.position.y + 22f)
            minYDist = BLevelBound.transform.position.y + 22f;
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Mathf.Clamp(Player.transform.position.y, minYDist, Player.transform.position.y), transform.position.z), Time.deltaTime);
    }
}
