using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : UICanvas
{
    public void ContinueButton()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
        Close();
    }

    public void MainMenuButton()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        // LevelManager.Ins.SetupLevel();
        Close();
    }
}
