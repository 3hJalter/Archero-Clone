using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Coin : HMonoBehaviour
{
    public void OnInit(Vector3 position)
    {
        Tf.position = position;
        Tf.DOJump(GetRandomXZPositionFromCurrent(),
            4, 1, 0.5f)
            .SetEase(Ease.Linear);
    }

    private Vector3 GetRandomXZPositionFromCurrent()
    {
        Vector3 position = Tf.position;
        float x = position.x + Random.Range(-2f, 2f);
        float z = position.z + Random.Range(-2f, 2f);
        return new Vector3(x, position.y, z);
    }
}
