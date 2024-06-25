using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyDynamiteButton : AllGrabbableButton
{
    public int price = 100;

    protected override bool ButtonCondition()
    {
        return playerManager.totalGold >= price;
    }
    protected override void ButtonEffect()
    {
        if (playerManager.totalGold >= price)
        {
            playerManager.RemoveGold(price);
            playerManager.AddDynamite();
        }
    }
}
