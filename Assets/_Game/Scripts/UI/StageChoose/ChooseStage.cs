using System;
using UnityEngine;

public class ChooseStage : UICanvas
{
    [SerializeField] private StageButton stageButtonPrefab;
    [SerializeField] private Transform content;
    private readonly MiniPool<StageButton> _stageButtonPool = new ();
    
    private void Awake()
    {
        _stageButtonPool.OnInit(stageButtonPrefab, 0, content);
    }

    public override void Setup()
    {
        base.Setup();
        OnInit();
    }

    
    
    public override void Close()
    {
        _stageButtonPool.Collect();
        base.Close();
    }

    private void OnInit()
    {
        // Show Stage Button
        if (LevelManager.Ins.CanContinue())
            UIManager.Ins.OpenUI<ContinueStagePopup>();
        for (int i = 0; i < GameData.Ins.StageDataList.Count; i++)
        {
            Stage stage = GameData.Ins.StageDataList[i];
            if (i == 0 || stage.IsPassed() || GameData.Ins.StageDataList[i-1].IsPassed())
                _stageButtonPool.Spawn().OnInit(this, GameData.Ins.StageDataList[i], i, true);
            else _stageButtonPool.Spawn().OnInit(this, GameData.Ins.StageDataList[i], i);
        }
            
    }
    
    public void CloseButton()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close();
    }
    
}
