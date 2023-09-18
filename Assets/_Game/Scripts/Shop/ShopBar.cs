using UnityEngine;

public class ShopBar : HMonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private ShopType type;

    private Shop _shop;
    public ShopType Type => type;

    public void SetShop(Shop shop)
    {
        _shop = shop;
    }

    public void OnSelect()
    {
        _shop.currentShopBar.Release();
        _shop.SelectBar(this);
        bg.SetActive(true);
    }

    public void Release()
    {
        bg.SetActive(false);
    }
}
