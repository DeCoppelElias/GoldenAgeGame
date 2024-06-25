using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoldIncreaseButton : ShopButton
{
    private LevelManager levelManager;
    protected override void CustomStart()
    {
        base.CustomStart();
        this.levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    protected override void ButtonEffect()
    {
        base.ButtonEffect();
        levelManager.currentMinLevelValue += 50;
        levelManager.currentMaxLevelValue += 50;
    }
}
