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
    [SerializeField] Button RewardedAdButton;
    [SerializeField] AudioMixer masterMixer;
    private AudioSource audioSource;
    //private float maxVol;
    private bool isAudioOn;
    [SerializeField] Sprite AudioOn, AudioOff;
    [SerializeField] GameObject playerCurrHeight, playerHighScore;
    TextMeshPro pHeightText, pHighScoreText;
    public float pHighScore, bestHeightThisRun;
    public int extraLifeCount = 0;

    [SerializeField] Image levelFade;
    private bool changeLevel = false;
    private float fadeTime = 0;
    private float fadeDuration = 10f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pHeightText = playerCurrHeight.GetComponent<TextMeshPro>();
        pHighScoreText = playerHighScore.GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        if (changeLevel)
        {

            fadeTime += Time.unscaledDeltaTime;
            if (levelFade.color.a < 1)
            {
                levelFade.color = Color.Lerp(levelFade.color, Color.black, fadeTime / fadeDuration);
            }
            else
                GameManager.setGameState(GameManager.GAME_STATE.MAIN_MENU);
        }

        if (GameManager.bestHeightThisRun > bestHeightThisRun)
            bestHeightThisRun = GameManager.bestHeightThisRun;
        pHeightText.text = bestHeightThisRun.ToString("0.000");
    }
    public void TogglePauseMenu()
    {
        audioSource.Play();
        if (GameManager.getGameState() == GameManager.GAME_STATE.GAME_OVER)
            return;
        GameManager.setGameState((GameManager.getGameState() == GameManager.GAME_STATE.GAME_RUNNING) ? GameManager.GAME_STATE.GAME_PAUSED : GameManager.GAME_STATE.GAME_RUNNING);
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }
    //public void ToggleConfirmWindow()
    //{
    //    audioSource.Play();
    //    ConfirmWindow.SetActive(!ConfirmWindow.activeSelf);
    //}
    //public void ConfirmSettings()
    //{
    //    //master volume settings PlayerPrefs.SetFloat("MasterVolume",x);
    //    //sfxvolume settings PlayerPrefs.SetFloat("SfxVolume",x);
    //    //musicvolume settings PlayerPrefs.SetFloat("MusicVolume",x);
    //}
    public void ToggleAudio()
    {
        isAudioOn = !isAudioOn;
        float vol;
        masterMixer.GetFloat("MasterVolume", out vol);
        masterMixer.SetFloat("MasterVolume", (vol == -80.0f) ? 0.0f : -80.0f);

        AudioButton.image.sprite = (isAudioOn) ? AudioOff : AudioOn;
        audioSource.Play();
    }

    public void returnToMenu()
    {
        audioSource.Play();
        changeLevel = true;
    }
    public void restartGame()
    {
        GameManager.setGameState(GameManager.GAME_STATE.GAME_RESTART);
    }

    public void updateHighScore(float hs) { pHighScore = hs; pHighScoreText.text = "Highscore: " + pHighScore.ToString("0.000"); }

    public void lossScreenTrigger()
    {
        if (LossWindow.activeSelf)
        {
            LossWindow.SetActive(false);
            return;
        }
        //0 == highscore, 1 == best height this run, 2 == extra life count
        TextMeshProUGUI[] lossText = LossWindow.GetComponentsInChildren<TextMeshProUGUI>();
        lossText[0].text = "HighScore\n" + pHighScoreText.text;
        lossText[1].text = "Best Height This Run\n" + bestHeightThisRun.ToString("0.000");
        lossText[2].text = "x" + extraLifeCount.ToString();
        //0 == restart, 1 == backtomenu, 2 == extralife
        Button[] lossUIImages = LossWindow.GetComponentsInChildren<Button>();
        if (extraLifeCount <= 0)
        {
            lossUIImages[2].interactable = false;
            Color tempColor = lossText[2].faceColor;
            tempColor.a = 50;
            lossText[2].faceColor = tempColor;
            RewardedAdButton.gameObject.SetActive(true);
        }
        else
        {
            lossUIImages[2].interactable = true;
            Color tempColor = lossText[2].faceColor;
            tempColor.a = 256;
            lossText[2].faceColor = tempColor;
        }
        LossWindow.SetActive(true);
    }
    public void Continue()
    {
        GameManager.setGameState(GameManager.GAME_STATE.GAME_CONTINUE);
    }
}
