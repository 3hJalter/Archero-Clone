using TMPro;
using UnityEngine;

public class Lose : UICanvas
{
    [SerializeField] private TextMeshProUGUI moneyGain;
    [SerializeField] private TextMeshProUGUI stageReach;

    public void OnInit()
    {
        stageReach.text = "Reached stage " + LevelManager.Ins.StageIndex + " "
            + LevelManager.Ins.LEVELIndex;
        moneyGain.text = "Gained " + LevelManager.Ins.totalCoinGet + " coin";
    }
    
    public override void Close()
    {
        Time.timeScale = 1;
        base.Close();
    }
    
    public void OnClickMainMenu()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollower.Ins.ChangeState(CameraState.MainMenu);
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnResetStage();
    }
}
