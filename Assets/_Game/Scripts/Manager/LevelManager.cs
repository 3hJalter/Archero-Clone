using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelData levelData;
    public Player player;
    [SerializeField] private Level _currentLevel;
    [SerializeField] private Stage currentStage;
    [SerializeField] private int stageIndex;
    [SerializeField] private int levelIndex;

    [SerializeField] private List<Enemy> enemyList = new();

    private void Start()
    {
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
        enemyList.Remove(enemy);
        if (enemyList.Count == 0) OnFinishLevel();
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

    public void OnStartGame()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
    }

    private void OnFinishLevel()
    {
        _currentLevel.OpenDoor();
    }

    private void OnReset()
    {
        player.CancelAttack();
        SimplePool.CollectAll();
        enemyList.Clear();
    }

    internal void OnResetStage()
    {
        ResetLevelData();
        LoadLevel();
        OnInit();
        UIManager.Ins.OpenUI<MainMenu>();
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
        } else SaveStage();
        // PlayerPrefs.SetInt(Constants.LEVEL, levelIndex);
        OnReset();
        LoadLevel();
        player.Tf.position = _currentLevel.PlayerSpawnPoint.position;
    }
    
    private void SaveStage()
    {
        PlayerPrefs.SetInt(Constants.LEVEL, levelIndex);
        PlayerPrefs.SetInt(Constants.STAGE, stageIndex);
    }

    private void ResetStageData()
    {
        stageIndex = levelIndex = 0;
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
