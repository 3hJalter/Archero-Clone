using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelData levelData;
    public Player player;
    [SerializeField] private Level _currentLevel;
    [SerializeField] private int levelIndex;
    
    [SerializeField] private List<Enemy> enemyList = new();
    public List<Enemy> EnemyList => enemyList;

    private void Start()
    {
        levelIndex = PlayerPrefs.GetInt(Constants.LEVEL, 0);
        // COMMENT FOR TEST
        LoadLevel(levelIndex);
        OnInit();
        UIManager.Ins.OpenUI<MainMenu>();
    }

    private void OnInit()
    {
        //set vi tri player
        player.Tf.position = _currentLevel.PlayerSpawnPoint.position;
        player.OnInit();
    }

    private void OnSpawnEnemy()
    {
        _currentLevel.OnSpawnEnemy();
    }

    public void OnAddEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
    }
    
    public void OnRemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }
    
    public void OnEnemyDeath(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        if (enemyList.Count == 0) _currentLevel.OpenDoor();
    }

    public Enemy GetNearestEnemy()
    {
        if (enemyList.Count <= 0)
        {
            return null;
        }
        float minDistance = float.MaxValue;
        Enemy enemy = enemyList[0];
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].IsDie()) continue;
            float distance = Vector3.Distance(enemyList[i].Tf.position, player.Tf.position);
            if (!(distance < minDistance)) continue;
            minDistance = distance;
            enemy = enemyList[i];
        }
        return enemy;
    }
    
    public void OnPlayerDeath()
    {
        UIManager.Ins.OpenUI<Revive>().OnInit();
    }
    
    private void LoadLevel(int level)
    {
        if (_currentLevel != null) Destroy(_currentLevel.gameObject);

        if (level >= levelData.levelList.Count)
        {
            level = 0;
            PlayerPrefs.SetInt(Constants.LEVEL, level);
        }
        _currentLevel = Instantiate(levelData.levelList[level]);
        _currentLevel.OnInit();
        OnSpawnEnemy();
    }

    private void ResetLevel()
    {
        OnSpawnEnemy();
    }
    
    public void OnStartGame()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
    }

    public void OnFinishLevel()
    {
        // OPEN DOOR IN LEVEL
    }

    private void OnReset()
    {
        player.CancelAttack();
        SimplePool.CollectAll();
        enemyList.Clear();
    }

    internal void OnRetry()
    {
        OnReset();
        ResetLevel();
        OnInit();
        UIManager.Ins.OpenUI<MainMenu>();
    }

    internal void OnRevive()
    {
        player.OnInit();
    }
    
    internal void OnNextLevel()
    {
        levelIndex++;
        PlayerPrefs.SetInt(Constants.LEVEL, levelIndex);
        OnReset();
        LoadLevel(levelIndex);
        player.Tf.position = _currentLevel.PlayerSpawnPoint.position;
    }
}