using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class BossSpider : GroundEnemy
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform spawnPointLocate;
    [SerializeField] private List<SpawnPointAndDirection> spawnPoints;

    [SerializeField] private Canvas healthCanvas;

    [SerializeField] private int maxRollingNumber;

    [SerializeField] private SpriteRenderer fallViewPoint;
    private readonly BossPhase _bossPhase = new();
    private int _currentRollingNumber;

    private bool _isAttack;
    private bool _isAttackDone;
    private bool _isJumpOne;

    private Transform _playerTf;

    public override void OnInit()
    {
        base.OnInit();
        _playerTf = LevelManager.Ins.player.Tf;
        ChangePhase(Phase.Phase1, AttackState, 35);
        healthCanvas.worldCamera = CameraFollower.Ins.GetCamera(CameraState.InGame);
    }

    protected override void IdleState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float timeWait = 0;
        onEnter = () =>
        {
            timeWait = 1f;
            ChangeAnim(Constants.ANIM_IDLE);
        };
        onExecute = () =>
        {
            if (timeWait > 0)
            {
                timeWait -= Time.deltaTime;
            }
            else
            {
                // More when in phase 2
                if (_bossPhase.phase == Phase.Phase2
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
        };
        onExit = () => { };
    }

    private void RollingState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            _currentRollingNumber -= 1;
            ChangeAnimWithOutCheckCurrent(Constants.ANIM_ROLL);
            Utilities.LookTarget(skin.Tf, _playerTf);
            Vector3 position = Tf.position;
            Vector3 direction = (_playerTf.position - position).normalized;
            Vector3 sampleDestination = position + direction * 30f;
            if (NavMesh.SamplePosition(sampleDestination, out NavMeshHit hit, 30f, NavMesh.AllAreas))
                NavMeshAgent.SetDestination(hit.position - direction * 4.5f);
            else StateMachine.ChangeState(_bossPhase.attackAction);
        };
        onExecute = () =>
        {
            if (!IsReachDestination()) return;
            StateMachine.ChangeState(Utilities.Chance(10) ? _bossPhase.attackAction : IdleState);
        };
        onExit = () => { };
    }

    private void JumpState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float jumpTime = 0f;
        onEnter = () =>
        {
            _isJumpOne = true;
            jumpTime = 1f;
            ChangeAnim(Constants.ANIM_JUMP);
            entityCollider.enabled = false;
            LevelManager.Ins.OnRemoveEnemy(this);
            NavMeshAgent.SetDestination(_playerTf.position);
            fallViewPoint.enabled = true;
        };
        onExecute = () =>
        {
            if (jumpTime > 0) jumpTime -= Time.deltaTime;
            if (!IsReachDestination() && jumpTime > 0) return;
            StateMachine.ChangeState(FallState);
        };
        onExit = () => { };
    }

    private void FallState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float fallTime = 0.5f;
        onEnter = () =>
        {
            fallTime = 0.5f;
            ChangeAnim(Constants.ANIM_FALL);
        };
        onExecute = () =>
        {
            if (fallTime > 0)
            {
                fallTime -= Time.deltaTime;
            }
            else
            {
                entityCollider.enabled = true;
                LevelManager.Ins.OnAddEnemy(this);
                fallViewPoint.enabled = false;
                StateMachine.ChangeState(IdleState);
            }
        };
        onExit = () => { };
    }

    private void AttackState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            _isAttack = true;
            _isAttackDone = false;
            ChangeAnim(Constants.ANIM_ATTACK);
            Utilities.LookTarget(skin.Tf, _playerTf);
            StartCoroutine(FireMultiple(entityData.damage, entityData.bulletSpeed, 5));
        };
        onExecute = () =>
        {
            if (!IsDie() && _isAttackDone) StateMachine.ChangeState(IdleState);
        };
        onExit = () => { };
    }

    private void Phase2AttackState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            _isAttack = true;
            _isAttackDone = false;
            ChangeAnim(Constants.ANIM_ATTACK);
            Utilities.LookTarget(skin.Tf, _playerTf);
            const int shotNums = 10;
            const float timePerShoot = 0.2f;
            spawnPointLocate.transform.DOLocalRotate(new Vector3(0, 360, 0), shotNums * timePerShoot,
                RotateMode.FastBeyond360);
            StartCoroutine(FireMultiple(entityData.damage,
                entityData.bulletSpeed, shotNums));
        };
        onExecute = () =>
        {
            if (!IsDie() && _isAttackDone)
                StateMachine.ChangeState(IdleState);
        };
        onExit = () => { };
    }

    private void ChangePhase(Phase phase, StateMachine.StateAction attackAction, int chanceToAttack)
    {
        _bossPhase.phase = phase;
        // Currently, only change attack action
        _bossPhase.attackAction = attackAction;
        _bossPhase.chanceToAttack = chanceToAttack;
        // Maybe add more by customize the function and BossPhase class
    }

    public override void OnHit(int damageHit)
    {
        base.OnHit(damageHit);
        if (entityData.health <= 0.5 * entityPrimitiveData.health)
            ChangePhase(Phase.Phase2, Phase2AttackState, 40);
    }

    private void OnFire(int damageIn, float bulletSpeedIn)
    {
        foreach (SpawnPointAndDirection spawnPointIn in spawnPoints)
            FireOne(damageIn, bulletSpeedIn, spawnPointIn);
    }

    private void FireOne(int damageIn, float bulletSpeedIn, SpawnPointAndDirection spawnPointIn)
    {
        Vector3 position = spawnPointIn.spawnPoint.position;
        Vector3 direction = spawnPointIn.direction.position;
        SimplePool.Spawn<Bullet>(bullet, position, Quaternion.identity)
            .OnInit(position, direction, bulletSpeedIn, damageIn, 0f);
    }

    private IEnumerator FireMultiple(int damageIn, float bulletSpeedIn, int fireNumber = 0,
        float timePerShoot = 0.2f)
    {
        for (int i = 0; i < fireNumber; i++)
        {
            OnFire(damageIn, bulletSpeedIn);
            yield return new WaitForSeconds(timePerShoot);
        }

        _isAttackDone = true;
    }
}

[Serializable]
public class SpawnPointAndDirection
{
    public Transform spawnPoint;
    public Transform direction;
}


public class BossPhase
{
    public StateMachine.StateAction attackAction;
    public int chanceToAttack;
    public Phase phase;
}
