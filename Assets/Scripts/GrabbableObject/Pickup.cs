using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : GrabbableObject
{
    public float weight = 1;
    public int value;
    public Grapple activeGrapple;

    protected override void OnTriggerEnter2DCustom(Grapple grapple)
    {
        if (this.activeGrapple == null || this.activeGrapple == grapple)
        {
            this.activeGrapple = grapple;
            grapple.OnGrabPickup(this);
        }
        else
        {
            grapple.OnHelpingOtherGrapple(this.activeGrapple);
        }
    }

    public void PickupEffect(PlayerManager playerManager)
    {
        playerManager.AddGold(value);
    }

    public void OnDynamite(Grapple grapple)
    {
        grapple.RemovePickup(this);
        Destroy(this.gameObject);
    }
}
