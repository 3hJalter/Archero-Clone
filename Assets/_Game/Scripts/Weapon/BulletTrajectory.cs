using UnityEngine;
using DG.Tweening;
public class BulletTrajectory : Bullet
{
    public override void OnInit(Vector3 initPos, Vector3 targetInput, float velocityScaleIn, int damageIn,
        float offsetY)
    {
        base.OnInit(initPos, targetInput, velocityScaleIn, damageIn, offsetY);
        Tf.DOJump(targetPos - Vector3.down * 0.5f, velocityScale, 1, 1f).OnComplete(OnReachDestination);
    }

    protected override void OnReachDestination()
    {
        OnDeSpawn();
        // Invoke(nameof(OnDeSpawn), 0.25f);
    }
}
