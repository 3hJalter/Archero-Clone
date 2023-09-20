using TMPro;
using UnityEngine;

public class Victory : UICanvas
{
    [SerializeField] private TextMeshProUGUI moneyGain;
    [SerializeField] private TextMeshProUGUI stageReach;

    public override void Setup()
    {
        base.Setup();
        stageReach.text = "Reached stage " + LevelManager.Ins.StageIndex + " "
                          + LevelManager.Ins.LEVELIndex;
        moneyGain.text = "Gained " + LevelManager.Ins.totalCoinGet + " coin";
    }
    
    public void OnClickMainMenu()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollower.Ins.ChangeState(CameraState.MainMenu);
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnNextLevel();
        AudioManager.Ins.PlayBgm(BgmType.MainMenu);
        AudioManager.Ins.StopSfx();
        UIManager.Ins.OpenUI<MainMenu>();
    }

    public void OnClickContinue()
    {
        UIManager.Ins.OpenUI<Gameplay>();
        Close();
    }
}
