using UnityEngine;

public class GoMainMenuPopup : UICanvas
{
    public void OnClickMainMenu()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollower.Ins.ChangeState(CameraState.MainMenu);
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnResetStage();
    }

    public void OnClickContinue()
    {
        UIManager.Ins.OpenUI<Settings>();
        Close();
    }
}
