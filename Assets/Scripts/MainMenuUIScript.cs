using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Sprite audioON,audioOFF;
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

    }
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }
}
