using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : Entity
{
    [Title("Enemy Component")] [SerializeField]
    protected bool isNearPlayer;

    protected readonly StateMachine StateMachine = new();
    private bool _isInCameraView;

    protected Player playerTrigger;
    protected Transform playerTf;
    protected float timeHitPlayerAgain = 1f;
    private bool _lockTrigger;
    private void OnTriggerEnter(Collider other)
    {
        // if (!other.CompareTag(Constants.TAG_PLAYER)) return;
        OnTriggerLogic(other);   
    }


    protected virtual void OnTriggerLogic(Collider other)
    {
        if (_lockTrigger) return ;
        _lockTrigger = true;
        UnlockTrigger();
        playerTrigger = Cache.GetPlayer(other);
        isNearPlayer = true;
        playerTrigger.OnHit(entityData.damage);
    }
    
    private void UnlockTrigger()
    {
        DOVirtual.DelayedCall(0.5f, () => _lockTrigger = false);
    }
    
    private void OnTriggerExit(Collider other)
    {
        playerTrigger = null;
        isNearPlayer = false;
        ResetTimeHit();
    }

    public override void OnInit()
    {
        base.OnInit();
        isNearPlayer = false;
        playerTrigger = null;
        StateMachine.ChangeState(IdleState);
        LevelManager.Ins.OnAddEnemy(this);
        playerTf = LevelManager.Ins.player.Tf;
    }

    protected void ResetTimeHit(float timeHit = 1f)
    {
        timeHitPlayerAgain = timeHit;
    }

    protected virtual void IdleState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () => { };
        onExecute = () => { };
        onExit = () => { };
    }

    protected override void OnDie()
    {
        base.OnDie();
        LevelManager.Ins.OnEnemyDeath(this, entityPrimitiveData.coin);
    }

    protected static void FireWithDirection(Bullet bulletPrefab, int damageIn, float bulletSpeedIn,
        SpawnBulletPointAndDirection spawnPointIn, float offsetY = 0f)
    {
        Vector3 position = spawnPointIn.spawnPoint.position;
        Vector3 direction = spawnPointIn.direction.position;
        SimplePool.Spawn<Bullet>(bulletPrefab, position, Quaternion.identity)
            .OnInit(position, direction, bulletSpeedIn, damageIn, offsetY);
    }

    protected override void DeSpawn()
    {
        SimplePool.Despawn(this);
    }
    
    // protected void CheckEnemyInView()
    // {
    //     bool currentIsInCameraView = CameraFollower.Ins.IsObjectInCameraView(Tf);
    //     switch (currentIsInCameraView)
    //     {
    //         case true when !_isInCameraView:
    //             _isInCameraView = true;
    //             LevelManager.Ins.EnemyList.Add(this);
    //             break;
    //         case false when _isInCameraView:
    //             LevelManager.Ins.EnemyList.Remove(this);
    //             _isInCameraView = false;
    //             break;
    //     }
    // }
}
