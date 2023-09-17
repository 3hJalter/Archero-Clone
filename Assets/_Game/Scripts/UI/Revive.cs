using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Revive : UICanvas
{
    [SerializeField] private TextMeshProUGUI reviveTime;
    [SerializeField] private GameObject reviveButton;
    [SerializeField] private GameObject mainMenuButton;
    private float _reviveTime;
    private bool _timeOut;
    public void OnInit()
    {
        _timeOut = false;
        _reviveTime = 5f;
        reviveButton.SetActive(true);
        mainMenuButton.SetActive(false);
    }
    
    private void Update()
    {
        if (_timeOut) return;
        _reviveTime -= Time.unscaledDeltaTime;
        reviveTime.text = Mathf.CeilToInt(_reviveTime).ToString();
        if (!(_reviveTime <= 0)) return;
        _timeOut = true;
        mainMenuButton.SetActive(true);
    }
    
    public override void Open()
    {
        Time.timeScale = 0;
        base.Open();
    }

    public override void Close()
    {
        Time.timeScale = 1;
        base.Close();
    }
    
    public void MainMenuButton()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollower.Ins.ChangeState(CameraState.MainMenu);
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnRetry();
    }

    public void ReviveButton()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
        LevelManager.Ins.OnRevive();
        Close();
    }
    
}