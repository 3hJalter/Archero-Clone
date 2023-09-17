using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : UICanvas
{
    [SerializeField] private ShopItem prefab;
    [SerializeField] private Transform content;
    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private List<ShopBar> shopBarList;

    public ShopItem currentItem;
    public ShopBar currentShopBar;
    [SerializeField] private ShopData shopData;

    [SerializeField] private GameObject buyButton;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject onUsePanel;

    private readonly MiniPool<ShopItem> _shopItemPool = new();

    private void Awake()
    {
        OnInit();
    }

    private void OnEnable()
    {
        currentShopBar = shopBarList[(int)ShopType.Weapon];
        currentShopBar.OnSelect();
    }

    private void OnInit()
    {
        coinTxt.text = GameData.Ins.PlayerData.coin.ToString();
        _shopItemPool.OnInit(prefab, 10, content);
        for (int i = 0; i < shopBarList.Count; i++) shopBarList[i].SetShop(this);
        // SelectBar(shopBarList[(int) ShopType.Weapon]);
        currentShopBar = shopBarList[(int)ShopType.Weapon];
        currentShopBar.OnSelect();
    }
    
    public void OnClose()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        LevelManager.Ins.player.OnEquipItem();
        currentItem.Release();
        currentShopBar.Release();
        Close();
    }

    public void OnBuy()
    {
        if (GameData.Ins.PlayerData.coin < currentItem.Cost) return;
        GameData.Ins.PlayerData.coin -= currentItem.Cost;
        coinTxt.text = GameData.Ins.PlayerData.coin.ToString();
        // Change item data to OnUse and isBought = true in ShopData
        currentItem.itemData.isBought = true;
        currentItem.ShowBoughtIcon();
        // Change item data to OnUse in PlayerData
        GameData.Ins.PlayerData.SaveTypeDataFromShopType(currentShopBar.Type, currentItem.Item.poolType);
        ChangeShopButton();
    }
    
    public void UseButton()
    {
        GameData.Ins.PlayerData.SaveTypeDataFromShopType(currentShopBar.Type, currentItem.Item.poolType);
        ChangeShopButton();
    }
    
    public void SelectBar(ShopBar shopBar)
    {
        currentShopBar = shopBar;
        if (currentItem != null) currentItem.Release();
        _shopItemPool.Collect();
        SetItemListInBar(shopBar.Type);
    }

    private void SetItemListInBar(ShopType shopType)
    {
        List<ItemData> itemDataList = shopData.GetItemDataList(shopType);
        for (int i = 0; i < itemDataList.Count; i++)
        {
            ShopItem shopItem = _shopItemPool.Spawn();
            shopItem.SetData(itemDataList[i], this);
            if (shopItem.Item.poolType != GameData.Ins.PlayerData.GetTypeDataFromShopType(shopType)) continue;
            currentItem = shopItem;
            currentItem.OnSelect();
        }

        ChangeShopButton();
    }

    public void ChangeShopButton()
    {
        priceTxt.text = currentItem.Cost.ToString();
        onUsePanel.SetActive(
            currentItem.Item.poolType == GameData.Ins.PlayerData.GetTypeDataFromShopType(currentShopBar.Type));
        useButton.SetActive(currentItem.itemData.isBought);
        buyButton.SetActive(!currentItem.itemData.isBought);
    }
}
