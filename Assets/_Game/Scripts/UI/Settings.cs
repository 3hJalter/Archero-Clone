using UnityEngine;

public class Settings : UICanvas
{
    public override void Open()
    {
        GameManager.Ins.ChangeState(GameState.Pause);
        base.Open();
    }

    public override void Close()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
        base.Close();
    }

    private void StillPauseClose()
    {
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
        UIManager.Ins.OpenUI<GoMainMenuPopup>();
        StillPauseClose();
    }
    
}
