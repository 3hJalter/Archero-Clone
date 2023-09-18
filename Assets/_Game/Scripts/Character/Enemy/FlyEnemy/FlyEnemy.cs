using UnityEngine;

public class FlyEnemy : Enemy
{
    [SerializeField] private Vector3 destination;

    protected Vector3 Destination
    {
        get => destination;
        set => destination = value;
    }
    
    private void Update()
    {
        if (!GameManager.Ins.IsState(GameState.InGame)) return;
        if (entityState == EntityState.Die) OnDeSpawn();
        else StateMachine?.Execute();
    }   

    protected bool IsReachDestination()
    {
        return (Tf.position - destination).sqrMagnitude < 0.4f;
        // return !(Vector3.Distance(Tf.position, destination) > 0.2f);
    }
}
