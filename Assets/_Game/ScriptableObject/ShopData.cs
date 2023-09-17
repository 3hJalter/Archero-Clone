using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData", order = 1)]
public class ShopData : ScriptableObject
{
   [SerializeField] private List<ItemData> weaponDataList;
   [SerializeField] private List<ItemData> hatDataList;
   [SerializeField] private List<ItemData> pantDataList;
   
   public List<ItemData> GetItemDataList(ShopType type)
   {
       return type switch
       {
           ShopType.Weapon => weaponDataList,
           ShopType.Hat => hatDataList,
           ShopType.Pant => pantDataList,
           _ => null
       };
   }
   
   public static Item GetItem(IEnumerable<ItemData> itemList, PoolType type)
   {
       return (from t in itemList where t.item.poolType == type select t.item).FirstOrDefault();
   }
   
}

[Serializable]
public class ItemData
{
    public Item item;
    public int cost;
    public Sprite image;
    public bool isBought;
}
