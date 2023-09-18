﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Golem : GroundEnemy
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Transform> additionalSpawnPoints;

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
                if (Utilities.Chance(30)) StateMachine.ChangeState(PatrolState);
                else StateMachine.ChangeState(AttackState);
            }
        };

        onExit = () => { };
    }

    private void PatrolState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_RUN);
            Vector3 position = Tf.position;
            Vector3 destination = (LevelManager.Ins.player.Tf.position - position).normalized * 2f + position;
            Utilities.LookTarget(skin.Tf, destination);
            NavMeshAgent.SetDestination(destination);
        };
        onExecute = Execute;
        onExit = () => { };
        return;

        void Execute()
        {
            if (!IsReachDestination()) return;
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
            attackTime = 0.75f;
        };
        onExecute = () => Utilities.DoAfterSeconds(ref attackTime, Execute, Wait);
        onExit = () => { };
        return;

        void Wait()
        {
            skin.Tf.LookAt(playerT.Tf);
        }

        void Execute()
        {
            if (IsDie()) return;
            OnFire(playerT, entityData.damage, entityData.bulletSpeed);
            StateMachine.ChangeState(IdleState);
        }
    }

    private void OnFire(HMonoBehaviour target, int damageIn, float bulletSpeedIn)
    {
        Fire(spawnPoint);
        foreach (Transform spawnPointIn in additionalSpawnPoints) Fire(spawnPointIn);

        return;

        void Fire(Transform spawnPointIn)
        {
            Vector3 position = spawnPointIn.position;
            Bullet init = SimplePool.Spawn<Bullet>(bullet,
                position, Quaternion.identity);
            // TEST
            Vector3 localPosition = spawnPointIn.localPosition;
            Vector3 newTargetPos = new(localPosition.x, localPosition.y, localPosition.z + 10f);
            newTargetPos = spawnPointIn.TransformPoint(newTargetPos);
            newTargetPos = new Vector3(newTargetPos.x, target.Tf.position.y, newTargetPos.z);
            init.OnInit(position, newTargetPos, bulletSpeedIn, damageIn, 0.75f);
        }
    }
}
