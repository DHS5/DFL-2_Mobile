using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ParkourManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    private DataManager dataManager;

    public Parkour Parkour { get; private set; }

    [Header("Parkours")]
    [SerializeField] private Parkour[] parkours;

    [SerializeField] private PlayerInfo parkourPlayer;
    [SerializeField] private GameObject parkourStadium;


    [Header("UI components")]
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject congratulationsText;


    private void Awake()
    {
        main = GetComponent<MainManager>();
        dataManager = FindObjectOfType<DataManager>();

        if (dataManager.gameData.gameMode == GameMode.DRILL && dataManager.gameData.gameDrill == GameDrill.PARKOUR)
        {
            dataManager.gameData.gameDifficulty = GameDifficulty.ROOKIE;
            dataManager.gameData.gameOptions.Clear();
            dataManager.gameData.gameWeather = GameWeather.PARKOUR;
            dataManager.gameData.player = parkourPlayer;
            dataManager.gameData.stadium = parkourStadium;
        }
    }


    // ### Functions ###


    public void GenerateParkour()
    {
        Vector3 position = main.FieldManager.field.enterZone.transform.position;
        int parkourNumber = main.GameManager.gameData.parkour;

        Parkour = Instantiate(parkours[parkourNumber].gameObject, position, Quaternion.identity).GetComponent<Parkour>();

        nextButton.SetActive(parkourNumber != parkours.Length - 1);
        congratulationsText.SetActive(parkourNumber == parkours.Length - 1);
    }

    public void Win()
    {
        if (Won((int)Parkour.ParkourNum))
            main.DataManager.inventoryData.coins += Parkour.BaseReward;

        else
        {
            main.DataManager.inventoryData.coins += Parkour.Reward;
            UnlockNextParkour();
        }
    }

    public void Next()
    {
        main.DataManager.playerPrefs.parkourIndex++;
        main.DataManager.gameData.parkour++;
    }

    private void UnlockNextParkour()
    {
        List<int> parkours = main.DataManager.inventoryData.parkours.ToList();
        parkours.Add((int)Parkour.ParkourNum + 1);
        main.DataManager.inventoryData.parkours = parkours.ToArray();
    }

    public static bool Won(int parkourNum)
    {
        foreach (int i in DataManager.InstanceDataManager.inventoryData.parkours)
            if (i == parkourNum + 1) return true;

        return false;
    }
}
