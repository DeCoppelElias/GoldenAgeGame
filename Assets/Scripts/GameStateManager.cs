using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public List<Manager> managers = new List<Manager>();

    public enum GameState { Initial, MainMenu, Level, BetweenLevel, GameOver}
    public GameState gameState = GameState.Initial;

    private void Start()
    {
        foreach (Manager manager in managers)
        {
            manager.OnInitialStart();
        }
        Invoke("StartGame", 6);
    }

    private void StartGame()
    {
        EnterGameState(GameState.MainMenu);
    }

    public void EnterGameState(GameState nextGameState)
    {
        ExitGameState(this.gameState);
        this.gameState = nextGameState;

        if (nextGameState == GameState.MainMenu)
        {
            foreach (Manager manager in managers)
            {
                manager.OnMainMenuStart();
            }
        }
        else if (nextGameState == GameState.Level)
        {
            foreach (Manager manager in managers)
            {
                manager.OnLevelStart();
            }
        }
        else if (nextGameState == GameState.BetweenLevel)
        {
            foreach (Manager manager in managers)
            {
                manager.OnBetweenLevelStart();
            }
        }
        else if (nextGameState == GameState.GameOver)
        {
            foreach (Manager manager in managers)
            {
                manager.OnGameOver();
            }
            EnterGameState(GameState.MainMenu);
        }
    }

    private void ExitGameState(GameState currentGameState)
    {
        if (currentGameState == GameState.Initial)
        {
            foreach (Manager manager in managers)
            {
                manager.OnInitialEnd();
            }
        }
        if (currentGameState == GameState.MainMenu)
        {
            foreach (Manager manager in managers)
            {
                manager.OnMainMenuEnd();
            }
        }
        else if (currentGameState == GameState.Level)
        {
            foreach (Manager manager in managers)
            {
                manager.OnLevelEnd();
            }
        }
        else if (currentGameState == GameState.BetweenLevel)
        {
            foreach (Manager manager in managers)
            {
                manager.OnBetweenLevelEnd();
            }
        }
    }

    public bool InLevel()
    {
        return this.gameState == GameState.Level;
    }

    public bool InBetweenLevel()
    {
        return this.gameState == GameState.BetweenLevel;
    }

    public bool InMainMenu()
    {
        return this.gameState == GameState.MainMenu;
    }
}
