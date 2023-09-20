using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelType levelType = LevelType.Normal;

    public LevelType LEVELType => levelType;

    [SerializeField] private List<EnemySpawn> enemySpawns = new();
    [SerializeField] private NavMeshData navMeshData;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform bossSpawnPoint;

    public Transform BossSpawnPoint => bossSpawnPoint;

    [SerializeField] private Door door;
    public Transform PlayerSpawnPoint => playerSpawnPoint;

    public void OnInit()
    {
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(navMeshData);
        // if (levelType == LevelType.Boss) AudioManager.Ins.PlayBgm(BgmType.Boss);
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
}

[Serializable]
internal class EnemySpawn
{
    public EnemyType enemyType;
    public List<Transform> spawnPoints;
}
