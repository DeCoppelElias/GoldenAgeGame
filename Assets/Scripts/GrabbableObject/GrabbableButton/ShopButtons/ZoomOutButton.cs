using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOutButton : ShopButton
{
    public CameraManager cameraManager;
    private LevelManager levelManager;

    protected override void CustomStart()
    {
        base.CustomStart();
        this.cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        this.levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    protected override bool ButtonCondition()
    {
        return base.ButtonCondition() && cameraManager.cameraSizeLevel < 20;
    }
    protected override void ButtonEffect()
    {
        base.ButtonEffect();
        this.cameraManager.ZoomOut(1);
        levelManager.currentMinLevelValue += 50;
        levelManager.currentMaxLevelValue += 50;
        levelManager.levelDuration += 5;
    }
}
