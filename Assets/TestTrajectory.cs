using UnityEngine;

public class TestTrajectory : MonoBehaviour
{
    [SerializeField]
    Transform target;
 
    [SerializeField]
    float initialAngle;
    private Vector3 _position;
    private Vector3 _velocity;
    [SerializeField] private float customGravity = 9.81f;
    void Start () {
        // var rigid = GetComponent<Rigidbody>();
        _position = transform.position;
        Vector3 p = target.position;
 
        // float gravity = Physics.gravity.magnitude;
        float gravity = customGravity;

        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;
 
        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);
 
        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;
 
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
 
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
 
        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
 
        // Fire!
        // rigid.velocity = finalVelocity;
        _velocity = finalVelocity;
        
        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }
    
    void FixedUpdate()
    {
        // Update the position based on velocity and custom gravity
        _position += _velocity * Time.fixedDeltaTime;
        _velocity.y -= customGravity * Time.fixedDeltaTime;

        // Update the object's position
        transform.position = _position;
    }
}
