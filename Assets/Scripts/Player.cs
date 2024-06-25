using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private GameStateManager gameStateManager;
    private PlayerManager playerManager;
    private NormalGrapple grapple;

    private int initialMoveSpeed = 2;
    public int defaultMoveSpeed = 2;
    public int shopMoveSpeed = 4;

    public Vector2 direction = new Vector2(0, 0);
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        this.grapple = this.GetComponentInChildren<NormalGrapple>();
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        this.gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        this.transform.SetParent(GameObject.Find("Players").transform);
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        (float, float) boundsTuple = this.playerManager.GetBounds();
        int moveSpeed = this.defaultMoveSpeed;
        if (!this.gameStateManager.InLevel())
        {
            moveSpeed = this.shopMoveSpeed;
        }
        if (direction.x > 0)
        {
            if (grapple.grappleState == NormalGrapple.GrappleState.Rotating && this.transform.position.x < boundsTuple.Item2)
            {
                this.transform.position += moveSpeed * Time.deltaTime * new Vector3(direction.x, 0, 0);
            }
        }
        else
        {
            if (grapple.grappleState == NormalGrapple.GrappleState.Rotating && this.transform.position.x > boundsTuple.Item1)
            {
                this.transform.position += moveSpeed * Time.deltaTime * new Vector3(direction.x, 0, 0);
            }
        }
    }

    public void IncreaseSpeed(int speed)
    {
        this.defaultMoveSpeed += speed;
    }

    public void IncreaseGrappleMaxLength(float amount)
    {
        this.grapple.grappleMaxDistance += amount;
    }

    public void ChangeGrappleShootSpeed(float amount)
    {
        this.grapple.grappleShootSpeed += amount;
    }

    public void IncreaseLaserSightLength(float amount)
    {
        this.grapple.IncreaseLaserSightLength(amount);
    }

    public void ResetPosition()
    {
        (float, float) boundsTuple = this.playerManager.GetBounds();
        this.transform.position = new Vector3((boundsTuple.Item1 + boundsTuple.Item2) / 2, 2.9f, 0);
    }

    public void Move(InputAction.CallbackContext context)
    {
        this.direction = context.ReadValue<Vector2>();
    }

    public void ShootGrapple(InputAction.CallbackContext context)
    {
        if (context.performed && this.grapple != null)
        {
            this.grapple.ShootGrapple();
        }
    }

    public void ReleaseGrapple(InputAction.CallbackContext context)
    {
        if (context.performed && this.grapple != null)
        {
            this.grapple.ReleaseGrapple();
        }
    }

    public void ThrowDynamite(InputAction.CallbackContext context)
    {
        if (context.performed && this.grapple != null)
        {
            this.grapple.ThrowDynamite();
        }
    }

    public void ResetGrapple()
    {
        this.grapple.ResetGrapple();
    }

    public void DrinkPowerPotion()
    {
        this.grapple.maxWeight *= 2;
    }

    public void PowerPotionWearsOff()
    {
        this.grapple.maxWeight /= 2;
    }

    public void ResetPlayerStats()
    {
        this.defaultMoveSpeed = initialMoveSpeed;
        this.grapple.ResetGrappleStats();
    }
}
