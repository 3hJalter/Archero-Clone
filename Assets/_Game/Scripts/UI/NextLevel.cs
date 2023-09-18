using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NextLevel : UICanvas
{
    [SerializeField] private Image panel;

    public override void Close()
    {
        GameManager.Ins.ChangeState(GameState.InGame);
        base.Close();
    }

    private void OnEnable()
    {   
        GameManager.Ins.ChangeState(GameState.Pause);
        DOVirtual.Float(0,1, 0.5f, TransparentImage).OnComplete(
            () =>
            {
                LevelManager.Ins.OnNextLevel();
                DOVirtual.Float(1, 0, 0.5f, TransparentImage).OnComplete(
                    Close);
            });
    }
    
    private void TransparentImage(float alpha)
    {
        Color stageImageColor = panel.color;
        stageImageColor.a = alpha;
        panel.color = stageImageColor;
    }
}
