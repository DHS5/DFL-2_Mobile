using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;


public abstract class AdManager : MonoBehaviour
{
    protected DataManager dataManager;

    protected bool Consent
    {
        get { return dataManager.adPrefs.consent; }
        set { dataManager.adPrefs.consent = value; }
    }

    // ### Results Logs ###

    public void Fail()
    {
        Debug.Log("Failed to load");
    }


    // ### Ad Request ###

    protected void NoConsentAdRequest(out AdRequest adRequest)
    {
        adRequest = new AdRequest.Builder().AddExtra("npa", "1").Build();
    }
}
