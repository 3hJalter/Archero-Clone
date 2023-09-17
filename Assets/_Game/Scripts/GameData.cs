using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    [SerializeField] private PlayerData playerData;

    [SerializeField] private ShopData shopData;

    [SerializeField] private HatData hatData;

    [SerializeField] private PantData pantData;
    public PlayerData PlayerData => playerData;
    
    // Remove
    public HatData HatData => hatData;
    public PantData PantData => pantData;
    //
    public Weapon GetWeapon(WeaponType weaponType)
    {
        return (Weapon)ShopData.GetItem(shopData.GetItemDataList(ShopType.Weapon), (PoolType)weaponType);
    }

    public Hat GetHat(HatType hatType)
    {
        return (Hat) ShopData.GetItem(shopData.GetItemDataList(ShopType.Hat), (PoolType) hatType);
    }

    public Material GetPant(PantType pantType)
    {   Pant item = (Pant) ShopData.GetItem(shopData.GetItemDataList(ShopType.Pant), (PoolType)pantType);
        return item.Material;
    }
    
}