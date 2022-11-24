using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Enemy/Defender/WingmanAttribute", order = 1)]
public class WingmanAttributesSO : DefenderAttributesSO
{
    public override int Type { get { return (int) DefenderType.WINGMAN; } }

    [Header("Wingman attributes")]
    [Tooltip("Precision in the interception process")]
    [Range(0, 1)] public float precision;
    [Tooltip("Anticipation of the player movement during attack")]
    public float attackAnticipation;
    [Space]
    [Tooltip("If in the angle --> Chase")]
    public float chaseAngle;
    [Space]
    [Tooltip("If RawDist < chaseDist --> Chase / else --> Intercept")]
    public float chaseDist;
}
