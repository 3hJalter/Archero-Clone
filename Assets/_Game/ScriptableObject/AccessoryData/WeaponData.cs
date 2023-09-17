using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private List<WeaponItem> weaponItems;

    public List<WeaponItem> WeaponItems => weaponItems;

    public WeaponItem GetWeaponItem(WeaponType weaponType)
    {
        return weaponItems.Single(q => q.type == weaponType);
    }
    
}

[System.Serializable]
public class WeaponItem
{
    public string name;
    public Weapon weapon;
    public WeaponType type;
    public int cost;
    public int ads;
}
