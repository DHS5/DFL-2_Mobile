using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum SceneNumber { MENU = 0, GAME = 1}

public enum ScreenNumber { SETTINGS, GAMEPLAY, INFO, ALL }


public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// Settings Manager of the game
    /// </summary>
    public static SettingsManager Instance { get; private set; }

    [SerializeField] private GameObject EventSystem;

    // ### Managers ###
    // Multi scene managers
    public DataManager DataManager { get; private set; }

    // Menu scene managers
    private MenuMainManager menuMain;

    // Game scene managers
    private MainManager main;


    [Header("Settings screens")]
    [SerializeField] private GameObject[] screens;


    [Header("UI elements")]
    [SerializeField] private Slider joystickModeSlider;
    [SerializeField] private Slider centerSizeSlider;
    [SerializeField] private Slider boundarySizeSlider;
    [SerializeField] private Slider joystickSizeSlider;
    [SerializeField] private Slider joystickPosXSlider;
    [SerializeField] private Slider joystickPosYSlider;
    [SerializeField] private Slider jumpButtonSlider;

    [SerializeField] private Toggle goalpostToggle;
    [SerializeField] private Toggle backviewToggle;
    [Space]
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Toggle soundOnToggle;
    [SerializeField] private Slider playerVolumeSlider;
    [SerializeField] private Slider ambianceVolumeSlider;
    [SerializeField] private Slider enemiesVolumeSlider;
    [Space]
    [SerializeField] private Toggle infoButtonsToggle;

    [Header("Audio mixer")]
    [SerializeField] private AudioMixer audioMixer;

    // ### Properties ###

    private MainManager Main
    {
        get 
        { 
            if (main == null) main = FindObjectOfType<MainManager>();
            return main;
        }
    }
    private MenuMainManager MainMenu
    {
        get 
        { 
            if (menuMain == null) menuMain = FindObjectOfType<MenuMainManager>();
            return menuMain;
        }
    }




    public float HeadAngle
    {
        set 
        {
            DataManager.gameplayData.headAngle = value;
            if (Main != null) Main.PlayerManager.HeadAngle = value;
        }
    }
    public float YMouseSensitivity
    {
        set 
        {
            DataManager.gameplayData.yms = value;
            if (Main != null) Main.PlayerManager.YMouseSensitivity = value;
        }
    }
    public float YSmoothRotation
    {
        set 
        {
            DataManager.gameplayData.ysr = value;
            if (Main != null) Main.PlayerManager.YSmoothRotation = value;
        }
    }

    public float ViewType
    {
        set 
        {
            DataManager.gameplayData.viewType = (ViewType) value;
        }
    }

    public bool Goalpost
    {
        set { DataManager.gameplayData.goalpost = value; }
    }
    public bool Backview
    {
        set 
        { 
            DataManager.gameplayData.backview = value;
            if (Main != null) Main.GameUIManager.SetBackview(value);
        }
    }
    // Joystick gameplay
    public float JoystickMode
    {
        set
        {
            DataManager.gameplayData.joystickMode = (JoystickMode) value;
        }
    }
    public float JoystickCenterSize
    {
        set
        {
            DataManager.gameplayData.centerSize = value;
            if (Main != null) Main.TouchManager.CenterSize = value;
        }
    }
    public float JoystickBoundarySize
    {
        set
        {
            DataManager.gameplayData.boundarySize = value;
            if (Main != null) Main.TouchManager.Range = value;
        }
    }
    public float JoystickSize
    {
        set
        {
            DataManager.gameplayData.joystickSize = value;
            if (Main != null) Main.TouchManager.JoystickSize = value;
        }
    }
    public float JoystickPosX
    {
        set
        {
            DataManager.gameplayData.joystickPosX = value;
        }
    }
    public float JoystickPosY
    {
        set
        {
            DataManager.gameplayData.joystickPosY = value;
        }
    }
    
    public float JumpButtonSize
    {
        set
        {
            DataManager.gameplayData.jumpButtonSize = value;
        }
    }

    public bool InfoButtonsOn
    {
        get { return DataManager.playerPrefs.infoButtonsOn; }
        set 
        {
            DataManager.playerPrefs.infoButtonsOn = value;
            foreach (InfoButton ib in FindObjectsOfType<InfoButton>())
                ib.SetActive(value);
        }
    }

    public bool SoundOn 
    { 
        set 
        { 
            DataManager.audioData.soundOn = value;

            if (Main != null) Main.GameAudioManager.SoundOn = value;
            else audioMixer.SetFloat("Volume", value ? SoundVolume : -80);
        } 
    }
    public float SoundVolume 
    { 
        get { return DataManager.audioData.soundVolume; }
        set 
        { 
            DataManager.audioData.soundVolume = value;

            if (Main != null) Main.GameAudioManager.SoundVolume = value;
            else audioMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        } 
    }

    public float PlayerVolume
    {
        set
        {
            DataManager.audioData.playerVolume = value;
            audioMixer.SetFloat("PlayerVolume", Mathf.Log10(value) * 20);
        }
    }
    public float AmbianceVolume
    {
        set
        {
            DataManager.audioData.ambianceVolume = value;
            audioMixer.SetFloat("AmbianceVolume", Mathf.Log10(value) * 20);
        }
    }
    public float EnemiesVolume
    {
        set
        {
            DataManager.audioData.enemiesVolume = value;
            audioMixer.SetFloat("EnemiesVolume", Mathf.Log10(value) * 20);
        }
    }


    /// <summary>
    /// Gets the Singleton Instance
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        DataManager = FindObjectOfType<DataManager>();
    }

    /// <summary>
    /// Gets the Data Manager Instance
    /// </summary>
    private void Start()
    {
        SetEventSystem(true);
    }



    // ### Functions ###

    /// <summary>
    /// Load datas once DataManager has loaded all the datas
    /// </summary>
    public void Load()
    {
        LoadGameplayData(DataManager.gameplayData);
        LoadAudioData(DataManager.audioData);
        LoadPlayerPrefs(DataManager.playerPrefs);
    }


    private void LoadGameplayData(GameplayData data)
    {
        joystickModeSlider.value = (int)data.joystickMode;
        centerSizeSlider.value = data.centerSize;
        boundarySizeSlider.value = data.boundarySize;
        joystickSizeSlider.value = data.joystickSize;
        joystickPosXSlider.value = data.joystickPosX;
        joystickPosYSlider.value = data.joystickPosY;
        jumpButtonSlider.value = data.jumpButtonSize;

        goalpostToggle.isOn = data.goalpost;
        backviewToggle.isOn = data.backview;
    }


    private void LoadAudioData(AudioData data)
    {
        soundOnToggle.isOn = data.soundOn;
        soundVolumeSlider.value = data.soundVolume;

        playerVolumeSlider.value = data.playerVolume;
        ambianceVolumeSlider.value = data.ambianceVolume;
        enemiesVolumeSlider.value = data.enemiesVolume;

        SoundOn = data.soundOn;
        SoundVolume = data.soundVolume;
    }

    private void LoadPlayerPrefs(PlayerPrefs prefs)
    {
        infoButtonsToggle.isOn = prefs.infoButtonsOn;
    }



    // ## Menu Scene

    // ## Game Scene
    public void PauseGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int) SceneNumber.GAME && !Main.GameManager.GameOver)
        {
            Main.GameManager.PauseGame();
        }
    }
    public void UnpauseGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneNumber.GAME && !Main.GameManager.GameOver)
        {
            Main.GameManager.UnpauseGame();
        }
    }


    // ### Tools

    public void SetEventSystem(bool state) { EventSystem.SetActive(state); }

    public void SetScreen(ScreenNumber screen, bool state) 
    {
        if (screen != ScreenNumber.ALL)
            screens[(int)screen].SetActive(state);
        else
        {
            screens[(int)ScreenNumber.SETTINGS].SetActive(state);
            screens[(int)ScreenNumber.GAMEPLAY].SetActive(state);
            screens[(int)ScreenNumber.INFO].SetActive(state);
        }
    }
}
