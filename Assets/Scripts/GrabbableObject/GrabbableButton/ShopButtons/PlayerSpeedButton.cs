using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedButton : ShopButton
{
    protected override bool ButtonCondition()
    {
        return base.ButtonCondition() && this.playerManager.GetPlayerSpeed() <= 8;
    }
    protected override void ButtonEffect()
    {
        base.ButtonEffect();
        this.playerManager.IncreaseSpeed(1);
    }
}
