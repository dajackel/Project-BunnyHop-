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
    private bool testMode = false;
    Button adButton;
    // Start is called before the first frame update
    private void Start()
    {
        adButton = gameObject.GetComponent<Button>();
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnUnityAdsDidError("No internet connection");
            return;
        }
        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Debug.Log("adListener");
#if UNITY_IOS
        Advertisement.Initialize(myGameIdAndroid, testMode, true);
        _adUnitId = _iOSAdUnitId;
        Debug.Log("adInitialize");
#else
        Advertisement.Initialize(myGameIdAndroid, testMode, true);
        _adUnitId = _androidAdUnitId;
        Debug.Log("adInitialize");
#endif
    }
    public void playAd()
    {

        //load & play ad
        Debug.Log("adLoad");
        Debug.Log(_adUnitId);
        Advertisement.Load(_adUnitId);
        adButton.interactable = false;
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

    public void OnUnityAdsDidError(string message)
    {
        //ad failed remove button do not grant reward
        Debug.Log(message);
        PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();
        TextMeshProUGUI lossScText = gameObject.transform.parent.GetComponentsInChildren<TextMeshProUGUI>()[2];
        lossScText.text = "x" + player.getExtraLifeCount().ToString();
        adButton.gameObject.SetActive(false);
        Advertisement.RemoveListener(this);
        Debug.Log("adRListener");
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
            player.gainExtraLife();
            adButton.gameObject.transform.parent.GetChild(4).GetComponent<Button>().interactable = true;
            TextMeshProUGUI lossScText = gameObject.transform.parent.GetComponentsInChildren<TextMeshProUGUI>()[2];
            lossScText.text = "x" + player.getExtraLifeCount().ToString();
            adButton.gameObject.SetActive(false);
        }
        Advertisement.RemoveListener(this);
        Debug.Log("adRListener");
    }
}