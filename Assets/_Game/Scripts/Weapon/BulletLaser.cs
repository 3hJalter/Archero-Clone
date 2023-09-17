using UnityEngine;

public class BulletLaser : Bullet
{

    private void FixedUpdate()
    {
        // Move the bullet in a straight line
        if (isHitWall) return;
        Vector3 newPosition = transform.position + (velocity * Time.fixedDeltaTime);
        Tf.position = newPosition;
    }
    
    public override void OnInit(Vector3 initPos, Vector3 targetInput, float velocityScaleIn, int damageIn,
        float offsetY)
    {
        base.OnInit(initPos, targetInput, velocityScaleIn, damageIn, offsetY);
        
        // Calculate the direction from initPos to targetPos
        Vector3 direction = (targetPos - initPos).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
        // Tf.LookAt(targetInput.Tf);
        
        // Set the initial velocity to move in that direction
        velocity = direction * velocityScaleIn;
    }
}
