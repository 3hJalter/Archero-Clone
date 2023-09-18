using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        IInteractWallObject bullet = Cache.GetInteractWallObject(other.collider);
        bullet.OnHitWall();
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractWallObject bullet = Cache.GetInteractWallObject(other);
        bullet.OnHitWall();
    }
}
