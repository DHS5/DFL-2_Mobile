using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;

public class GameAdManager : AdManager
{
    [SerializeField] private MainManager main;

    [Header("Parameters")]
    [SerializeField] private int gameBeforeAd;

    [Header("Ad Game Objects")]
    [SerializeField] private InterstitialAdGameObject interAd;


    private bool showAd = false;

    public bool WillShowAd
    {
        get { return showAd; }
    }


    private void Start()
    {
        dataManager = main.DataManager;

        dataManager.adPrefs.gamesPlayed++;
        if ((dataManager.adPrefs.gamesPlayed) % gameBeforeAd == 0)
        {
            LoadInterAd();
            showAd = true;
        }
    }


    private void LoadInterAd()
    {
        if (Consent)
        {
            Debug.Log("Inter loaded with consent");
            interAd.LoadAd();
        }
        else
        {
            Debug.Log("Inter loaded without consent");
            NoConsentAdRequest(out AdRequest adRequest);
            interAd.LoadAd(adRequest);
        }
    }

    public void ShowInterAd()
    {
        interAd.ShowIfLoaded();
    }
}
