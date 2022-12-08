using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;


public class MenuAdManager : AdManager
{
    [SerializeField] private MenuMainManager main;

    [Header("Ad Game Objects")]
    [SerializeField] private BannerAdGameObject bannerAd;
    [SerializeField] private RewardedAdGameObject rewardedAd;
    [Space, Space]
    [SerializeField] private GameObject consentPopup;

    private bool gotConsentResponse = false;

    public bool RememberChoice
    {
        get { return dataManager.adPrefs.rememberChoice; }
        set { dataManager.adPrefs.rememberChoice = value; }
    }

    private void Start()
    {
        dataManager = main.DataManager;

        StartCoroutine(GetConsentCR());
    }

    // ### Start Coroutine ###

    private IEnumerator GetConsentCR()
    {
        if (dataManager.FirstScene)
        {
            if (!RememberChoice)
            {
                consentPopup.SetActive(true);

                yield return new WaitUntil(() => gotConsentResponse);
            }

            MobileAds.Initialize((initStatus) =>
            {
                Debug.Log("Ads initialized");

                BannerLoadAd();
                RewardedLoadAd();
            });
        }
        else
        {
            BannerLoadAd();
            RewardedLoadAd();
        }
    }
    public void GotConsentResponse(bool response) { gotConsentResponse = true; Consent = response; Destroy(consentPopup); }


    // ### Ad Loading ###

    public void BannerLoadAd()
    {
        if (Consent)
        {
            Debug.Log("Banner loaded with consent");
            bannerAd.LoadAd();
        }
        else
        {
            Debug.Log("Banner loaded without consent");
            NoConsentAdRequest(out AdRequest adRequest);
            bannerAd.LoadAd(adRequest);
        }
    }
    public void RewardedLoadAd()
    {
        if (Consent)
        {
            rewardedAd.LoadAd();
        }
        else
        {
            NoConsentAdRequest(out AdRequest adRequest);
            rewardedAd.LoadAd(adRequest);
        }
    }

    // ### Ad Showing ###

    public void ShowBanner()
    {
        bannerAd.Show();
    }
    public void ShowRewarded()
    {
        rewardedAd.ShowIfLoaded();
    }
}
