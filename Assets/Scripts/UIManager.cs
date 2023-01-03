using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject ConfirmWindow;
    [SerializeField] GameObject Player;
    [SerializeField] Button LeftButton,RightButton,AudioButton;
    [SerializeField] AudioMixer masterMixer;
    private float maxVol;
    private bool isAudioOn;
    [SerializeField] Sprite AudioOn, AudioOff, PressedButtonSprite;
    [SerializeField] GameObject playerCurrHeight, playerHighScore;
    TextMeshPro pHeightText, pHighScoreText;
    public float pCurrHeight, pHighScore;

    private void Start()
    {
        pHeightText = playerCurrHeight.GetComponent<TextMeshPro>();
        pHighScoreText = playerHighScore.GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        pHeightText.text = pCurrHeight.ToString("0.000");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
        var playerstats = Player.GetComponent<PlayerScript>();
    }
    public void ReturnToMenu(){
        SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
    public void TogglePauseMenu()
    {
        Time.timeScale=(Time.timeScale > 0) ? 0 : Time.timeScale = 1;
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }
    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }
    public void SaveGame()
    {
        var playerstats = Player.GetComponent<PlayerScript>();
        PlayerPrefs.Save();
    }
    public void ToggleConfirmWindow()
    {
        ConfirmWindow.SetActive(!ConfirmWindow.activeSelf);
    }
    public void ConfirmSettings()
    {
        //master volume settings PlayerPrefs.SetFloat("MasterVolume",x);
        //sfxvolume settings PlayerPrefs.SetFloat("SfxVolume",x);
        //musicvolume settings PlayerPrefs.SetFloat("MusicVolume",x);
    }
    public void ToggleAudio()
    {
        isAudioOn = !isAudioOn;
        float vol;
        masterMixer.GetFloat("MasterVolume", out vol);
        masterMixer.SetFloat("MasterVolume", (vol== -80.0f) ? 0.0f : -80.0f);

        AudioButton.image.sprite = (isAudioOn) ? AudioOff : AudioOn;
    }

    public void updateHighScore(float hs) { pHighScore = hs; pHighScoreText.text = pHighScore.ToString("0.000"); }
}
