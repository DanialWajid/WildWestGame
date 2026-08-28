using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager1 : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidGameId = "6116626"; // Replace with your Game ID from Unity Dashboard
    [SerializeField] string _adUnitId = "Interstitial_Android";
    [SerializeField] bool _testMode = true;

    public static AdsManager1 Instance;

    void Awake()
    {
        Instance = this;
        InitializeAds();
    }

    public void InitializeAds()
    {
        Advertisement.Initialize(_androidGameId, _testMode, this);
    }

    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
    }

    // Required Implementations for Listeners
    public void OnInitializationComplete() { Debug.Log("Ads Initialized"); LoadAd(); }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { }
    public void OnUnityAdsAdLoaded(string adUnitId) { }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) { }
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        LoadAd(); // Pre-load the next ad
    }
}