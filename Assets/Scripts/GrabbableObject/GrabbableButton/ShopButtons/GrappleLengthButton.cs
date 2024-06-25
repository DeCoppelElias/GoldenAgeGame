using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLengthButton : ShopButton
{
    protected override void ButtonEffect()
    {
        base.ButtonEffect();
        playerManager.IncreaseGrappleLength(2);
    }
}
