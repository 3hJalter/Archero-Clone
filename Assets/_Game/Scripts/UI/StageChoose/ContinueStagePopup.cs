using System;
using TMPro;
using UnityEngine;

public class ContinueStagePopup : UICanvas
{
    [SerializeField] private TextMeshProUGUI tmp;
    private void OnEnable()
    {
        tmp.text = "You reached level "
        + LevelManager.Ins.StageIndex 
        + "-" + LevelManager.Ins.LEVELIndex 
        + " in the last played time, continue?";
    }

    public void OnClickContinue()
    {
        LevelManager.Ins.SetOldHealthForPlayer();
        LevelManager.Ins.OnStartGame();
        CameraFollower.Ins.ChangeState(CameraState.InGame);
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<Gameplay>();
    }
    
    public void OnClickExit()
    {
        LevelManager.Ins.ResetLevelData();
        Close();
    }
}
