using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Revive : UICanvas
{
    [SerializeField] private TextMeshProUGUI reviveTime;
    [SerializeField] private TextMeshProUGUI moneyGain;
    [SerializeField] private TextMeshProUGUI stageReach;
    [SerializeField] private GameObject reviveButton;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject reviveImageObj;
    private float _reviveTime;
    private bool _timeOut;
    public void OnInit()
    {
        _timeOut = false;
        _reviveTime = 5f;
        stageReach.gameObject.SetActive(false);
        moneyGain.gameObject.SetActive(false);
        reviveImageObj.SetActive(true);
        reviveButton.SetActive(true);
        mainMenuButton.SetActive(false);
        GameManager.Ins.ChangeState(GameState.Pause);
    }
    
    private void Update()
    {
        if (_timeOut) return;
        _reviveTime -= Time.deltaTime;
        reviveTime.text = Mathf.CeilToInt(_reviveTime).ToString();
        if (!(_reviveTime <= 0)) return;
        _timeOut = true;
        reviveImageObj.SetActive(false);
        reviveButton.SetActive(false);
        mainMenuButton.SetActive(true);   
        stageReach.text = "Reached\nstage " + LevelManager.Ins.StageIndex + "-"
                          + LevelManager.Ins.LEVELIndex;
        moneyGain.text = "Gained " + LevelManager.Ins.totalCoinGet + " coin";
        stageReach.gameObject.SetActive(true);
        moneyGain.gameObject.SetActive(true);
    }
    
    public void MainMenuButton()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        CameraFollower.Ins.ChangeState(CameraState.MainMenu);
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnOutStage();
    }

    public void ReviveButton()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
        LevelManager.Ins.OnRevive();
        Close();
    }
    
}