using TMPro;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        levelText.text = "Stage Reached: " +
                         GameData.Ins.GetReachedStageIndexString();
    }

    public void PlayButton()
    {
        LevelManager.Ins.OnStartGame();
        CameraFollower.Ins.ChangeState(CameraState.InGame);
        UIManager.Ins.OpenUI<Gameplay>();
        Close();
    }

    public void OpenStageButton()
    {
        UIManager.Ins.OpenUI<ChooseStage>();
        Close();
    }

    public void ShopButton()
    {
        UIManager.Ins.OpenUI<Shop>();
        Close();
    }
}
