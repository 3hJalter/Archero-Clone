using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "HatData", menuName = "ScriptableObjects/HatData", order = 1)]
public class HatData : ScriptableObject
{
    [SerializeField] private List<HatItem> hatItems;

    public List<HatItem> HatItems => hatItems;

    public HatItem GetHatItem(HatType weaponType)
    {
        return hatItems.Single(q => q.type == weaponType);
    }
    
}

[System.Serializable]
public class HatItem
{
    public string name;
    public Hat hat;
    public HatType type;
    public int cost;
    public int ads;
}