using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : AllGrabbableButton
{
    private GameStateManager gameStateManager;
    protected override void ButtonEffect()
    {
        this.gameStateManager.EnterGameState(GameStateManager.GameState.MainMenu);
    }

    protected override void CustomStart()
    {
        this.gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
    }
}
