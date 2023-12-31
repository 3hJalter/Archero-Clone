﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class BossSpider : GroundEnemy
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform spawnPointLocate;
    [SerializeField] private List<SpawnBulletPointAndDirection> spawnPoints;
    [SerializeField] private Canvas healthCanvas;
    [SerializeField] private int maxRollingNumber;
    [SerializeField] private SpriteRenderer fallViewPoint;
    [SerializeField] private List<Collider> collisionColliders;
    private readonly BossPhase _bossPhase = new();
    private int _currentRollingNumber;

    private bool _isAttack;
    private bool _isAttackDone;
    private bool _isJumpOne;


    public override void OnInit()
    {
        base.OnInit();
        ChangePhase(EnemyPhase.Phase1, AttackState, 35);
        healthCanvas.worldCamera = CameraFollower.Ins.GetCamera(CameraState.InGame);
    }

    protected override void IdleState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float timeWait = 0;
        onEnter = () =>
        {
            ChangeCollisionColliderEnable(true);
            timeWait = 1f;
            ChangeAnim(Constants.ANIM_IDLE);
        };
        onExecute = () => Utilities.DoAfterSeconds(ref timeWait, Execute);
        onExit = () => { };
        return;

        void Execute()
        {
            // More when in phase 2
            if (_bossPhase.EnemyPhase == EnemyPhase.Phase2
                && Utilities.Chance(25)
                && !_isJumpOne)
            {
                StateMachine.ChangeState(JumpState);
                return;
            }

            _isJumpOne = false;
            switch (_currentRollingNumber)
            {
                // Phase 1
                case <= 0 when !_isAttack:
                    StateMachine.ChangeState(_bossPhase.attackAction);
                    _currentRollingNumber = maxRollingNumber;
                    break;
                case > 0 when _isAttack:
                    StateMachine.ChangeState(RollingState);
                    _isAttack = false;
                    break;
                default:
                {
                    if (Utilities.Chance(_bossPhase.chanceToAttack))
                    {
                        StateMachine.ChangeState(_bossPhase.attackAction);
                        _currentRollingNumber = maxRollingNumber;
                    }
                    else
                    {
                        StateMachine.ChangeState(RollingState);
                        _isAttack = false;
                    }

                    break;
                }
            }
        }
    }

    private bool CanCreateNewDestination(out Vector3 destination)
    {
        Vector3 position = Tf.position;
        Vector3 direction = (playerTf.position - position).normalized;
        Vector3 sampleDestination = position + direction * 30f;
        if (NavMesh.SamplePosition(sampleDestination, out NavMeshHit hit, 30f, NavMesh.AllAreas))
        {
            destination = hit.position - direction * 4.5f;
            return true;
        }
        destination = default;
        return false;
    }
    
    private void RollingState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            _currentRollingNumber -= 1;
            ChangeCollisionColliderEnable(false);
            ChangeAnimWithOutCheckCurrent(Constants.ANIM_ROLL);
            if (CanCreateNewDestination(out Vector3 destination))
                NavMeshAgent.SetDestination(destination);
            else StateMachine.ChangeState(_bossPhase.attackAction);
            Utilities.LookTarget(skin.Tf, playerTf);
            AudioManager.Ins.PlaySfx(SfxType.BossSpiderRoll);
        };
        onExecute = Execute;
        onExit = () => { };
        return;

        void Execute()
        {
            if (!IsReachDestination()) return;
            StateMachine.ChangeState(Utilities.Chance(10) ? _bossPhase.attackAction : IdleState);
        }
    }

    private void ChangeCollisionColliderEnable(bool isEnable)
    {
        for (int i = 0; i < collisionColliders.Count; i++)
            collisionColliders[i].enabled = isEnable;
    }

    private void JumpState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float jumpTime = 0f;
        onEnter = () =>
        {
            ChangeCollisionColliderEnable(false);
            _isJumpOne = true;
            jumpTime = 1f;
            ChangeAnim(Constants.ANIM_JUMP);
            entityCollider.enabled = false;
            notBeDetected = true;
            NavMeshAgent.SetDestination(playerTf.position);
            fallViewPoint.enabled = true;
            AudioManager.Ins.PlaySfx(SfxType.BossSpiderJump);
        };
        onExecute = () => Utilities.DoAfterSeconds(ref jumpTime, Execute);
        onExit = () => { };
        return;

        void Execute()
        {
            if (!IsReachDestination()) return;
            StateMachine.ChangeState(FallState);
        }
    }

    private void FallState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float fallTime = 0.25f;
        onEnter = () =>
        {
            fallTime = 0.25f;
            ChangeAnim(Constants.ANIM_FALL);
        };
        onExecute = () => Utilities.DoAfterSeconds(ref fallTime, Execute);
        onExit = () => { };
        return;

        void Execute()
        {
            AudioManager.Ins.PlaySfx(SfxType.BossSpiderFall);
            entityCollider.enabled = true;
            notBeDetected = false;
            fallViewPoint.enabled = false;
            StateMachine.ChangeState(IdleState);
        }
    }

    private void AttackState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            ChangeCollisionColliderEnable(true);
            _isAttack = true;
            _isAttackDone = false;
            ChangeAnim(Constants.ANIM_ATTACK);
            Utilities.LookTarget(skin.Tf, playerTf);
            StartCoroutine(FireMultiple(entityData.damage, entityData.bulletSpeed, 5));
        };
        onExecute = Execute;
        onExit = () => { };
        return;

        void Execute()
        {
            if (!IsDie() && _isAttackDone) StateMachine.ChangeState(IdleState);
        }
    }

    private void Phase2AttackState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            _isAttack = true;
            _isAttackDone = false;
            ChangeAnim(Constants.ANIM_ATTACK);
            Utilities.LookTarget(skin.Tf, playerTf);
            const int shotNums = 10;
            const float timePerShoot = 0.2f;
            spawnPointLocate.transform.DOLocalRotate(new Vector3(0, 360, 0), shotNums * timePerShoot,
                RotateMode.FastBeyond360);
            StartCoroutine(FireMultiple(entityData.damage,
                entityData.bulletSpeed, shotNums));
        };
        onExecute = Execute;
        onExit = () => { };
        return;

        void Execute()
        {
            if (!IsDie() && _isAttackDone)
                StateMachine.ChangeState(IdleState);
        }
    }

    private void ChangePhase(EnemyPhase enemyPhase, StateMachine.StateAction attackAction, int chanceToAttack)
    {
        _bossPhase.EnemyPhase = enemyPhase;
        // Currently, only change attack action
        _bossPhase.attackAction = attackAction;
        _bossPhase.chanceToAttack = chanceToAttack;
        // Maybe add more by customize the function and BossPhase class
    }

    public override void OnHit(int damageHit)
    {
        base.OnHit(damageHit);
        if (entityData.health <= 0.5 * entityPrimitiveData.health)
            ChangePhase(EnemyPhase.Phase2, Phase2AttackState, 40);
    }

    private void OnFire(int damageIn, float bulletSpeedIn)
    {
        AudioManager.Ins.PlaySfx(SfxType.BossSpiderShoot);
        foreach (SpawnBulletPointAndDirection spawnPointIn in spawnPoints)
            FireWithDirection(bullet, damageIn, bulletSpeedIn, spawnPointIn);
    }

    private IEnumerator FireMultiple(int damageIn, float bulletSpeedIn, int fireNumber = 0,
        float timePerShoot = 0.2f)
    {
        for (int i = 0; i < fireNumber; i++)
        {
            // Pause if GameState is not InGame
            while (!GameManager.Ins.IsState(GameState.InGame))
                yield return null;
            if (IsDie()) break;
            OnFire(damageIn, bulletSpeedIn);
            yield return new WaitForSeconds(timePerShoot);
        }

        _isAttackDone = true;
    }

    // Temporary
    protected override void OnTriggerLogic(Collider other)
    {
        base.OnTriggerLogic(other);
        if (playerTrigger != null) playerTrigger.PushPlayer(NavMeshAgent.velocity, 1f);
    }

    private class BossPhase
    {
        public StateMachine.StateAction attackAction;
        public int chanceToAttack;
        public EnemyPhase EnemyPhase;
    }
}
