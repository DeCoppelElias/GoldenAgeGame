using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AllGrabbableButton : GrabbableButton
{
    protected PlayerManager playerManager;
    protected float startTime = 0;

    void Start()
    {
        this.slider = this.transform.Find("Slider");
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        this.CustomStart();
    }

    protected virtual void CustomStart()
    {

    }

    protected override void OnTriggerEnter2DCustom(Grapple grapple)
    {
        base.OnTriggerEnter2DCustom(grapple);
        this.startTime = Time.time;
    }
    public override void ButtonUpdate()
    {
        if (this.playerManager.players.Count == this.grapples.Count && this.playerManager.players.Count > 0)
        {
            if (ButtonCondition())
            {
                float currentDuration = Time.time - startTime;
                if (currentDuration >= duration)
                {
                    this.ButtonEffect();
                    foreach (NormalGrapple grapple in new List<NormalGrapple>(this.grapples))
                    {
                        grapple.ResetGrapple();
                        ReleaseButton(grapple);
                    }
                }
                else
                {
                    slider.localScale = new Vector3(currentDuration / duration, 1, 1);
                }
            }
            else
            {
                foreach (NormalGrapple grapple in new List<NormalGrapple>(this.grapples))
                {
                    grapple.ReleaseGrapple();
                    ReleaseButton(grapple);
                }
            }
        }
    }
}
