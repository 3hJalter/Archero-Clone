using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Player player;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private int stageIndex;

    [SerializeField] private int levelIndex;
    [SerializeField] private int coinInLevel;
    [SerializeField] public int totalCoinGet;
    [SerializeField] public int lastPlayerHealth;

    [SerializeField] private List<Enemy> enemyList = new();

    private Level _currentLevel;

    public int LEVELIndex => levelIndex;

    public int StageIndex
    {
        get => stageIndex;
        set => stageIndex = value;
    }

    private void Start()
    {
        coinInLevel = 0;
        GameData.GetLastPlayedData(player, out lastPlayerHealth, out totalCoinGet, out levelIndex,
            out stageIndex);
        // COMMENT FOR TEST
        LoadLevel();
        OnInit();
        UIManager.Ins.OpenUI<MainMenu>();
    }

    public LevelType GetCurrentLevelType()
    {
        return _currentLevel.LEVELType;
    }

    public Transform GetCurrentLevelBossSpawnPoint()
    {
        return _currentLevel.BossSpawnPoint;
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
            if (enemyList[i].IsDie() || enemyList[i].notBeDetected) continue;
            float distance = Vector3.Distance(enemyList[i].Tf.position, player.Tf.position);
            if (!(distance < minDistance)) continue;
            minDistance = distance;
            enemy = enemyList[i];
        }

        return enemy;
    }

    public void OnPlayerDeath()
    {
        UIManager.Ins.OpenUI<Revive>();
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
        AudioManager.Ins.PlayBgm(_currentLevel.LEVELType == LevelType.Boss ? BgmType.Boss : BgmType.InGame);
        // OpenNextLevelUI();
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
        if (!openMainMenu) return;
        UIManager.Ins.OpenUI<MainMenu>();
        AudioManager.Ins.PlayBgm(BgmType.MainMenu);
        AudioManager.Ins.StopSfx();
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

    internal bool IsLastLevel()
    {
        return levelIndex >= GameData.Ins.StageDataList[stageIndex].CountLevel() - 1;
    }

    internal void OnNextLevel()
    {
        levelIndex++;
        lastPlayerHealth = player.GetHealth();
        GameData.SavePlayerLastPlayedData(lastPlayerHealth, totalCoinGet);
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
        GameData.SaveLevelAndStageData(stageIndex, levelIndex);
    }

    private void ResetStageData()
    {
        levelIndex = 0;
        stageIndex = 0;
        GameData.SaveLevelAndStageData(stageIndex, levelIndex);
    }

    public void ResetLevelData(bool isNextStageReset = false)
    {
        if (isNextStageReset) GameData.Ins.PlayerData.coin += totalCoinGet;
        levelIndex = 0;
        totalCoinGet = 0;
        lastPlayerHealth = player.GetMaxHealth();
        GameData.SavePlayerLastPlayedData(lastPlayerHealth, totalCoinGet);
        GameData.SaveLevelAndStageData(stageIndex, levelIndex);

    }
}
