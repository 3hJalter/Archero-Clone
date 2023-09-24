using UnityEngine;

public class HMonoBehaviour : MonoBehaviour
{
    private Transform _tf;
    public Transform Tf
    {
        get
        {
            _tf = _tf ? _tf : gameObject.transform;
            return _tf;
        }
    }
}