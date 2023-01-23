using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScoreHeights : MonoBehaviour
{
    [SerializeField] GameObject highscore, playerHeight;
    private UIManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<UIManager>();
        Vector3 pos = highscore.transform.position;
        highscore.transform.position = new Vector3 (pos.x, manager.pHighScore, pos.z);
        pos = playerHeight.transform.position;
        playerHeight.transform.position = new Vector3 (pos.x, manager.bestHeightThisRun, pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = playerHeight.transform.position;
        playerHeight.transform.position = new Vector3(pos.x, manager.bestHeightThisRun, pos.z);
        if (playerHeight.transform.position.y > highscore.transform.position.y) {
            highscore.transform.position = new Vector3(pos.x, pos.y, pos.z);
            if(playerHeight.activeSelf)
                playerHeight.SetActive(false);
        }
    }
}
