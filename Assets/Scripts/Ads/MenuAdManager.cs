using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;
using TMPro;


public class MenuAdManager : AdManager
{
    [SerializeField] private MenuMainManager main;

    [Header("Ad Game Objects")]
    [SerializeField] private BannerAdGameObject bannerAd;
    [SerializeField] private RewardedAdGameObject rewardedAd;
    [Space, Space]
    [SerializeField] private GameObject consentPopup;
    [Space, Space]
    [SerializeField] private TextMeshProUGUI rewardText;

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

        ActuRewardText();
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


    // ### Ad Reward ###

    private int IntReward()
    {
        if (!dataManager.progressionData.legendDiff) return 40000;
        if (!dataManager.progressionData.veteranDiff) return 30000;
        if (!dataManager.progressionData.starDiff) return 20000;
        if (!dataManager.progressionData.proDiff) return 10000;
        else return 5000;
    }

    public void AdReward()
    {
        Debug.Log("You've been rewarded !");
        dataManager.inventoryData.coins += IntReward();
        main.shopManager.ActuCoinsTexts();
    }

    private void ActuRewardText()
    {
        rewardText.text = "= " + IntReward();
    }
}
