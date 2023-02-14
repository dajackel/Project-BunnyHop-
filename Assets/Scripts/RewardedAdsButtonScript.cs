using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class RewardedAdsButtonScript : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string _androidAdUnitId = "Rewarded_Android";
    public string _iOSAdUnitId = "Rewarded_iOS";
    public string _adUnitId = null; // This will remain null for unsupported platforms
    private const string myGameIdAndroid = "5068035", myGameIdIOS = "5068034";
    public string _AdStatus = "";
    public bool adStarted;
    public bool adCompleted;
    private bool testMode = true;

    // Start is called before the first frame update
    public void playAd()
    {
#if UNITY_IOS
	        Advertisement.Initialize(myGameIdIOS, testMode, this);
	        myAdUnitId = _iOSAdUnitId;
#else
        Advertisement.Initialize(myGameIdAndroid, testMode, this,this);
        _adUnitId = _androidAdUnitId;
#endif
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        Advertisement.Load(_adUnitId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        _AdStatus = message;
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        if (!adStarted)
        {
            Advertisement.Show(_adUnitId, this);
        }
    }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        _AdStatus = message;
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");

    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        _AdStatus = message;
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        adStarted = true;
        Debug.Log("Ad Started: " + adUnitId);
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("Ad Clicked: " + adUnitId);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        adCompleted = showCompletionState == UnityAdsShowCompletionState.COMPLETED;
        Debug.Log("Ad Completed: " + adUnitId);
    }

}