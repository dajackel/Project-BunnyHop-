using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour//, IUnityAdsInitializationListener
{
    const string androidGameId = "", iosGameId = "",
       androidBannerUnit = "Banner_Android", iosBannerUnit = "Banner_iOS", FullScreenAd = "Pre-respawn_FS_ad";
    [SerializeField] Button _loadBannerButton, _showBannerButton, _hideBannerButton;
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
    //bool _testMode = true;
    //private string adUnitId;
    public bool paused = false;
    private float timeScale = 1, currentLevelPos = 0;
    public GameObject[] levelSection;
    private GameObject lastLevelSpawned;
    [SerializeField] UIManager UI;
    private bool creatingLevel = false;

    //add opposite to display pretty numbers as highscore
    private float groundLevel = -3.69f;
    //Player reference to easily grab height
    [SerializeField] PlayerScript player;
    //player highscore
    public float highScore;

    private void Awake()
    {
        //adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
        //   ? iosBannerUnit
        //   : androidBannerUnit;
    }

    //    #region BANNERADS
    //    public void InitializeAds()
    //    {
    //        Advertisement.Initialize(_gameId, _testMode, this);
    //    }
    //    public void OnInitializationComplete()
    //    {
    //        Debug.Log("Unity Ads initialization complete.");
    //    }
    //    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    //    {
    //        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    //    }
    //    // Implement code to execute when the loadCallback event triggers:
    //    void OnBannerLoaded()
    //    {
    //        Debug.Log("Banner loaded");

    //        // Configure the Show Banner button to call the ShowBannerAd() method when clicked:
    //        _showBannerButton.onClick.AddListener(ShowBannerAd);
    //        // Configure the Hide Banner button to call the HideBannerAd() method when clicked:
    //        _hideBannerButton.onClick.AddListener(HideBannerAd);

    //        // Enable both buttons:
    //        _showBannerButton.interactable = true;
    //        _hideBannerButton.interactable = true;
    //    }

    //    // Implement code to execute when the load errorCallback event triggers:
    //    void OnBannerError(string message)
    //    {
    //        Debug.Log($"Banner Error: {message}");
    //        // Optionally execute additional code, such as attempting to load another ad.
    //    }

    //    // Implement a method to call when the Show Banner button is clicked:
    //    void ShowBannerAd()
    //    {
    //        // Set up options to notify the SDK of show events:
    //        BannerOptions options = new BannerOptions
    //        {
    //            clickCallback = OnBannerClicked,
    //            hideCallback = OnBannerHidden,
    //            showCallback = OnBannerShown
    //        };

    //        // Show the loaded Banner Ad Unit:
    //        Advertisement.Banner.Show(bannerAd, options);
    //    }

    //    // Implement a method to call when the Hide Banner button is clicked:
    //    void HideBannerAd()
    //    {
    //        // Hide the banner:
    //        Advertisement.Banner.Hide();
    //    }

    //    void OnBannerClicked() { }
    //    void OnBannerShown() { }
    //    void OnBannerHidden() { }

    //    void OnDestroy()
    //    {
    //        // Clean up the listeners:
    //        _loadBannerButton.onClick.RemoveAllListeners();
    //        _showBannerButton.onClick.RemoveAllListeners();
    //        _hideBannerButton.onClick.RemoveAllListeners();
    //    }
    //    public void LoadBanner()
    //    {
    //        // Set up options to notify the SDK of load events:
    //        BannerLoadOptions options = new BannerLoadOptions
    //        {
    //            loadCallback = OnBannerLoaded,
    //            errorCallback = OnBannerError
    //        };

    //        // Load the Ad Unit with banner content:
    //        Advertisement.Banner.Load(bannerAd, options);
    //    }
    ////#endregion

    void Start()
    {
        //_showBannerButton.interactable = false;
        //_hideBannerButton.interactable=false;

        //Advertisement.Banner.SetPosition(_bannerPosition);

        //_loadBannerButton.onClick.AddListener(LoadBanner);
        //_loadBannerButton.interactable = true;
        highScore = groundLevel;
        PlayerPrefs.GetFloat("highScore", highScore);
        UI.pHighScore = highScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (!creatingLevel)
            StartCoroutine(levelGenerator());
        //top of player's head position
        float pHeight = player.getPlayerHeight();
        if (pHeight > highScore)
        {
            highScore = pHeight;
            UI.pHighScore = highScore;
        }
        UI.pCurrHeight = pHeight;
    }


    IEnumerator levelGenerator()
    {
        creatingLevel = true;
        print("level gen called");
        currentLevelPos += 25;
        int lvlToSpawn = Random.Range(1, levelSection.Length);
        if (lastLevelSpawned == levelSection[lvlToSpawn])
            lvlToSpawn = (lvlToSpawn + 1) % levelSection.Length;
        Instantiate(levelSection[lvlToSpawn], new Vector3(-1.25f, currentLevelPos, 0), Quaternion.identity);
        yield return new WaitForSecondsRealtime(5);
        creatingLevel = false;
    }
}
