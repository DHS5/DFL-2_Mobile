using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [Tooltip("Player script")]
    private Player player;


    [Header("Audio")]
    [Tooltip("Audio source of the player")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] hurtClips;
    [SerializeField] private AudioClip[] loseClips;
    [SerializeField] private AudioClip[] dieClips;
    [SerializeField] private AudioClip[] effortClips;
    [SerializeField] private AudioClip[] softEffortClips;
    [Space]
    [SerializeField] private AudioSource playerBreathAudioSource;

    [Header("VFX game objects")]
    [Tooltip("Rain particle system")]
    [SerializeField] private ParticleSystem rainParticleSystem;
    [Tooltip("Rain splash particle system")]
    [SerializeField] private ParticleSystem rainSplashParticleSystem;



    private Coroutine sprintCR;
    private Coroutine restCR;


    private void Awake()
    {
        player = GetComponent<Player>();
    }



    // ### Functions ###

    // # Audio #

    public void AudioPlayerEffort(bool soft)
    {
        if (!soft) audioSource.PlayOneShot(effortClips[Random.Range(0, effortClips.Length)]);
        else audioSource.PlayOneShot(softEffortClips[Random.Range(0, softEffortClips.Length)]);
    }
    public void AudioPlayerHurt()
    {
        audioSource.PlayOneShot(hurtClips[Random.Range(0, hurtClips.Length)]);
    }
    public void AudioPlayerLose()
    {
        audioSource.PlayOneShot(loseClips[Random.Range(0, loseClips.Length)]);
        Invoke(nameof(DisableBreathAudio), 1f);
    }
    public void AudioPlayerDie()
    {
        audioSource.clip = dieClips[Random.Range(0, dieClips.Length)];
        audioSource.PlayDelayed(1f);
        Invoke(nameof(DisableBreathAudio), 1f);
    }
    public void DisableBreathAudio()
    {
        playerBreathAudioSource.enabled = false;
    }


    // # VFX #

    public void Rain(bool state, float particleAddition)
    {
        var emission = rainParticleSystem.emission;
        float emi = emission.rateOverTime.constant;
        emission.rateOverTime = emi + particleAddition;

        rainParticleSystem.gameObject.SetActive(state);
        rainSplashParticleSystem.gameObject.SetActive(state);
    }
}
