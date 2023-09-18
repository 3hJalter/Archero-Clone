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

        onExecute = () => Utilities.DoAfterSeconds(ref timeWait, Execute);

        onExit = () => { };
        return;
        void Execute()
        {
            StateMachine.ChangeState(AttackState);
        }
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
        onExecute = () => Utilities.DoAfterSeconds(ref attackTime, Execute, Wait);
        onExit = () => { };
        return;
        void Wait()
        {
            Utilities.LookTarget(skin.Tf, playerT.Tf);
        }
        
        void Execute()
        {
            if (IsDie()) return;
            OnFire(playerT, entityData.damage, entityData.bulletSpeed);
            StateMachine.ChangeState(IdleState);
        }
    }

    private void OnFire(Entity target, int damageIn, float bulletSpeedIn)
    {
        Vector3 position = spawnPoint.position;
        Vector3 targetPosition = target.GetSkinPosition();
        Bullet init = SimplePool.Spawn<Bullet>(bullet,
            position, Quaternion.identity);
        init.OnInit(position, targetPosition, bulletSpeedIn, damageIn, 0.75f);
    }
    
}
