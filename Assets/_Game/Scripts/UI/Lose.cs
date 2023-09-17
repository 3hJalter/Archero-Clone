using UnityEngine.UI;

public class Lose : UICanvas
{
    public Text score;

    public void MainMenuButton()
    {
        
        UIManager.Ins.OpenUI<MainMenu>();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        // LevelManager.Ins.SetupLevel();
        Close();
    }

    public void RetryButton()
    {
        UIManager.Ins.OpenUI<Gameplay>();
        GameManager.Ins.ChangeState(GameState.InGame);
        // LevelManager.Ins.SetupLevel();
        Close();
    }
}
