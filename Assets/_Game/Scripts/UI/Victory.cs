using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : UICanvas
{
    public void RetryButton()
    {
        LevelManager.Ins.OnRetry();
        Close();
    }

    public void NextButton()
    {
        LevelManager.Ins.OnNextLevel();
        Close();
    }
}
