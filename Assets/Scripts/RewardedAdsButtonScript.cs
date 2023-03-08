using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class RewardedAdsButtonScript : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{

    public const string _androidAdUnitId = "Rewarded_Android";
    public const string _iOSAdUnitId = "Rewarded_iOS";
    public string _adUnitId = null; // This will remain null for unsupported platforms
    private const string myGameIdAndroid = "5068035", myGameIdIOS = "5068034";
    public string _AdStatus = "";
    public bool adStarted;
    public bool adCompleted;
    private bool isPlayingAd = false;
    private bool testMode = false;
    private bool rewardGranted = false;
    Button adButton;

    private void Awake()
    {
        adButton = gameObject.GetComponent<Button>();
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection");
            return;
        }
        // Initialize the Ads listener and service:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
        Advertisement.Initialize(myGameIdIOS, testMode, this);
#else
        _adUnitId = _androidAdUnitId;
        Advertisement.Initialize(myGameIdAndroid, testMode, this);
#endif
        adButton.interactable = false;
    }
    public void playAd()
    {
        Debug.Log("play ad called");
        //load & play ad
        if (isPlayingAd)
            return;
        isPlayingAd = true;
        adButton.interactable = false;
        Advertisement.Show(_adUnitId, this);
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("adReady");
        Debug.Log(placementId + " " + _adUnitId);
        //if (placementId == _adUnitId)
        //{
        //    Advertisement.Show(_adUnitId);
        //}
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Loaded: " + _adUnitId + " " + placementId);

        if (_adUnitId.Equals(_adUnitId))
        {
            Debug.Log("adunit was adunit");
            // Configure the button to call the ShowAd() method when clicked:
            adButton.onClick.AddListener(playAd);
            // Enable the button for users to click:
            adButton.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        UpdateUI();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        UpdateUI();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Show ad start");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Show ad click");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();
        adCompleted = showCompletionState == UnityAdsShowCompletionState.COMPLETED;
        Debug.Log("Ad Completed: " + _adUnitId);
        if (adCompleted && !rewardGranted)
        {
            Debug.Log("reward granted");
            rewardGranted = true;
            player.gainExtraLife();
            UpdateUI(player);
        }
        else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
        {
            isPlayingAd = false;
            Debug.Log("ad skipped");
            adButton.onClick.RemoveListener(playAd);
            LoadAd();
        }
    }
    public void OnDestroy()
    {
        adButton.onClick.RemoveAllListeners();
    }

    public void OnInitializationComplete()
    {
        Debug.Log("adInitialize");
        Advertisement.Load(_adUnitId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Error Initializing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        UpdateUI();
    }

    private void UpdateUI(PlayerScript player = null)
    {
        adButton.gameObject.transform.parent.GetChild(4).GetComponent<Button>().interactable = true;
        TextMeshProUGUI lossScText = gameObject.transform.parent.GetComponentsInChildren<TextMeshProUGUI>()[2];
        if (player != null)
            lossScText.text = "x" + player.getExtraLifeCount().ToString();
        adButton.gameObject.SetActive(false);
    }
}