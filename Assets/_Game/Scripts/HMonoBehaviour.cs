using UnityEngine;

public class HMonoBehaviour : MonoBehaviour
{
    private Transform tf;
    public Transform Tf
    {
        get
        {
            tf = tf ? tf : gameObject.transform;
            return tf;
        }
    }
}