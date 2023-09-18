using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    [SerializeField] private Image stageImage;
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private int stageIndex;
    
    public void OnInit(Stage stage, int index)
    {
        stageIndex = index;
        stageText.text = stage.GetName();
        if (!stage.IsPassed())
        {
            lockPanel.SetActive(true);
            TransparentImage();
        }
        else
        {
            lockPanel.SetActive(false);
            TransparentImage(1f);
        }
    }

    public void OnSelect()
    {
        LevelManager.Ins.StageIndex = stageIndex;
        LevelManager.Ins.OnResetStage(false);
        UIManager.Ins.OpenUI<Gameplay>();
    }
    
    private void TransparentImage(float alpha = 0.7f)
    {
        Color stageImageColor = stageImage.color;
        stageImageColor.a = 0.7f;
        stageImage.color = stageImageColor;
    }
}
