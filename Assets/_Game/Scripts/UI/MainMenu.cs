using TMPro;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        levelText.text = Constants.LEVEL + " " + PlayerPrefs.GetInt(Constants.LEVEL);
    }

    private void OnEnable()
    {
        levelText.text = Constants.LEVEL + " " + PlayerPrefs.GetInt(Constants.LEVEL);
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
