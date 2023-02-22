using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class RewardedAdsButtonScript : MonoBehaviour, IUnityAdsListener
{

    public string _androidAdUnitId = "Rewarded_Android";
    public string _iOSAdUnitId = "Rewarded_iOS";
    public string _adUnitId = null; // This will remain null for unsupported platforms
    private const string myGameIdAndroid = "5068035", myGameIdIOS = "5068034";
    public string _AdStatus = "";
    public bool adStarted;
    public bool adCompleted;
    private bool testMode = true;
    Button adButton;
    // Start is called before the first frame update
    private void Start()
    {
        adButton = gameObject.GetComponent<Button>();
        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
#if UNITY_IOS
	        Advertisement.Initialize(myGameIdIOS, testMode, true, this);
	        myAdUnitId = _iOSAdUnitId;
#else
        Advertisement.Initialize(myGameIdAndroid, testMode, true);
        _adUnitId = _androidAdUnitId;
#endif
    }
    public void playAd()
    {

        //load & play ad
        Advertisement.Load(_adUnitId);
        adButton.interactable = false;
    }

    //public void OnInitializationComplete()
    //{
    //    Debug.Log("Unity Ads initialization complete.");
    //    adButton.interactable = true;
    //}
    //
    //public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    //{
    //    _AdStatus = message;
    //    Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    //}
    //
    //public void OnUnityAdsAdLoaded(string adUnitId)
    //{
    //    Debug.Log("Ad Loaded: " + adUnitId);
    //    if (!adStarted)
    //    {
    //        Advertisement.Show(_adUnitId);
    //    }
    //}
    //public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    //{
    //    _AdStatus = message;
    //    Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    //
    //}
    //
    //public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    //{
    //    _AdStatus = message;
    //    Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    //}
    //
    //public void OnUnityAdsShowStart(string adUnitId)
    //{
    //    adStarted = true;
    //    Debug.Log("Ad Started: " + adUnitId);
    //}
    //
    //public void OnUnityAdsShowClick(string adUnitId)
    //{
    //    Debug.Log("Ad Clicked: " + adUnitId);
    //}
    //
    //
    //public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    //{
    //    PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();
    //    adCompleted = showCompletionState == UnityAdsShowCompletionState.COMPLETED;
    //    Debug.Log("Ad Completed: " + adUnitId);
    //    player.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    //    if (adCompleted)
    //    {
    //        player.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    //        player.extraLives++;
    //        adButton.gameObject.SetActive(false);
    //        TextMeshProUGUI[] lossScText = gameObject.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
    //        lossScText[2].text = "x" + player.extraLives.ToString();
    //    }
    //}

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("adReady");
        if (placementId == _adUnitId)
        {
            Advertisement.Show(_adUnitId);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("adErr");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("adStart");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();
        adCompleted = showResult == ShowResult.Finished;
        Debug.Log("Ad Completed: " + _adUnitId);
        if (adCompleted)
        {
            player.extraLives++;
            adButton.gameObject.transform.parent.GetChild(4).GetComponent<Button>().interactable = true;
            TextMeshProUGUI[] lossScText = gameObject.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            lossScText[2].text = "x" + player.extraLives.ToString();
            adButton.gameObject.SetActive(false);
        }
        Advertisement.RemoveListener(this);
    }
}