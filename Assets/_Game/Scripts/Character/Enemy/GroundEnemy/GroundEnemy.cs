using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : Enemy
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    private Enemy _enemyImplementation;

    protected NavMeshAgent NavMeshAgent => navMeshAgent;

    public override void OnInit()
    {
        base.OnInit();
        navMeshAgent.speed = entityData.moveSpeed;
    }

    private void Update()
    {
        if (!GameManager.Ins.IsState(GameState.InGame))
        {
            NavMeshAgent.isStopped = true;
            return;
        }

        if (player != null && !player.IsDie())
        {
            HitPlayerAgain();
        }
        if (NavMeshAgent.isStopped) NavMeshAgent.isStopped = false;
        if (entityState == EntityState.Die) OnDeSpawn();
        else StateMachine?.Execute();
    }

    private void HitPlayerAgain()
    {
        if (timeHitPlayerAgain > 0) timeHitPlayerAgain -= Time.deltaTime;
        else
        {
            player.OnHit(entityData.damage);
            ResetTimeHit();
        }
    }
    
    protected bool IsReachDestination()
    {
        if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) return false;
        return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f;
    }
}