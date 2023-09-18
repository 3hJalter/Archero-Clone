using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : Entity
{
    [Title("Enemy Component")] [SerializeField]
    protected bool isNearPlayer;

    protected readonly StateMachine StateMachine = new();
    private bool _isInCameraView;

    protected MiniPool<Bullet> bulletPool = new();

    protected Player player;

    protected float timeHitPlayerAgain = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Constants.TAG_PLAYER)) return;
        player = Cache.GetPlayer(other);
        isNearPlayer = true;
        player.OnHit(entityData.damage);
    }

    private void OnTriggerExit(Collider other)
    {
        player = null;
        isNearPlayer = false;
        ResetTimeHit();
    }

    public override void OnInit()
    {
        base.OnInit();
        StateMachine.ChangeState(IdleState);
        LevelManager.Ins.OnAddEnemy(this);
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
        LevelManager.Ins.OnEnemyDeath(this);
    }


    protected virtual Vector3 GetRandomPoint()
    {
        return Vector3.zero;
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
