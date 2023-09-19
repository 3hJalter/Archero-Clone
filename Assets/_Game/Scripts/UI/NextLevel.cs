using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NextLevel : UICanvas
{
    [SerializeField] private Image panel;
    [SerializeField] private Animation showBossAnim;
    public override void Close()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
        base.Close();
    }
    
    public void OnInit()
    {
        panel.gameObject.SetActive(true);
        GameManager.Ins.ChangeState(GameState.Transition);
        DOVirtual.Float(0,1, 0.5f, TransparentImage).OnComplete(
            () =>
            {
                LevelManager.Ins.OnNextLevel();
                DOVirtual.Float(1, 0, 0.5f, TransparentImage).OnComplete(
                    CheckIfLevelHasBoss);
            });
    }

    private void CheckIfLevelHasBoss()
    {
        if (LevelManager.Ins.GetCurrentLevelType() == LevelType.Boss)
        {   
            panel.gameObject.SetActive(false);
            showBossAnim.Play();
            CameraFollower.Ins.targetTf = LevelManager.Ins.GetCurrentLevelBossSpawnPoint();
            float halfAnimTime = showBossAnim.clip.length / 2;
            DOVirtual.DelayedCall(halfAnimTime, OnHalfAnim)
                .SetEase(Ease.Linear)
                .OnComplete(() => DOVirtual.DelayedCall(halfAnimTime, Close)
                    .SetEase(Ease.Linear));
        } else Close();
    }
    
    private static void OnHalfAnim()
    {
        CameraFollower.Ins.targetTf = LevelManager.Ins.player.Tf;
    }
    
    private void TransparentImage(float alpha)
    {
        Color stageImageColor = panel.color;
        stageImageColor.a = alpha;
        panel.color = stageImageColor;
    }
}
