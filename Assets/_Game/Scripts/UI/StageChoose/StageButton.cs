using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    
    [SerializeField] private Image stageImage;
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private int stageIndex;
    private ChooseStage _chooseStage;
    
    public void OnInit(ChooseStage chooseStage, Stage stage, int index, bool isPassed = false)
    {
        _chooseStage = chooseStage;
        Sprite image = stage.image;
        stageImage.sprite = image;
        stageImage.rectTransform.sizeDelta = new Vector2(image.rect.width, image.rect.height);
        stageIndex = index;
        stageText.text = stage.GetName();
        if (isPassed == false)
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
        LevelManager.Ins.OnStartGame();
        CameraFollower.Ins.ChangeState(CameraState.InGame);
        UIManager.Ins.OpenUI<Gameplay>();
        _chooseStage.Close();
    }
    
    private void TransparentImage(float alpha = 0.5f)
    {
        Color stageImageColor = stageImage.color;
        stageImageColor.a = alpha;
        stageImage.color = stageImageColor;
    }
}
