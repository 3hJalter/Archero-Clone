using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CoinSpawner : HMonoBehaviour
{
    private readonly MiniPool<Coin> _coinPool = new();
    [SerializeField] private Coin coinPrefab;
    private void Awake()
    {
        _coinPool.OnInit(coinPrefab, 0, Tf);
    }
    
    public void SpawnCoin(Vector3 position, int coinNumber)
    {   
        int numSpawn = coinNumber / 20;
        for (int i = 0; i < numSpawn; i++)
            _coinPool.Spawn().OnInit(position);
    }

    public void MoveCoinToPlayer(Player player, Level currentLevel)
    {
        List<Coin> activeList = _coinPool.GetActiveList();
        Vector3 destination = player.Tf.position + Vector3.up * 0.5f;
        int count = activeList.Count;
        for (int i = 0; i < count; i++)
        {
            int i1 = i;
            activeList[i].Tf.DOMove(destination, 0.25f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (i1 != count - 1) return;
                    CollectCoin();
                    currentLevel.OpenDoor();
                    LevelManager.Ins.AddTotalCoin();
                });
        }
    }
    
    public void CollectCoin()
    {
        _coinPool.Collect();
    }
}
