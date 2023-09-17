using System;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : GroundEnemy
{
    protected override void IdleState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float timeWait = 0f;
        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_IDLE);
            timeWait = 2f;
        };

        onExecute = () =>
        {
            if (timeWait > 0) timeWait -= Time.deltaTime;
            else StateMachine.ChangeState(PatrolState);
        };

        onExit = () => { };
    }

    private void PatrolState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_RUN);
            NavMeshAgent.SetDestination(GetRandomPoint());
        };

        onExecute = () =>
        {
            if (isNearPlayer)
            {
                StateMachine.ChangeState(AttackState);
                return;
            }
            if (!IsReachDestination()) return;
            StateMachine.ChangeState(IdleState);
        };

        onExit = () => { };
    }

    private void AttackState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float attackTime = 0f;
        onEnter = () =>
        {
            Debug.Log("Start Attack");
            ChangeAnim(Constants.ANIM_ATTACK);
            attackTime = 0.3f;
            NavMeshAgent.ResetPath();
        };
        onExecute = () =>
        {
            if (attackTime > 0) attackTime -= Time.deltaTime;
            else 
            {
                if (isNearPlayer)
                {
                    player.OnHit(entityData.damage);
                } StateMachine.ChangeState(IdleState);
            }
        };
        onExit = () => { };
    }

    protected override Vector3 GetRandomPoint()
    {
        Vector3 randomPoint = Vector3.zero;
        while (randomPoint == Vector3.zero)
        {
            Vector3 randomPointInCircle = UnityEngine.Random.insideUnitCircle * 10f;
            randomPoint = transform.position + randomPointInCircle;
            if (!NavMesh.SamplePosition(randomPoint, out NavMeshHit _, 1f, NavMesh.AllAreas)) randomPoint = Vector3.zero;
        }
        return randomPoint;
    }
}
