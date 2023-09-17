using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelType levelType = LevelType.Normal;
    [SerializeField] private List<EnemySpawn> enemySpawns = new();
    [SerializeField] private NavMeshData navMeshData;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Door door;
    public Transform PlayerSpawnPoint => playerSpawnPoint;

    public void OnInit()
    {
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(navMeshData);
        OnOpenOtherUI();
    }

    public void OnSpawnEnemy()
    {
        for (int i = 0; i < enemySpawns.Count; i++)
        for (int j = 0; j < enemySpawns[i].spawnPoints.Count; j++)
            SimplePool.Spawn<Enemy>((PoolType)enemySpawns[i].enemyType,
                    enemySpawns[i].spawnPoints[j].position, enemySpawns[i].spawnPoints[j].rotation)
                .OnInit();

        door.OnInit();
    }

    public void OpenDoor()
    {
        door.OpenDoor();
    }

    private void OnOpenOtherUI()
    {
        if (levelType == LevelType.Normal) return;
        switch (levelType)
        {
            case LevelType.Tutorial:
                // Open Tutorial UI
                break;
            case LevelType.Boss:
                // OPen Boss UI
                break;
        }
    }
}

[Serializable]
internal class EnemySpawn
{
    public EnemyType enemyType;
    public List<Transform> spawnPoints;
}
