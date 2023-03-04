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
    private bool testMode = false;
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
        _adUnitId = myGameIdIOS;
        Advertisement.Initialize(_adUnitId, testMode, this);
#else
        _adUnitId = myGameIdAndroid;
        Advertisement.Initialize(_adUnitId, testMode, this);
#endif
        adButton.interactable = false;
    }
    public void playAd()
    {

        //load & play ad
        Debug.Log("show ad");
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
        if (placementId == _adUnitId)
        {
            Advertisement.Show(_adUnitId);
        }
    }

    //public void OnUnityAdsDidError(string message)
    //{
    //    //ad failed remove button do not grant reward
    //    Debug.Log(message);
    //    PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();
    //    TextMeshProUGUI lossScText = gameObject.transform.parent.GetComponentsInChildren<TextMeshProUGUI>()[2];
    //    lossScText.text = "x" + player.getExtraLifeCount().ToString();
    //    adButton.gameObject.SetActive(false);
    //    Advertisement.RemoveListener(this);
    //    Debug.Log("adRListener");
    //}

    //public void OnUnityAdsDidStart(string placementId)
    //{
    //    Debug.Log("adStart");
    //}

    //public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    //{
    //    PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();
    //    adCompleted = showResult == ShowResult.Finished;
    //    Debug.Log("Ad Completed: " + _adUnitId);
    //    if (adCompleted)
    //    {
    //        player.gainExtraLife();
    //        adButton.gameObject.transform.parent.GetChild(4).GetComponent<Button>().interactable = true;
    //        TextMeshProUGUI lossScText = gameObject.transform.parent.GetComponentsInChildren<TextMeshProUGUI>()[2];
    //        lossScText.text = "x" + player.getExtraLifeCount().ToString();
    //        adButton.gameObject.SetActive(false);
    //    }
    //    Advertisement.RemoveListener(this);
    //    Debug.Log("adRListener");
    //}

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Loaded: " + _adUnitId);

        if (_adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            adButton.onClick.AddListener(playAd);
            // Enable the button for users to click:
            adButton.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {_adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();
        adCompleted = showCompletionState == UnityAdsShowCompletionState.COMPLETED;
        Debug.Log("Ad Completed: " + _adUnitId);
        if (adCompleted)
        {
            player.gainExtraLife();
            adButton.gameObject.transform.parent.GetChild(4).GetComponent<Button>().interactable = true;
            TextMeshProUGUI lossScText = gameObject.transform.parent.GetComponentsInChildren<TextMeshProUGUI>()[2];
            lossScText.text = "x" + player.getExtraLifeCount().ToString();
            adButton.gameObject.SetActive(false);
        }
    }
    public void OnDestroy()
    {
        adButton.onClick.RemoveAllListeners();
    }

    public void OnInitializationComplete()
    {
        Debug.Log("adInitialize");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Error Initializing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
    }
}