using TMPro;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI levelText;
    private void Start()
    {
        levelText.text = Constants.STAGE + " " +
                         PlayerPrefs.GetInt(Constants.STAGE);
    }

    private void OnEnable()
    {
        levelText.text = Constants.STAGE + " " +
                         PlayerPrefs.GetInt(Constants.STAGE);
    }

    public void PlayButton()
    {
        LevelManager.Ins.OnStartGame();
        CameraFollower.Ins.ChangeState(CameraState.InGame);
        UIManager.Ins.OpenUI<Gameplay>();
        Close();
    }

    public void ShopButton()
    {
        UIManager.Ins.OpenUI<Shop>();
        Close();
    }
}
