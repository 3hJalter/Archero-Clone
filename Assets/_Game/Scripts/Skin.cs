using System;
using UnityEngine;

public class Skin : HMonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Renderer pant;
    
    public Weapon OnEquipWeaponFromData(WeaponType weaponType)
    {
        Weapon weapon = GameData.Ins.GetWeapon(weaponType);
        return Instantiate(weapon, rightHand);
    }

    public Hat OnEquipHatFromData(HatType hatType)
    {
        Hat hat = GameData.Ins.GetHat(hatType);
        return Instantiate(hat, head);
    }

    public Material OnEquipPantFromData(PantType pantType)
    {
        if (pantType == PantType.None)
        {
            pant.materials = Array.Empty<Material>();
            return null;
        }
        Material pantMaterial = GameData.Ins.GetPant(pantType);
        pant.material = pantMaterial;
        return pantMaterial;
    }
}
