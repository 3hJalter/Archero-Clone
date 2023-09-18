using UnityEngine;

public class GoMainMenuPopup : UICanvas
{
    public override void Close()
    {
        Time.timeScale = 1;
        base.Close();
    }

    private void StillFreezeClose()
    {
        base.Close();
    }
    
    public void OnClickMainMenu()
    {
        Time.timeScale = 1;
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollower.Ins.ChangeState(CameraState.MainMenu);
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnResetStage();
    }

    public void OnClickContinue()
    {
        UIManager.Ins.OpenUI<Settings>();
        StillFreezeClose();
    }
}
