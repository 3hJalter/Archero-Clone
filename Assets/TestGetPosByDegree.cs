using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetPosByDegree : MonoBehaviour
{
    public GameObject object1; // Drag and drop your object1 in the inspector
    public GameObject object2; // Drag and drop your object2 in the inspector
    public GameObject cubePrefab; // The cube prefab you want to spawn

    [SerializeField] protected Vector3 direction;
    [SerializeField] protected float degree;
    [SerializeField] protected float radius;
    private void Start()
    {

        var position = object2.transform.position;
        var position1 = object1.transform.position;
        direction = position1 - position;
        radius = Vector3.Distance(position1, position);
        // Calculate the angle to rotate the direction by 45 degrees (in radians)
        float angleInRadians = Mathf.Deg2Rad * degree;

        float xOffset = Mathf.Cos(angleInRadians) * radius;
        float yOffset = position1.y; // Assuming no change in the vertical position
        float zOffset = Mathf.Sin(angleInRadians) * radius;

        Vector3 offset = new(xOffset, yOffset, zOffset);
        Vector3 desiredPosition = transform.position + offset;
        Instantiate(cubePrefab, desiredPosition, Quaternion.identity);
    }
}
