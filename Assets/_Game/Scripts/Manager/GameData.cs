using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    [SerializeField] private PlayerData playerData;

    [SerializeField] private ShopData shopData;

    [SerializeField] private StageData stageData;
    
    public List<Stage> StageDataList => stageData.stageList;

    public string GetReachedStageIndexString()
    {
        int index = 0;
        for (int i = 0; i < StageDataList.Count; i++)
        {
            if (StageDataList[i].IsPassed()) index = i;
        }
        index += 1;
        return index.ToString();
    }
    
    public PlayerData PlayerData => playerData;
    
    public Weapon GetWeapon(WeaponType weaponType)
    {
        return (Weapon)ShopData.GetItem(shopData.GetItemDataList(ShopType.Weapon), (PoolType)weaponType);
    }

    public Hat GetHat(HatType hatType)
    {
        return (Hat)ShopData.GetItem(shopData.GetItemDataList(ShopType.Hat), (PoolType)hatType);
    }

    public Material GetPant(PantType pantType)
    {
        Pant item = (Pant)ShopData.GetItem(shopData.GetItemDataList(ShopType.Pant), (PoolType)pantType);
        return item.Material;
    }

    public static void SaveLevelAndStageData(int stageIndex, int levelIndex)
    {
        PlayerPrefs.SetInt(Constants.STAGE, stageIndex);
        PlayerPrefs.SetInt(Constants.LEVEL, levelIndex);
    }

    public static void SavePlayerLastPlayedData(int lastPlayerHealth, int totalCoinGet)
    {
        PlayerPrefs.SetInt(Constants.LAST_PLAYER_HEALTH, lastPlayerHealth);
        PlayerPrefs.SetInt(Constants.TOTAL_COIN_GET_IN_LEVEL, totalCoinGet);
    }

    // ReSharper disable once RedundantAssignment
    public static void GetLastPlayedData(Player player, out int lastPlayerHealth, 
        out int totalCoinGet, out int levelIndex, out int stageIndex)
    {
        lastPlayerHealth = PlayerPrefs.GetInt(Constants.LAST_PLAYER_HEALTH,
            player.GetMaxHealth());
        totalCoinGet = PlayerPrefs.GetInt(Constants.TOTAL_COIN_GET_IN_LEVEL, 0);
        levelIndex = PlayerPrefs.GetInt(Constants.LEVEL, 0);
        stageIndex = PlayerPrefs.GetInt(Constants.STAGE, 0);
    }
}
