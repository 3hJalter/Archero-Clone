using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PantData", menuName = "ScriptableObjects/PantData", order = 1)]
public class PantData : ScriptableObject
{
    [SerializeField] private List<PantItem> pantItems;

    public List<PantItem> PantItems => pantItems;

    public PantItem GetPantItem(PantType weaponType)
    {
        return pantItems.Single(q => q.type == weaponType);
    }
    
}

[System.Serializable]
public class PantItem
{
    public string name;
    public Material pant;
    public PantType type;
    public int cost;
    public int ads;
}