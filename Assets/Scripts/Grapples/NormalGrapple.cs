using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGrapple : Grapple
{
    public GameObject dynamitePrefab;
    public enum GrappleState { Rotating, Shot, Returning, GrabbingPickup, GrabbingButton, HelpingOtherGrapple }
    public GrappleState grappleState = GrappleState.Rotating;

    public List<Pickup> grabbedPickups = new List<Pickup>();
    public List<Grapple> helpingGrapples = new List<Grapple>();

    protected GrabbableButton grabbedButton;
    protected float buttonGrabStartTime;

    protected LineRenderer laserSightRenderer;

    public GameObject miniGrapplePrefab;
    public List<MiniGrapple> miniGrapples = new List<MiniGrapple>();

    protected float currentAngle;

    protected override void CustomStart()
    {
        base.CustomStart();

        currentAngle = 0f;
        this.laserSightRenderer = this.transform.Find("LaserSight").GetComponent<LineRenderer>();
        this.laserSightRenderer.startWidth = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, GetRotateAroundPosition());
        lineRenderer.SetPosition(1, this.transform.position);

        laserSightRenderer.positionCount = 0;

        // if grapple is rotating, move around player gameobject
        if (this.grappleState == GrappleState.Rotating)
        {
            float totalDistance = Vector3.Distance(this.transform.position, GetRotateAroundPosition());
            float distance = Mathf.Abs(this.transform.position.y - GetRotateAroundPosition().y);
            float percentage = distance / totalDistance;
            if (percentage <= 0.5)
            {
                if (this.transform.position.x < GetRotateAroundPosition().x)
                {
                    this.clockwiseRotation = false;
                }
                else
                {
                    this.clockwiseRotation = true;
                }
            }
            RotateGrapple();

            if (laserSightLength > 0)
            {
                Vector3 direction = Vector3.Normalize(this.transform.position - GetRotateAroundPosition());
                laserSightRenderer.positionCount = 2;
                laserSightRenderer.SetPosition(0, this.transform.position);
                laserSightRenderer.SetPosition(1, this.transform.position + (this.laserSightLength * direction));
            }
        }

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
                if (miniGrapples.Count == 0)
                {
                    this.grappleState = GrappleState.Rotating;
                }
            }
        }

        // If grapple is returning, return grapple to initial point
        if (this.grappleState == GrappleState.Returning)
        {
            ReturnGrapple();
            if (Vector3.Distance(this.transform.position, GetInitialShotPoint()) < 0.1f)
            {
                this.transform.position = GetInitialShotPoint();
                if (miniGrapples.Count == 0)
                {
                    this.grappleState = GrappleState.Rotating;
                }
            }
        }
    }

    public void IncreaseLaserSightLength(float amount)
    {
        laserSightLength += amount;
    }

    public override void ShootGrapple()
    {
        if (this.grappleState == GrappleState.Rotating)
        {
            this.grappleState = GrappleState.Shot;
            this.shootDirection = Vector3.Normalize(this.transform.position - GetRotateAroundPosition());
            SpawnAndShootMiniGrapples();
        }
    }
    private void SpawnAndShootMiniGrapples()
    {
        SpawnMiniGrapple(25);
        SpawnMiniGrapple(-25);
    }

    private void SpawnMiniGrapple(float angle)
    {
        GameObject miniGrapple = Instantiate(this.miniGrapplePrefab, this.GetInitialPosition(), Quaternion.identity, this.player.transform);
        miniGrapple.transform.RotateAround(GetRotateAroundPosition(), Vector3.forward, this.currentAngle + angle);

        MiniGrapple grapple = miniGrapple.GetComponent<MiniGrapple>();
        grapple.normalGrapple = this;

        grapple.initialGrappleMaxDistance = 0.8f * this.initialGrappleMaxDistance;

        miniGrapples.Add(grapple);
    }

    public void DeleteMiniGrapple(MiniGrapple miniGrapple)
    {
        this.miniGrapples.Remove(miniGrapple);
        Destroy(miniGrapple.gameObject);
    }

    public void ReleaseGrapple()
    {
        if (this.grappleState == GrappleState.GrabbingButton)
        {
            this.grappleState = GrappleState.Returning;
            this.grabbedButton.ReleaseButton(this);
        }
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
        this.grappleState = GrappleState.Rotating;
        this.transform.position = GetInitialPosition();
        this.transform.rotation = Quaternion.identity;
        this.grabbedPickups = new List<Pickup>();

        this.currentAngle = 0;

        this.helpingGrapples.Clear();
        this.transform.SetParent(this.player.transform);
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

    private void RotateGrapple()
    {
        if (clockwiseRotation)
        {
            this.transform.RotateAround(GetRotateAroundPosition(), Vector3.forward, -this.grappleRotateSpeed * Time.deltaTime);
            this.currentAngle += -this.grappleRotateSpeed * Time.deltaTime;
        }
        else
        {
            this.transform.RotateAround(GetRotateAroundPosition(), Vector3.forward, this.grappleRotateSpeed * Time.deltaTime);
            this.currentAngle += this.grappleRotateSpeed * Time.deltaTime;
        }
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
        this.ResetGrapple();
    }

    public void OnGrabButton(GrabbableButton button)
    {
        this.grabbedButton = button;
        this.grappleState = GrappleState.GrabbingButton;
        this.buttonGrabStartTime = Time.time;
        
        this.transform.position += (0.5f * this.shootDirection);
    }
}
