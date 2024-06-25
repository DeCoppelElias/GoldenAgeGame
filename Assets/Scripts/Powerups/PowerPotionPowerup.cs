using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPotionPowerup : Powerup
{
    private PlayerManager playerManager;
    private float strengthIncrease;
    public PowerPotionPowerup(PlayerManager playerManager, float strengthIncrease)
    {
        this.playerManager = playerManager;
        this.strengthIncrease = strengthIncrease;
    }
    public override void ActivatePowerup()
    {
        foreach (Player player in playerManager.players)
        {
            player.ChangeGrappleShootSpeed(this.strengthIncrease);
        }
    }

    public override void DisablePowerup()
    {
        foreach (Player player in playerManager.players)
        {
            player.ChangeGrappleShootSpeed(-this.strengthIncrease);
        }
    }
}
