using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGrapple : Grapple
{
    public GameObject dynamitePrefab;
    public enum GrappleState { Idle, Shot, Returning, GrabbingPickup, HelpingOtherGrapple }
    public GrappleState grappleState = GrappleState.Idle;

    public List<Pickup> grabbedPickups = new List<Pickup>();

    public List<Grapple> helpingGrapples = new List<Grapple>();

    public NormalGrapple normalGrapple;

    protected override void CustomStart()
    {
        base.CustomStart();
        ShootGrapple();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, GetRotateAroundPosition());
        lineRenderer.SetPosition(1, this.transform.position);

        // If grapple is shot, shoot in initial direction
        if (this.grappleState == GrappleState.Shot)
        {
            this.transform.position = this.transform.position + (this.grappleShootSpeed * Time.deltaTime * this.shootDirection);
            if (Vector3.Distance(this.transform.position, this.player.transform.position) > this.grappleMaxDistance)
            {
                this.grappleState = GrappleState.Returning;
            }
        }

        // If grapple is grabbing a pickup, return grapple to initial point with speed reduction
        if (this.grappleState == GrappleState.GrabbingPickup)
        {
            ReturnGrapple();
            if (Vector3.Distance(this.transform.position, GetInitialShotPoint()) < 0.1f)
            {
                this.transform.position = GetInitialShotPoint();
                GetPickups();
                DeleteGrapple();
            }
        }

        // If grapple is returning, return grapple to initial point
        if (this.grappleState == GrappleState.Returning)
        {
            ReturnGrapple();
            if (Vector3.Distance(this.transform.position, GetInitialShotPoint()) < 0.1f)
            {
                this.transform.position = GetInitialShotPoint();
                DeleteGrapple();
            }
        }
    }
    public override void ShootGrapple()
    {
        if (this.grappleState == GrappleState.Idle)
        {
            this.grappleState = GrappleState.Shot;
            this.shootDirection = Vector3.Normalize(this.transform.position - GetRotateAroundPosition());
        }
    }

    private void DeleteGrapple()
    {
        this.normalGrapple.DeleteMiniGrapple(this);
    }

    public void IncreaseLaserSightLength(float amount)
    {
        laserSightLength += amount;
    }

    public void ThrowDynamite()
    {
        if (this.grappleState == GrappleState.GrabbingPickup && this.playerManager.dynamite > 0)
        {
            this.playerManager.RemoveDynamite();
            this.grappleState = GrappleState.Returning;

            Dynamite dynamite = Instantiate(dynamitePrefab, GetInitialShotPoint(), Quaternion.identity, GameObject.Find("Objects").transform).GetComponent<Dynamite>();
            dynamite.target = this;
        }
    }

    public override void OnDynamite()
    {
        foreach (Pickup pickup in new List<Pickup>(this.grabbedPickups))
        {
            pickup.OnDynamite(this);
        }
    }

    public override void RemovePickup(Pickup pickup)
    {
        this.grabbedPickups.Remove(pickup);
    }

    public override void ResetGrapple()
    {
        DeleteGrapple();
    }

    private void GetPickups()
    {
        foreach (Grapple helpingGrapple in this.helpingGrapples)
        {
            helpingGrapple.OnHelpingDone();
        }
        this.helpingGrapples.Clear();

        foreach (Pickup p in this.grabbedPickups)
        {
            if (p != null)
            {
                p.PickupEffect(this.playerManager);
                Destroy(p.gameObject);
            }
        }
        this.grabbedPickups = new List<Pickup>();
    }

    private void ReturnGrapple()
    {
        float maxWeightWithHelpers = this.maxWeight;
        foreach (Grapple grapple in this.helpingGrapples)
        {
            maxWeightWithHelpers += grapple.maxWeight;
        }

        float totalWeight = CalculateTotalWeight();
        float percentage = Mathf.Clamp(1 - (totalWeight / maxWeightWithHelpers), 0.1f, 1f);
        this.transform.position = Vector3.MoveTowards(this.transform.position, GetInitialShotPoint(), 0.8f * this.grappleShootSpeed * percentage * Time.deltaTime);
    }

    public float CalculateTotalWeight()
    {
        float totalWeight = 0;
        foreach (Pickup pickup in this.grabbedPickups)
        {
            totalWeight += pickup.weight;
        }
        return totalWeight;
    }

    public override void OnGrabPickup(Pickup p)
    {
        if (grappleState == GrappleState.Shot)
        {
            p.transform.SetParent(this.transform);
            p.transform.position = this.transform.position + (0.25f * p.size * this.shootDirection);

            this.grabbedPickups.Add(p);
            this.grappleState = GrappleState.GrabbingPickup;
        }
    }

    public override void OnGrabEffect()
    {
        if (grappleState == GrappleState.Shot)
        {
            this.grappleState = GrappleState.Returning;
        }
    }

    public override void OnHelpingOtherGrapple(Grapple mainGrapple)
    {
        if (grappleState == GrappleState.Shot)
        {
            this.transform.SetParent(mainGrapple.transform);
            this.grappleState = GrappleState.HelpingOtherGrapple;
            mainGrapple.OnGettingHelpFromOtherGrapple(this);
        }
    }
    public override void OnGettingHelpFromOtherGrapple(Grapple otherGrapple)
    {
        this.helpingGrapples.Add(otherGrapple);
    }

    public override void OnHelpingDone()
    {
        this.transform.SetParent(this.player.transform);
        this.ResetGrapple();
    }
}
