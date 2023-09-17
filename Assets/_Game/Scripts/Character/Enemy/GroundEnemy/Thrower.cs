using System;
using UnityEngine;

public class Thrower : GroundEnemy
{
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected Transform spawnPoint;
    
    protected override void IdleState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float timeWait = 0f;
        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_IDLE);
            timeWait = 1.5f;
        };

        onExecute = () =>
        {
            if (timeWait > 0) timeWait -= Time.deltaTime;
            else StateMachine.ChangeState(AttackState);
        };

        onExit = () => { };
    }

    private void AttackState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float attackTime = 0f;
        Entity playerT = LevelManager.Ins.player;
        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_ATTACK);
            attackTime = 0.5f;
        };
        onExecute = () =>
        {
            if (attackTime > 0)
            {
                Utilities.LookTarget(skin.Tf, playerT.Tf);
                attackTime -= Time.deltaTime;
            }
            else if (!IsDie())
            {
                
                OnFire(playerT, entityData.damage, entityData.bulletSpeed);
                StateMachine.ChangeState(IdleState);
            }
        };
        onExit = () => { };
    }

    private void OnFire(Entity target, int damageIn, float bulletSpeedIn)
    {
        Vector3 position = spawnPoint.position;
        Vector3 targetPosition = target.GetSkinPosition();
        Bullet init = SimplePool.Spawn<Bullet>(bullet,
            position, Quaternion.identity);
        // TEST
        init.OnInit(position, targetPosition, bulletSpeedIn, damageIn, 0.75f);
    }
    
}
