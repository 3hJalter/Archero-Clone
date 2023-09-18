public class ChooseStage : UICanvas
{

    private void Awake()
    {
        OnInit();
    }

    private void OnEnable()
    {
        // TODO
    }

    private void OnInit()
    {
        
    }
    
    public void CloseButton()
    {
        UIManager.Ins.OpenUI<MainMenu>();
        Close();
    }
    
}
