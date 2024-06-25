using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : AllGrabbableButton
{
    public GameStateManager gameStateManager;
    public float startGrabTime;

    void Start()
    {
        this.slider = this.transform.Find("Slider");
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        this.gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
    }
    protected override void ButtonEffect()
    {
        gameStateManager.EnterGameState(GameStateManager.GameState.Level);
    }
}
