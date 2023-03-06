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
        Application.runInBackground = false;
    }
    public string getPreviousScene() { return previousScene; }
    public string getCurrentScene() { return SceneManager.GetActiveScene().name; }

    public void LoadScene(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("DEBUG: Application paused");
            // we are in background
        }
        else
        {
            Debug.Log("DEBUG: Application Resumed");
            // we are in foreground again.
        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("DEBUG: Application Quit");
    }
    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("DEBUG: Does Application have focus is "+ focus);
    }
}
