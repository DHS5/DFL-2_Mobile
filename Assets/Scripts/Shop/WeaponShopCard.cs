using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopCard : ShopCard
{
    [Header("Weapon card's specifics")]
    public WeaponCapacityCard capacityCard;
    [Space]
    [SerializeField] private Image bulletImage;

    public WeaponCardSO weaponCardSO { get { return cardSO as WeaponCardSO; } }

    public override void GenerateCard(ShopCardSO _cardSO, ShopButton _shopButton, bool _buyable, bool _enoughMoney)
    {
        base.GenerateCard(_cardSO, _shopButton, _buyable, _enoughMoney);

        Weapon w = weaponCardSO.prefab.GetComponent<Weapon>();

        bulletImage.sprite = weaponCardSO.bulletSprite;

        capacityCard.info.rangeInfo.value = w.Range;
        capacityCard.info.angleInfo.value = w.Angle;
        capacityCard.info.ammunitionInfo.value = w.Ammunition;
        capacityCard.info.reloadInfo.value = w.ReloadTime;
        capacityCard.info.maxVictimInfo.value = w.MaxVictim;

        capacityCard.ApplyInfos();
    }
}
