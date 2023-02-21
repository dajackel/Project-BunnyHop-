using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Sprite audioON, audioOFF;
    [SerializeField] GameObject resetScoreWindow;
    [SerializeField] Button resetScorebutton;
    [SerializeField] Image levelFade;
    private bool changeLevel = false;
    private float fadeTime = 0;
    private float fadeDuration = 9f;
    public void playGame()
    {
        disableButtons();
        GetComponent<AudioSource>().Play();
        changeLevel = true;
    }
    private void Update()
    {
        if (changeLevel)
        {
            fadeTime += Time.unscaledDeltaTime;
            if (levelFade.color.a < 1)
            {
                levelFade.color = Color.Lerp(levelFade.color, Color.black, fadeTime / fadeDuration);
                print(levelFade.color.a);
            }
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
        }
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
        resetScoreWindow.SetActive(false);
        PlayerPrefs.SetFloat("HighScore", 0.0f);
    }
    public void CancelReset()
    {
        Time.timeScale = 1.0f;
        resetScoreWindow.SetActive(false);
        resetScorebutton.interactable = true;

    }

    private void disableButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            btn.interactable = false;
        }
    }

    public void playInteractAudio()
    {
        GetComponent<AudioSource>().Play();
    }
}
