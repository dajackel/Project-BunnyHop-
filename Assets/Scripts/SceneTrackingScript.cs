using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrackingScript : MonoBehaviour
{
    string previousScene = "";
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public string getPreviousScene() { return previousScene; }
    public string getCurrentScene() { return SceneManager.GetActiveScene().name; }

    public void LoadScene(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
