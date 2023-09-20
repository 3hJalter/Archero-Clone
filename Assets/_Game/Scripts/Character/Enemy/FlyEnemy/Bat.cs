using System;
using UnityEngine;

public class Bat : FlyEnemy
{
    protected override void IdleState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        float timeWait = 0;
        onEnter = () =>
        {
            timeWait = 1f;
            ChangeAnim(Constants.ANIM_IDLE);
        };

        onExecute = () => Utilities.DoAfterSeconds(ref timeWait, Execute);

        onExit = () => { };
        return;

        void Execute()
        {
            if (isNearPlayer)
            {
                ChangeAnim(Constants.ANIM_ATTACK);
                if (playerTrigger != null) playerTrigger.OnHit(entityData.damage);
            }

            StateMachine.ChangeState(ChaseState);
        }
    }

    private void ChaseState(out Action onEnter, out Action onExecute, out Action onExit)
    {
        Vector3 targetPos = LevelManager.Ins.player.Tf.position;
        Vector3 direction = default;
        onEnter = () =>
        {
            Vector3 position = Tf.position;
            Destination = new Vector3(targetPos.x, position.y, targetPos.z);
            direction = (Destination - position).normalized;
            Utilities.LookTarget(skin.Tf, Destination);
            ChangeAnim(Constants.ANIM_RUN);
        };

        onExecute = Execute;

        onExit = () => { };
        return;

        void Execute()
        {
            if (IsReachDestination()) StateMachine.ChangeState(IdleState);
            else
                Tf.position += direction * (entityData.moveSpeed * Time.deltaTime);
        }
    }
}
