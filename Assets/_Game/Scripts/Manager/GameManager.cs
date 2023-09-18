using UnityEngine;

public enum GameState
{
    MainMenu,
    InGame,
    Pause
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameState gameState;

    private void Awake()
    {
        // Input.multiTouchEnabled = false;
        // Application.targetFrameRate = 60;
        // Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // const int maxScreenHeight = 1280;
        // float ratio = Screen.currentResolution.width / (float)Screen.currentResolution.height;
        // if (Screen.currentResolution.height > maxScreenHeight)
        // {
        //     Screen.SetResolution(Mathf.RoundToInt(ratio * maxScreenHeight), maxScreenHeight, true);
        // }
    }

    public void ChangeState(GameState gameStateI)
    {
        gameState = gameStateI;
    }

    public bool IsState(GameState gameStateI)
    {
        return gameState == gameStateI;
    }
}