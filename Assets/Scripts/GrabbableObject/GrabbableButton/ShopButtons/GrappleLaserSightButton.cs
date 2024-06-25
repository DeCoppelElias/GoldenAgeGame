using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLaserSightButton : ShopButton
{
    protected override void ButtonEffect()
    {
        base.ButtonEffect();
        playerManager.IncreaseLaserSightLength(5);
    }
}
