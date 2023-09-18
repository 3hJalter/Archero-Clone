using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private Bullet bullet;
    
    public void OnFireToTarget(Transform spawnPoint, Enemy target, int damage, float bulletSpeed)
    {
        // int damageSkill = damage;
        // float speedSkill = bulletSpeed;
        // if (owner.SkillDic.TryGetValue(SkillType.MoreDamage, out Skill skill))
        // {
        //     damageSkill *= (int) ((MoreDamageSkill)skill).DamageDic[skill.SkillLevel];
        // }
        Vector3 position = spawnPoint.position;
        Vector3 targetPosition = target.GetSkinPosition();
        SimplePool.Spawn<Bullet>(bullet,
            position, Quaternion.identity)
            .OnInit(position, targetPosition, bulletSpeed, damage, 0.75f);
    }
}
