using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    private SceneTrackingScript SceneManager;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameManager GameManager;
    [SerializeField] GameObject ConfirmWindow;
    [SerializeField] GameObject LossWindow;
    [SerializeField] Button AudioButton;
    [SerializeField] Button RewardedAdButton;
    [SerializeField] AudioMixer masterMixer;
    private AudioSource audioSource;
    private bool isAudioOn;
    [SerializeField] Sprite AudioOn, AudioOff;
    [SerializeField] GameObject playerCurrHeight, playerHighScore;
    TextMeshPro pHeightText, pHighScoreText;
    public float pHighScore, bestHeightThisRun;
    public int extraLifeCount = 0;

    [SerializeField] Image levelFade;
    private bool changeLevel;
    private float fadeTime = 0;
    private float fadeDuration = 10f;
    private bool isFadeCompleted;
    private bool fadeInOut;
    private bool haveWatchedAd;


    private void Start()
    {
        haveWatchedAd = false;
        SceneManager = GameObject.FindGameObjectWithTag("PreviousScene").GetComponent<SceneTrackingScript>();
        if (SceneManager.getPreviousScene() == "MainMenu")
        {
            levelFade.color = Color.black;
            setFadeCompleted(false);
            changeLevel = true;
            fadeInOut = false;
        }
        else
        {
            levelFade.color = Color.clear;
            setFadeCompleted(true);
            changeLevel = false;
            fadeInOut = true;
        }
        bestHeightThisRun = 0;
        audioSource = GetComponent<AudioSource>();
        pHeightText = playerCurrHeight.GetComponent<TextMeshPro>();
        pHighScoreText = playerHighScore.GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        if (changeLevel)//if changing to a new scene
        {
            fadeTime += Time.unscaledDeltaTime;
            //check for scene difference if true
            //fade to scean else fade to black
            if (!getIsFadeCompleted())
                setFadeCompleted(fadeTransition(fadeInOut));//fade
            else
            {
                //fade finished
                if (levelFade.color == Color.black)
                {
                    //going back to main menu
                    audioSource.Stop();
                    GameManager.setGameState(GameManager.GAME_STATE.MAIN_MENU);
                }
                //just faded into game
                changeLevel = false;
            }
        }
        else
        {
            if (GameManager.bestHeightThisRun > bestHeightThisRun)
                bestHeightThisRun = GameManager.bestHeightThisRun;
            pHeightText.text = bestHeightThisRun.ToString("0.000");
        }
    }
    //send true or nothing to fade to black, send false to fade to screen
    private bool fadeTransition(bool fwdsbckwds)
    {
        Color intendedColor = fwdsbckwds ? Color.black : Color.clear;
        levelFade.color = Color.Lerp(levelFade.color, intendedColor, fadeTime / fadeDuration);
        if (levelFade.color == intendedColor)
            return true;

        return false;
    }
    public void TogglePauseMenu()
    {
        if (GameManager.getGameState() == GameManager.GAME_STATE.GAME_OVER)
            return;
        GameManager.setGameState((GameManager.getGameState() == GameManager.GAME_STATE.GAME_RUNNING) ? GameManager.GAME_STATE.GAME_PAUSED : GameManager.GAME_STATE.GAME_RUNNING);
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }
    public void ToggleConfirmWindow()
    {
        ConfirmWindow.SetActive(!ConfirmWindow.activeSelf);
    }
    public void ToggleAudio()
    {
        isAudioOn = !isAudioOn;
        float vol;
        masterMixer.GetFloat("MasterVolume", out vol);
        masterMixer.SetFloat("MasterVolume", (vol == -80.0f) ? 0.0f : -80.0f);

        AudioButton.image.sprite = (isAudioOn) ? AudioOff : AudioOn;
    }

    public void returnToMenu()
    {
        changeLevel = true;
        disableButtons();
        setFadeCompleted(false);
        changeLevel = true;
        fadeInOut = true;
        fadeTime = 0;
    }
    public void restartGame()
    {
        GameManager.setGameState(GameManager.GAME_STATE.GAME_RESTART);
    }

    public void updateHighScore(float hs)
    {
        pHighScore = hs; pHighScoreText.text = "Highscore: " + pHighScore.ToString("0.000");
    }

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
            if (!getHaveWatchedAd())
            {
                lossText[2].text = "";
                RewardedAdButton.gameObject.SetActive(true);
            }

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
        audioSource.Play();
    }

    public bool getIsFadeCompleted() { return isFadeCompleted; }
    private void setFadeCompleted(bool tf) { isFadeCompleted = tf; }

    public bool getHaveWatchedAd() { return haveWatchedAd; }

    public void setHaveWatchedAd(bool tf) { haveWatchedAd = tf; }
}
