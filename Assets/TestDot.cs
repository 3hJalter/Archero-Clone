using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestDot : MonoBehaviour
{
    [SerializeField] private Transform target;
    void Start()
    {
        transform.DOMove(target.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log("Complete");
        }   );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
