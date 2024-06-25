using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grapple : MonoBehaviour
{
    protected Player player;
    protected bool clockwiseRotation = true;

    public Vector3 shootDirection;

    public float initialLaserSightLength = 0;
    public float initialMaxWeight = 10;
    public float initialGrappleRotateSpeed = 50;
    public float initialGrappleShootSpeed = 10;
    public float initialGrappleMaxDistance = 10;

    public float laserSightLength = 0;
    public float maxWeight = 10;
    public float grappleRotateSpeed = 50;
    public float grappleShootSpeed = 10;
    public float grappleMaxDistance = 10;

    protected GameStateManager gameStateManager;
    protected PlayerManager playerManager;

    protected LineRenderer lineRenderer;
    private void Start()
    {
        CustomStart();
    }
    protected virtual void CustomStart()
    {
        this.player = this.GetComponentInParent<Player>();
        this.lineRenderer = this.GetComponent<LineRenderer>();

        this.gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        ResetGrappleStats();
    }
    public abstract void OnGettingHelpFromOtherGrapple(Grapple grapple);

    public abstract void RemovePickup(Pickup p);

    public abstract void OnDynamite();

    public abstract void ResetGrapple();

    public abstract void ShootGrapple();

    public abstract void OnGrabPickup(Pickup p);

    public abstract void OnHelpingOtherGrapple(Grapple mainGrapple);

    public abstract void OnGrabEffect();

    public abstract void OnHelpingDone();

    protected Vector3 GetRotateAroundPosition()
    {
        return this.player.transform.position + new Vector3(0, -0.4f, 0);
    }

    protected Vector3 GetInitialPosition()
    {
        return this.player.transform.position + new Vector3(0, -0.9f, 0);
    }

    protected Vector3 GetInitialShotPoint()
    {
        float distance = Vector3.Distance(GetRotateAroundPosition(), GetInitialPosition());
        return GetRotateAroundPosition() + (distance * this.shootDirection);
    }
    public void ResetGrappleStats()
    {
        this.laserSightLength = this.initialLaserSightLength;
        this.maxWeight = this.initialMaxWeight;
        this.grappleRotateSpeed = this.initialGrappleRotateSpeed;
        this.grappleShootSpeed = this.initialGrappleShootSpeed;
        this.grappleMaxDistance = this.initialGrappleMaxDistance;
    }
}
