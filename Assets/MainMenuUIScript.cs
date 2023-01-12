using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIScript : MonoBehaviour
{
    public void playGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }
}
