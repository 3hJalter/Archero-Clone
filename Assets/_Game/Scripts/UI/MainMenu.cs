using TMPro;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI levelText;
    
    public override void Setup()
    {
        base.Setup();
        levelText.text = "Stage Reached: " +
                         GameData.Ins.GetReachedStageIndexString();
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
