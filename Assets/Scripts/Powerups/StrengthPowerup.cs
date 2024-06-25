using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerup : Powerup
{
    private PlayerManager playerManager;
    private float strengthIncrease;
    public StrengthPowerup(PlayerManager playerManager, float strengthIncrease)
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
