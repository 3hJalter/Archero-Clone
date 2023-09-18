using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData", order = 1)]
public class StageData : ScriptableObject
{
    public List<Stage> stageList;
}

[Serializable]
public class Stage
{
    public StageName stageName;
    public Sprite image;
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
    
    public string GetName()
    {
        StringBuilder result = new();

        foreach (char c in stageName.ToString())
        {
            if (char.IsUpper(c) && result.Length > 0)
            {
                result.Append(' ');
            }
            result.Append(c);
        }
        return result.ToString();
    }
}
