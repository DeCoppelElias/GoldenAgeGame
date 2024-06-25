using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPaintPowerup : Powerup
{
    private LevelManager levelManager;
    private int rockValueIncrease;
    public DiamondPaintPowerup(LevelManager levelManager, int rockValueIncrease)
    {
        this.levelManager = levelManager;
        this.rockValueIncrease = rockValueIncrease;
    }
    public override void ActivatePowerup()
    {
        this.levelManager.ChangeRockValue(rockValueIncrease);
    }

    public override void DisablePowerup()
    {
        this.levelManager.ChangeRockValue(-rockValueIncrease);
    }
}
