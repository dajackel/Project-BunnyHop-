using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Sprite audioON,audioOFF;
    [SerializeField] GameObject resetScoreWindow;
    [SerializeField] Button resetScorebutton;
    public void playGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }
    public void ToggleAudio()
    {
        float vol;
        mixer.GetFloat("MasterVolume", out vol);
        mixer.SetFloat("MasterVolume", (vol == -80.0f) ? 0.0f : -80.0f);
            Image audioSprite = GetComponentsInChildren<Image>()[1];
        if (vol == 0)
            audioSprite.sprite = audioOFF;
        else
            audioSprite.sprite = audioON;
    }
    public void TriggerHighscoreResetWindow()
    {
        Time.timeScale = 0f;
        resetScoreWindow.SetActive(true);
        resetScorebutton.interactable = false;
    }
    public void ResetHighScore()
    {
        Time.timeScale = 1.0f;
        PlayerPrefs.DeleteKey("HighScore");
    }
    public void CancelReset()
    {
        Time.timeScale = 1.0f;
        resetScoreWindow.SetActive(false);
        resetScorebutton.interactable = true;

    }
}
