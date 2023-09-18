using DG.Tweening;
using UnityEngine;

public class Bullet : GameUnit, IInteractWallObject
{   
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector3 _currentPos;
    private int _damage;
    private RaycastHit _hit;

    protected bool isHitWall;
    private float _timeToDeSpawn;
    protected Vector3 targetPos;
    protected Vector3 velocity;
    protected float velocityScale;

    protected void Update()
    {
        if (isHitWall) return;
        _currentPos = Tf.position;
        if (IsHitEnemy(out _hit)) OnHitTarget();
        if (_timeToDeSpawn <= 0) OnDeSpawn();
        _timeToDeSpawn -= Time.deltaTime;
    }

    protected virtual void OnReachDestination()
    {
    }

    public virtual void OnInit(Vector3 initPos, Vector3 targetInput, float velocityScaleIn, int damageIn,
        float offsetY)
    {
        _timeToDeSpawn = 5f;
        isHitWall = false;
        targetPos = targetInput + Vector3.up * offsetY;
        velocityScale = velocityScaleIn;
        _damage = damageIn;
    }

    protected void OnDeSpawn()
    {
        trailRenderer.Clear();
        SimplePool.Despawn(this);
    }

    protected bool IsReachDestination()
    {
        float distance = (targetPos - _currentPos).sqrMagnitude;
        return distance < 0.1f;
    }

    private bool IsHitEnemy(out RaycastHit outHit)
    {
        return Physics.SphereCast(Tf.position, 0.5f,
            (targetPos - _currentPos).normalized, out outHit, 1f, targetLayer);
    }

    private void OnHitTarget()
    {
        Tf.DOKill();
        Entity targetHit = Cache.GetEntity(_hit.collider);
        targetHit.OnHit(_damage);
        OnDeSpawn();
    }

    public void OnHitWall()
    {
        // MORE: Add effect when hit wall
        isHitWall = true;
        Tf.DOKill();
        Invoke(nameof(OnDeSpawn), 0.25f);
    }
}
