using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameManager GameManager;
    [SerializeField] GameObject ConfirmWindow;
    [SerializeField] GameObject LossWindow;
    [SerializeField] Button AudioButton;
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
    public void TogglePauseMenu()
    {
        GameManager.setGameState((GameManager.getGameState() == GameManager.GAME_STATE.GAME_RUNNING) ? GameManager.GAME_STATE.GAME_PAUSED : GameManager.GAME_STATE.GAME_RUNNING);
        PauseMenu.SetActive(!PauseMenu.activeSelf);
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

    public void returnToMenu()
    {
        GameManager.setGameState(GameManager.GAME_STATE.MAIN_MENU);
    }
    public void restartGame()
    {
        GameManager.setGameState(GameManager.GAME_STATE.GAME_START);
    }

    public void updateHighScore(float hs) { pHighScore = hs; pHighScoreText.text = pHighScore.ToString("0.000"); }

    public void lossScreenTrigger()
    {
        LossWindow.SetActive(true);
        //0 == highscore, 1 == best height this run
        TextMeshProUGUI[] lossText = LossWindow.GetComponentsInChildren<TextMeshProUGUI>();
        lossText[0].text = "HighScore\n"+ pHighScoreText.text;
        lossText[1].text = "Best Height This Run\n" + GameManager.bestHeightThisRun.ToString("0.000");


    }
}
