using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPaintButton : ShopButton
{
    public int stoneGoldIncrease = 9;
    private Powerup diamondPaintpowerup;
    protected override void CustomStart()
    {
        base.CustomStart();
        this.diamondPaintpowerup = new DiamondPaintPowerup(LevelManager.GetInstance(), stoneGoldIncrease);
    }
    protected override bool ButtonCondition()
    {
        return base.ButtonCondition() && !this.playerManager.DiamondPaint();
    }
    protected override void ButtonEffect()
    {
        base.ButtonEffect();
        playerManager.AddTemporaryPowerup(this.diamondPaintpowerup);
    }
}
