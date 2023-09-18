using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public List<Stage> stageList;
}

[Serializable]
public class Stage
{
    public StageName stageName;
    [SerializeField] private bool isPassed;
    [SerializeField] private List<Level> levelList;
    
    public int CountLevel()
    {
        return levelList.Count;
    }
    
    public Level GetLevel(int index)
    {
        if (index < 0 || index >= levelList.Count) return null;
        return levelList[index];
    }

    public void PassStage()
    {
        isPassed = true;
    }
    
    public bool IsPassed()
    {
        return isPassed;
    }
}
