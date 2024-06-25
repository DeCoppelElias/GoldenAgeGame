using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkPowerPotionButton : ShopButton
{
    private Powerup powerPotionPowerup;
    protected override void CustomStart()
    {
        base.CustomStart();
        this.powerPotionPowerup = new PowerPotionPowerup(playerManager, 2f);
    }
    protected override bool ButtonCondition()
    {
        return base.ButtonCondition() && !this.playerManager.PowerPotion();
    }
    protected override void ButtonEffect()
    {
        base.ButtonEffect();
        playerManager.AddTemporaryPowerup(this.powerPotionPowerup);
    }
}
