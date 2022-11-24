using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCapacityShopCard : MonoBehaviour
{
    [Header("Content")]
    public PlayerShopCapacityCardInfo info;

    [Header("UI components")]
    [Header("Physical Zone")]
    [SerializeField] private CapacityCardGauge speedGauge;
    [SerializeField] private CapacityCardGauge sideSpeedGauge;
    [SerializeField] private CapacityCardGauge sprintGauge;
    [SerializeField] private CapacityCardGauge sprintStaminaGauge;
    [SerializeField] private CapacityCardGauge slowGauge;
    [SerializeField] private CapacityCardGauge shiftSpeedGauge;
    [SerializeField] private CapacityCardGauge jumpHeightGauge;
    [SerializeField] private CapacityCardGauge jumpStaminaGauge;

    [Header("Skills Zone")]
    [SerializeField] private Toggle jukeToggle;
    [SerializeField] private Toggle feintToggle;
    [SerializeField] private Toggle spinToggle;
    [SerializeField] private Toggle jukeSpinToggle;
    [SerializeField] private Toggle slideToggle;
    [SerializeField] private Toggle flipToggle;
    [SerializeField] private Toggle truckToggle;
    [SerializeField] private Toggle highKneeToggle;
    [SerializeField] private Toggle sprintFeintToggle;
    [SerializeField] private Toggle hurdleToggle;

    [Header("Handling Zone")]
    [SerializeField] private CapacityCardGauge dirSensitivityGauge;
    [SerializeField] private CapacityCardGauge dirGravityGauge;
    [SerializeField] private CapacityCardGauge accSensitivityGauge;
    [SerializeField] private CapacityCardGauge accGravityGauge;


    public void ApplyInfos(PlayerShopCapacityCardInfo infos)
    {
        speedGauge.ApplyGaugeInfo(infos.speedInfo);
        sideSpeedGauge.ApplyGaugeInfo(infos.sideSpeedInfo);
        sprintGauge.ApplyGaugeInfo(infos.sprintInfo);
        sprintStaminaGauge.ApplyGaugeInfo(infos.staminaInfo);
        slowGauge.ApplyGaugeInfo(infos.slowInfo);
        shiftSpeedGauge.ApplyGaugeInfo(infos.shiftInfo);
        jumpHeightGauge.ApplyGaugeInfo(infos.jumpHeightInfo);
        jumpStaminaGauge.ApplyGaugeInfo(infos.jumpStaminaInfo);

        jukeToggle.isOn = infos.canJuke;
        feintToggle.isOn = infos.canFeint;
        spinToggle.isOn = infos.canSpin;
        jukeSpinToggle.isOn = infos.canJukeSpin;
        slideToggle.isOn = infos.canSlide;
        flipToggle.isOn = infos.canFlip;
        truckToggle.isOn = infos.canTruck;
        highKneeToggle.isOn = infos.canHighKnee;
        sprintFeintToggle.isOn = infos.canSprintFeint;
        hurdleToggle.isOn = infos.canHurdle;

        dirSensitivityGauge.ApplyGaugeInfo(infos.dirSensitivityInfo);
        dirGravityGauge.ApplyGaugeInfo(infos.dirGravityInfo);
        accSensitivityGauge.ApplyGaugeInfo(infos.accSensitivityInfo);
        accGravityGauge.ApplyGaugeInfo(infos.accGravityInfo);
    }

    public void ApplyInfos()
    {
        ApplyInfos(info);
    }

    public void ApplyInfos(PlayerAttributesSO p)
    {
        info.speedInfo.value = p.NormalSpeed;
        info.sideSpeedInfo.value = p.NormalSideSpeed;
        info.sprintInfo.value = p.AccelerationM;
        info.staminaInfo.value = p.accelerationTime / p.accelerationRestTime;
        info.slowInfo.value = p.SlowM;
        info.shiftInfo.value = p.SlowSideSpeed;
        info.jumpHeightInfo.value = p.JumpHeight;
        info.jumpStaminaInfo.value = p.JumpStamina / (p.JumpRechargeTime * p.JumpCost);

        info.dirSensitivityInfo.value = p.DirSensitivity;
        info.dirGravityInfo.value = p.DirGravity;
        info.accSensitivityInfo.value = p.AccSensitivity;
        info.accGravityInfo.value = p.AccGravity;

        info.canJuke = p.CanJuke;
        info.canFeint = p.CanFeint;
        info.canSpin = p.CanSpin;
        info.canJukeSpin = p.CanJukeSpin;
        info.canSlide = p.CanSlide;
        info.canFlip = p.CanFlip;
        info.canTruck = p.CanTruck;
        info.canHighKnee = p.CanHighKnee;
        info.canSprintFeint = p.CanSprintFeint;
        info.canHurdle = p.CanHurdle;

        ApplyInfos();
    }
}

[System.Serializable]
public class PlayerShopCapacityCardInfo
{
    [Header("Physical")]
    public CapacityCardGaugeInfo speedInfo;
    public CapacityCardGaugeInfo sideSpeedInfo;
    public CapacityCardGaugeInfo sprintInfo;
    public CapacityCardGaugeInfo staminaInfo;
    public CapacityCardGaugeInfo slowInfo;
    public CapacityCardGaugeInfo shiftInfo;
    public CapacityCardGaugeInfo jumpHeightInfo;
    public CapacityCardGaugeInfo jumpStaminaInfo;

    [Header("Skills")]
    public bool canJuke;
    public bool canFeint;
    public bool canSpin;
    public bool canJukeSpin;
    public bool canSlide;
    public bool canFlip;
    public bool canTruck;
    public bool canHighKnee;
    public bool canSprintFeint;
    public bool canHurdle;

    [Header("Handling")]
    public CapacityCardGaugeInfo dirSensitivityInfo;
    public CapacityCardGaugeInfo dirGravityInfo;
    public CapacityCardGaugeInfo accSensitivityInfo;
    public CapacityCardGaugeInfo accGravityInfo;
}
