using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Player owner;
    [SerializeField] private Transform spawnPoint;
    
    public void OnFire(Enemy target, int damage, float bulletSpeed)
    {
        int damageSkill = damage;
        float speedSkill = bulletSpeed;
        // if (owner.SkillDic.TryGetValue(SkillType.MoreDamage, out Skill skill))
        // {
        //     damageSkill *= (int) ((MoreDamageSkill)skill).DamageDic[skill.SkillLevel];
        // }
        Vector3 position = owner.Tf.position;
        Vector3 targetPosition = target.GetSkinPosition();
        SimplePool.Spawn<Bullet>(bullet,
            position, Quaternion.identity)
            .OnInit(position, targetPosition, speedSkill, damageSkill, 0.75f);
        // TEST
        // owner.bulletPool.Spawn(position, Quaternion.identity).OnInit(
        //     position, targetPosition, speedSkill, damageSkill);
    }

    public void SetOwner(Player value)
    {
        owner = value;
    }

    public Bullet GetBullet()
    {
        return bullet;
    }
}
