using UnityEngine;
using UnityEngine.UI;

public class ShopItem : GameUnit
{
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject focusObj;
    [SerializeField] private GameObject boughtObj;

    [SerializeField] private Item item;

    public Item Item => item;

    [SerializeField] public ItemData itemData;
    private Shop _shop;

    public int Cost { get; private set; }
    

    public void OnSelect()
    {
        if (focusObj.activeSelf) return;
        if (_shop.currentItem != null) _shop.currentItem.Release();
        Use();
    }

    private void Use()
    {
        focusObj.SetActive(true);
        _shop.currentItem = this;
        _shop.ChangeShopButton();
        InitItem(_shop.currentShopBar.Type, item.poolType);
    }

    private static void InitItem(ShopType shopType, PoolType itemType)
    {
        switch (shopType)
        {
            case ShopType.Weapon:
                LevelManager.Ins.player.OnEquipWeapon((WeaponType) itemType);
                break;
            case ShopType.Hat:
                LevelManager.Ins.player.OnEquipHat((HatType) itemType);
                break;
            case ShopType.Pant:
                LevelManager.Ins.player.OnEquipPant((PantType) itemType);
                break;
        }
    }

    public void Release()
    {
        focusObj.SetActive(false);
    }

    public void SetData(ItemData itemDataI, Shop shop)
    {
        itemData = itemDataI;
        _shop = shop;
        itemImage.sprite = itemDataI.image;
        item = itemDataI.item;
        Cost = itemDataI.cost;
        boughtObj.SetActive(itemDataI.isBought);
    }

    public void ShowBoughtIcon()
    {
        boughtObj.SetActive(itemData.isBought);
    }
}
