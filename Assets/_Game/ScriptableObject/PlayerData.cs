using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private WeaponType weaponType;

    public WeaponType WeaponType => weaponType;

    public HatType HatType => hatType;

    public PantType PantType => pantType;

    [SerializeField] private HatType hatType;
    [SerializeField] private PantType pantType;
    public int coin;
    
    public PoolType GetTypeDataFromShopType(ShopType shopType)
    {
        return shopType switch
        {
            ShopType.Weapon => (PoolType) weaponType,
            ShopType.Hat => (PoolType) hatType,
            ShopType.Pant => (PoolType) pantType,
            _ => PoolType.None
        };
    }
    
    public void SaveTypeDataFromShopType(ShopType shopType, PoolType poolType)
    {
        switch (shopType)
        {
            case ShopType.Weapon:
                weaponType = (WeaponType) poolType;
                break;
            case ShopType.Hat:
                hatType = (HatType) poolType;
                break;
            case ShopType.Pant:
                pantType = (PantType) poolType;
                break;
            case ShopType.None:
                break;
        }
    }
}
