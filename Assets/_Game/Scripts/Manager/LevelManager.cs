using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelData levelData;
    public Player player;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private int stageIndex;
    [SerializeField] private int levelIndex;
    [SerializeField] private int coinInLevel;
    [SerializeField] public int totalCoinGet;
    

    [SerializeField] private List<Enemy> enemyList = new();

    private Level _currentLevel;
    public int CoinInLevel => coinInLevel;

    private void Start()
    {
        coinInLevel = 0;
        levelIndex = PlayerPrefs.GetInt(Constants.LEVEL, 0);
        stageIndex = PlayerPrefs.GetInt(Constants.STAGE, 0);
        // COMMENT FOR TEST
        LoadLevel();
        OnInit();
        UIManager.Ins.OpenUI<MainMenu>();
    }

    private void OnInit()
    {
        //set vi tri player
        player.Tf.position = _currentLevel.PlayerSpawnPoint.position;
        player.OnInit();
        coinSpawner.CollectCoin();
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

    public void OnEnemyDeath(Enemy enemy, int coin)
    {
        enemyList.Remove(enemy);
        coinInLevel += coin;
        // Spawn coin
        coinSpawner.SpawnCoin(enemy.Tf.position, coin);
        //
        if (enemyList.Count == 0) Invoke(nameof(OnFinishLevel), 1f);
    }

    public Enemy GetNearestEnemy()
    {
        if (enemyList.Count <= 0) return null;
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

    public static void OnPlayerDeath()
    {
        UIManager.Ins.OpenUI<Revive>().OnInit();
    }

    private void LoadLevel()
    {
        if (_currentLevel != null)
        {
            SimplePool.CollectAll();
            enemyList.Clear();
            Destroy(_currentLevel.gameObject);
        }

        if (stageIndex >= levelData.stageList.Count)
            ResetStageData();

        if (levelIndex >= levelData.stageList[stageIndex].CountLevel())
        {
            stageIndex++;
            if (stageIndex >= levelData.stageList.Count) ResetStageData();
            else ResetLevelData();
        }

        // if (levelIndex >= levelData.levelList.Count)
        // {
        //     levelIndex = 0;
        //     PlayerPrefs.SetInt(Constants.LEVEL, level);
        // }
        // _currentLevel = Instantiate(levelData.levelList[levelIndex]);

        _currentLevel = Instantiate(levelData.stageList[stageIndex].GetLevel(levelIndex));
        _currentLevel.OnInit();
        OnSpawnEnemy();
    }

    private void ResetLevel()
    {
        OnSpawnEnemy();
    }

    public static void OnStartGame()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
    }

    private void OnFinishLevel()
    {
        coinSpawner.MoveCoinToPlayer(player, _currentLevel);
    }

    private void OnReset()
    {
        player.CancelAttack();
        SimplePool.CollectAll();
        enemyList.Clear();
        coinInLevel = 0;
    }

    internal void OnResetStage()
    {
        ResetLevelData();
        LoadLevel();
        OnInit();
        coinInLevel = 0;
        totalCoinGet = 0;
        UIManager.Ins.OpenUI<MainMenu>();
    }

    internal void OnOutStage()
    {
        GameData.Ins.PlayerData.coin += totalCoinGet;
        OnResetStage();
    }
    
    internal void OnRetry()
    {
        OnReset();
        ResetLevel();
        OnInit();
        UIManager.Ins.OpenUI<Gameplay>();
    }

    internal void OnRevive()
    {
        player.OnInit();
    }

    internal void OnNextLevel()
    {
        levelIndex++;
        if (levelIndex >= levelData.stageList[stageIndex].CountLevel())
        {
            levelData.stageList[stageIndex].PassStage();
            stageIndex++;
            if (stageIndex >= levelData.stageList.Count) ResetStageData();
            else ResetLevelData();
        }
        else
        {
            SaveStage();
        }

        // PlayerPrefs.SetInt(Constants.LEVEL, levelIndex);
        OnReset();
        LoadLevel();
        player.Tf.position = _currentLevel.PlayerSpawnPoint.position;
    }

    internal void AddTotalCoin()
    {
       totalCoinGet += coinInLevel;
       coinInLevel = 0;
    }
    
    private void SaveStage()
    {
        PlayerPrefs.SetInt(Constants.LEVEL, levelIndex);
        PlayerPrefs.SetInt(Constants.STAGE, stageIndex);
    }

    private void ResetStageData()
    {
        PlayerPrefs.SetInt(Constants.STAGE, 0);
        PlayerPrefs.SetInt(Constants.LEVEL, 0);
    }

    private void ResetLevelData()
    {
        levelIndex = 0;
        PlayerPrefs.SetInt(Constants.STAGE, stageIndex);
        PlayerPrefs.SetInt(Constants.LEVEL, levelIndex);
    }
}
