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
    [SerializeField] TextMeshProUGUI BounceCountText;
    [SerializeField] TextMeshProUGUI JumpCountText;
    [SerializeField] TextMeshProUGUI TimerText;
    [SerializeField] Button LeftButton,RightButton,ChargeButton,AudioButton;
    [SerializeField] AudioMixer masterMixer;
    private float maxVol;
    private bool isAudioOn, LOn=false,ROn=false,COn = false;
    [SerializeField] Sprite AudioOn, AudioOff, PressedButtonSprite;

    //private TextMeshProUGUI tmpInput;
    //private string playerStats;
    private PlayerScript pScript;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        pScript=Player.GetComponent<PlayerScript>();
    }
    private string timeConversion(float time){
        string convertedTime="";
        int hours=0, minutes=0;
        //print(time);
        //time = ((float)Mathf.Floor(time * 1000) / 1000);
        if (time >= 60)
        {
            minutes = (int)time / 60;
            time -= minutes * 60;
            if (minutes >= 60)
            {
                hours = minutes / 60;
                minutes -= hours * 60;
            }
        }
        string timestr = time.ToString().Substring(0,(time.ToString().Length>5)?5: time.ToString().Length);
        if (hours >= 1)
        {
            if(hours<10)
                convertedTime += "0"+hours.ToString()+":";
            else
                convertedTime += hours.ToString()+":";
        }
        if(minutes >= 1||hours>=1)
        {
            if (minutes == 0)
                convertedTime += "00:";
            else if (minutes < 10)
                convertedTime += "0"+minutes.ToString()+":";
            else
                convertedTime =minutes.ToString()+":";
        }
            if (time < 10)
                convertedTime +=timestr;
            else
                convertedTime +=timestr;

        return convertedTime;
    }
    private void Update()
    {
        BounceCountText.text = "Bounces: " + pScript.totalbounces.ToString();
        JumpCountText.text = "Jumps: " + pScript.totaljumps.ToString();
        TimerText.text = "Time Spent: " + timeConversion(pScript.elapsedtime);
        // tmpInput.SetText("Bounces: " + pScript.totalbounces + " \t\t\tJumps: " + pScript.totaljumps + "\t\tTime: " + ((float)((int)(Time.timeSinceLevelLoad*1000))/1000));
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
        Player.transform.position = new Vector2(PlayerPrefs.GetFloat("playerXPos"), PlayerPrefs.GetFloat("playerYPos"));
        var playerstats = Player.GetComponent<PlayerScript>();
        var camsettings = cam.GetComponent<CameraScript>();
        playerstats.totalbounces = PlayerPrefs.GetInt("Bounces");
        playerstats.totaljumps = PlayerPrefs.GetInt("Jumps");
        //gotta do one for play time

        cam.transform.position = new Vector2(PlayerPrefs.GetFloat("cameraXPos"), PlayerPrefs.GetFloat("cameraYPos"));
        camsettings.maxXDist = PlayerPrefs.GetFloat("cameraMaxXPos");
        camsettings.maxYDist = PlayerPrefs.GetFloat("cameraMaxYPos");
        camsettings.minXDist = PlayerPrefs.GetFloat("cameraMinXPos");
        camsettings.minYDist = PlayerPrefs.GetFloat("cameraMinYPos");
    }
    public void ReturnToMenu(){
        SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
    public void TogglePauseMenu()
    {
        pScript.setRunTimer(!pScript.getRunTimer());
        Time.timeScale=(Time.timeScale > 0) ? 0 : Time.timeScale = 1;
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        LeftButton.gameObject.SetActive(!LeftButton.gameObject.activeSelf);
        RightButton.gameObject.SetActive(!RightButton.gameObject.activeSelf);
        ChargeButton.gameObject.SetActive(!ChargeButton.gameObject.activeSelf);
    }
    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }
    public void SaveGame()
    {
        var playerstats = Player.GetComponent<PlayerScript>();
        var camsettings = cam.GetComponent<CameraScript>();
        PlayerPrefs.SetFloat("playerXPos",playerstats.lastsafepos.x);
        PlayerPrefs.SetFloat("playerYPos", playerstats.lastsafepos.y);
        PlayerPrefs.SetInt("Bounces", playerstats.totalbounces);
        PlayerPrefs.SetInt("Jumps", playerstats.totaljumps);

        PlayerPrefs.SetFloat("cameraXPos", cam.transform.position.x);
        PlayerPrefs.SetFloat("cameraYPos", cam.transform.position.y);
        PlayerPrefs.SetFloat("cameraMaxYPos", camsettings.maxYDist);
        PlayerPrefs.SetFloat("cameraMaxXPos", camsettings.maxXDist);
        PlayerPrefs.SetFloat("cameraMinYPos", camsettings.minYDist);
        PlayerPrefs.SetFloat("cameraMinXPos", camsettings.minXDist);

        PlayerPrefs.SetInt("playerSkin", 0);

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
    public void exitHeldButton(Button button)
    {
        button.GetComponent<Image>().overrideSprite = null;
    }
    public void setButtonDown(Button button)
    {
        switch (button.name)
        {
            case "LeftButton":
                LOn = !LOn;
                break;
            case "RightButton":
                ROn = !ROn;
                break;
            case "ChargeButton":
                COn = !COn;
                break;
        }
    }
    public void enterHeldButton(Button button)
    {
        switch (button.name)
        {
            case "LeftButton":
                if (LOn)
                {
                    pScript.setMoveDir(1);
                    button.GetComponent<Image>().overrideSprite = PressedButtonSprite;
                }
                break;
            case "RightButton":
                if (ROn)
                {
                    pScript.setMoveDir(2);
                    button.GetComponent<Image>().overrideSprite = PressedButtonSprite;
                }
                    break;
            case "ChargeButton":
                if (COn)
                {
                    pScript.startChargeJump();
                    button.GetComponent<Image>().overrideSprite = PressedButtonSprite;
                }
                break;

            default:
                break;
        }
    }
    public void colorPressedButton(Button button)
    {
        button.GetComponent<Image>().color = Color.green;
    }
    public void ToggleAudio()
    {
        isAudioOn = !isAudioOn;
        float vol;
        masterMixer.GetFloat("MasterVolume", out vol);
        masterMixer.SetFloat("MasterVolume", (vol== -80.0f) ? 0.0f : -80.0f);
        if (isAudioOn) 
            AudioButton.image.sprite = AudioOff;
        else
            AudioButton.image.sprite = AudioOn;
    }
}
