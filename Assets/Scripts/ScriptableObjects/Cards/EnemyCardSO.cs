using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCard", menuName = "ScriptableObjects/Card/EnemyCard", order = 1)]
public class EnemyCardSO : CardSO
{
    [Header("Enemy card specifics")]
    public DefenderAttributesSO attribute;

    public string position;

    [Header("Locker room attributes")]
    public Mesh mesh;
    public Avatar avatar;

    public override void SetActive()
    {
        DataManager.InstanceDataManager.gameData.enemy = attribute;
    }
}
