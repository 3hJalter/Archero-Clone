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

    private void Start()
    {
        // ChangeState(GameState.MainMenu);
        // TEST
        // ChangeState(GameState.InGame);
    }

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return this.gameState == gameState;
    }
}