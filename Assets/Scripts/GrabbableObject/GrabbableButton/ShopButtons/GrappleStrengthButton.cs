using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleStrengthButton : ShopButton
{
    protected override void ButtonEffect()
    {
        base.ButtonEffect();
        playerManager.ChangeGrappleShootSpeed(2f);
    }
}
