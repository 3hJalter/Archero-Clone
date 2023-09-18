using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : UICanvas
{
    public override void Open()
    {
        Time.timeScale = 0;
        base.Open();
    }

    public override void Close()
    {
        Time.timeScale = 1;
        base.Close();
    }

    public void ContinueButton()
    {
        UIManager.Ins.OpenUI<Gameplay>();
        Close();
    }

    public void RetryButton()
    {
        LevelManager.Ins.OnRetry();
        Close();
    }

    public void MainMenuButton()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollower.Ins.ChangeState(CameraState.MainMenu);
        LevelManager.Ins.OnResetStage();
        Close();
    }
    
}
