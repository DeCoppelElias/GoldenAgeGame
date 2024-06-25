using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class ShopButton : AllGrabbableButton
{
    public int price = 0;
    public int priceIncrease = 0;
    private TextMeshProUGUI goldAmountText;
    protected override void CustomStart()
    {
        base.CustomStart();

        int upgradeAmount = this.playerManager.GetUpgradeAmount(this.name);
        this.price += priceIncrease * upgradeAmount;

        goldAmountText = this.transform.Find("GoldInfo").transform.Find("GoldInfoText").GetComponent<TextMeshProUGUI>();
        goldAmountText.text = price.ToString();
    }
    protected override bool ButtonCondition()
    {
        return base.ButtonCondition() && this.playerManager.totalGold >= price;
    }

    protected override void ButtonEffect()
    {
        playerManager.RemoveGold(price);
        price += priceIncrease;
        goldAmountText.text = price.ToString();
        playerManager.AddUpgrade(this.name);
    }
}
