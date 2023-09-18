using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseStage : UICanvas
{
    public void MainMenuButton()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close();
    }
    
    
}
