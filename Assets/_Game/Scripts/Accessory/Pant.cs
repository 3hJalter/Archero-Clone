using UnityEngine;

public class Pant : Item
{
    [SerializeField] private Material material;

    public Material Material => material;
}