using System;
using System.Collections.Generic;
using UnityEngine;

public class Golem : GroundEnemy
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private List<SpawnBulletPointAndDirection> spawnPoints;

    protected override void IdleState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float timeWait = 0f;
        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_IDLE);
            timeWait = 1f;
        };

        onExecute = () =>
        {
            if (timeWait > 0)
            {
                timeWait -= Time.deltaTime;
            }
            else
            {
                if (Utilities.Chance(50)) StateMachine.ChangeState(PatrolState);
                else StateMachine.ChangeState(AttackState);
            }
        };

        onExit = () => { };
    }

    private void PatrolState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float timePatrol = 0f;
        onEnter = () =>
        {
            timePatrol = 1f;
            ChangeAnim(Constants.ANIM_RUN);
            NavMeshAgent.SetDestination(playerTf.position);
        };
        onExecute = () => Utilities.DoAfterSeconds(ref timePatrol, Execute, Wait);
        onExit = () => { };
        return;

        void Wait()
        {
            // Rotate the skin to the path of navmesh
            if (NavMeshAgent.velocity.normalized == Vector3.zero) return;
            skin.Tf.rotation = Quaternion.LookRotation(NavMeshAgent.velocity.normalized);
        }

        void Execute()
        {
            NavMeshAgent.ResetPath();
            StateMachine.ChangeState(AttackState);
        }
    }

    private void AttackState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float attackTime = 0f;
        onEnter = () =>
        {
            Utilities.LookTarget(skin.Tf, playerTf);
            ChangeAnim(Constants.ANIM_ATTACK);
            skin.ResetBodyRotation();
            attackTime = 0.75f;
        };
        onExecute = () => Utilities.DoAfterSeconds(ref attackTime, Execute, Wait);
        onExit = () => { };
        return;

        void Wait()
        {
            Utilities.LookTarget(skin.Tf, playerTf);
        }

        void Execute()
        {
            if (IsDie()) return;
            OnFire(entityData.damage, entityData.bulletSpeed);
            StateMachine.ChangeState(IdleState);
        }
    }


    private void OnFire(int damageIn, float bulletSpeedIn)
    {
        foreach (SpawnBulletPointAndDirection spawnPointIn in spawnPoints)
            FireWithDirection(bullet, damageIn, bulletSpeedIn, spawnPointIn);
    }
}
