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
        if (!GameManager.Ins.IsState(GameState.InGame) || IsDie())
        {
            if (NavMeshAgent.hasPath) NavMeshAgent.isStopped = true;
            return;
        }
        if (playerTrigger != null && !playerTrigger.IsDie())
        {
            HitPlayerAgain();
        }
        if (NavMeshAgent.isStopped) NavMeshAgent.isStopped = false;
        StateMachine?.Execute();
    }

    private void HitPlayerAgain()
    {
        if (timeHitPlayerAgain > 0) timeHitPlayerAgain -= Time.deltaTime;
        else
        {
            playerTrigger.OnHit(entityData.damage);
            ResetTimeHit();
        }
    }

    protected override void OnPause()
    {
        base.OnPause();
        if (!navMeshAgent.hasPath) return;
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;
    }

    protected override void OnUnPause()
    {
        base.OnUnPause();
        if (navMeshAgent.hasPath)
            navMeshAgent.isStopped = false;
    }

    protected override void OnDie()
    {
        base.OnDie();
        if (!navMeshAgent.hasPath) return;
        NavMeshAgent.isStopped = true;
        NavMeshAgent.velocity = Vector3.zero;
    }

    protected bool IsReachDestination()
    {
        if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) return false;
        return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f;
    }
}
