using UnityEngine;

[CreateAssetMenu(fileName = "EntityPrimitiveData", menuName = "ScriptableObjects/EntityPrimitiveData", order = 1)]
public class EntityPrimitiveData : ScriptableObject
{
    public int health;
    public int damage;
    public float bulletSpeed;
    public float attackSpeed;
    public float moveSpeed;
    public float deSpawnTime;
}
