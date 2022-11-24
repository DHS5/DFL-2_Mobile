using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerUD", menuName = "ScriptableObjects/Player/PlayerUniversalData", order = 1)]
public class PlayerUniversalDataSO : ScriptableObject
{
    [Header("Animation's time")]
    public float siderunTime;
    public float siderunDelay;
    public float jukeTime;
    public float jukeDelay;
    public float feintTime;
    public float spinTime;
    public float slideTime;
    public float sprintFeintTime;
    public float slipTime;
    public float slipSpeed;
    public float flipDelay;
    [Space]
    public float celebrationTime;
    public int celebrationNumber;
}
