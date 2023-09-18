using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Player player;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private int stageIndex;

    public int LEVELIndex => levelIndex;

    [SerializeField] private int levelIndex;
    [SerializeField] private int coinInLevel;
    [SerializeField] public int totalCoinGet;
    [SerializeField] public int lastPlayerHealth;

    [SerializeField] private List<Enemy> enemyList = new();

    private Level _currentLevel;

    public int StageIndex
    {
        get => stageIndex;
        set => stageIndex = value;
    }

    private void Start()
    {
        coinInLevel = 0;
        lastPlayerHealth = PlayerPrefs.GetInt(Constants.LAST_PLAYER_HEALTH,
            player.GetMaxHealth());
        totalCoinGet = PlayerPrefs.GetInt(Constants.TOTAL_COIN_GET_IN_LEVEL, 0);
        levelIndex = PlayerPrefs.GetInt(Constants.LEVEL, 0);
        stageIndex = PlayerPrefs.GetInt(Constants.STAGE, 0);
        // COMMENT FOR TEST
        LoadLevel();
        OnInit();
        UIManager.Ins.OpenUI<MainMenu>();
    }

    public bool CanContinue()
    {
        return levelIndex != 0;
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

    public void OnPlayerDeath()
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

        if (stageIndex >= GameData.Ins.StageDataList.Count)
            ResetStageData();

        if (levelIndex >= GameData.Ins.StageDataList[stageIndex].CountLevel())
        {
            stageIndex++;
            if (stageIndex >= GameData.Ins.StageDataList.Count) ResetStageData();
            else ResetLevelData();
        }

        // if (levelIndex >= levelData.levelList.Count)
        // {
        //     levelIndex = 0;
        //     PlayerPrefs.SetInt(Constants.LEVEL, level);
        // }
        // _currentLevel = Instantiate(levelData.levelList[levelIndex]);

        _currentLevel = Instantiate(GameData.Ins.StageDataList[stageIndex].GetLevel(levelIndex));
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

    internal void OnResetStage(bool openMainMenu = true)
    {
        ResetLevelData();
        LoadLevel();
        OnInit();
        coinInLevel = 0;
        
        if (openMainMenu) UIManager.Ins.OpenUI<MainMenu>();
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

    internal void OpenNextLevelUI()
    {
        UIManager.Ins.OpenUI<NextLevel>();
    }
    
    internal void OnNextLevel()
    {
        levelIndex++;
        PlayerPrefs.SetInt(Constants.TOTAL_COIN_GET_IN_LEVEL, totalCoinGet);
        lastPlayerHealth = player.GetHealth();
        PlayerPrefs.SetInt(Constants.LAST_PLAYER_HEALTH, lastPlayerHealth);
        if (levelIndex >= GameData.Ins.StageDataList[stageIndex].CountLevel())
        {
            GameData.Ins.StageDataList[stageIndex].PassStage();
            stageIndex++;
            if (stageIndex >= GameData.Ins.StageDataList.Count) ResetStageData(); // Reset Stage to 0
            else ResetLevelData(true);
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

    internal void SetOldHealthForPlayer()
    {
        player.SetHealth(lastPlayerHealth);
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

    public void ResetLevelData(bool isNextStageReset = false)
    {
        if (isNextStageReset) GameData.Ins.PlayerData.coin += totalCoinGet;
        levelIndex = 0;
        totalCoinGet = 0;
        lastPlayerHealth = player.GetMaxHealth();
        PlayerPrefs.SetInt(Constants.LAST_PLAYER_HEALTH,lastPlayerHealth);
        PlayerPrefs.SetInt(Constants.TOTAL_COIN_GET_IN_LEVEL, totalCoinGet);
        PlayerPrefs.SetInt(Constants.STAGE, stageIndex);
        PlayerPrefs.SetInt(Constants.LEVEL, levelIndex);
    }
}
