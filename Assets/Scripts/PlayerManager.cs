using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Manager
{
    private GameStateManager gameStateManager;
    private UIManager uiManager;
    private PlayerInputManager playerInputManager;
    
    public int totalGold = 0;
    public int levelGold = 0;
    public int dynamite = 0;

    public List<Player> players = new List<Player>();
    public List<Color> colors = new List<Color>();

    public Dictionary<string, int> playerUpgrades = new Dictionary<string, int>();
    public Dictionary<Powerup, int> temporaryPowerups = new Dictionary<Powerup, int>();

    public int GetUpgradeAmount(string upgradeName)
    {
        if (playerUpgrades.ContainsKey(upgradeName))
        {
            return playerUpgrades[upgradeName];
        }
        else
        {
            return 0;
        }
    }
    public void AddUpgrade(string upgradeName)
    {
        if (playerUpgrades.ContainsKey(upgradeName))
        {
            playerUpgrades[upgradeName] += 1;
        }
        else
        {
            playerUpgrades.Add(upgradeName, 1);
        }
    }

    public void AddTemporaryPowerup(Powerup powerup)
    {
        powerup.ActivatePowerup();
        if (this.temporaryPowerups.ContainsKey(powerup))
        {
            this.temporaryPowerups[powerup] += 1;
        }
        else
        {
            this.temporaryPowerups.Add(powerup, 1);
        }

        this.UpdateUI();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Player player = playerInput.GetComponent<Player>();
        AddPlayer(player);
    }

    public int GetPlayerSpeed()
    {
        return this.players[0].defaultMoveSpeed;
    }

    public void IncreaseSpeed(int amount)
    {
        foreach (Player player in this.players)
        {
            player.IncreaseSpeed(amount);
        }
    }

    public void RemovePlayer(Player player)
    {
        this.players.Remove(player);
        Destroy(player.gameObject);

        if (this.players.Count == 0)
        {
            gameStateManager.EnterGameState(GameStateManager.GameState.MainMenu);
        }
    }

    public override void OnInitialStart()
    {
        this.uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        this.playerInputManager = this.GetComponent<PlayerInputManager>();
        this.colors.Add(Color.red);
        this.colors.Add(Color.green);
        this.colors.Add(Color.yellow);
        this.colors.Add(Color.blue);
        UpdateUI();
    }
    public (float, float) GetBounds()
    {
        return (Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x, Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).x);
    }

    public void AddPlayer(Player player)
    {
        player.index = this.players.Count;
        this.players.Add(player);
        this.uiManager.AddPlayer(player);
        this.UpdateUI();

        if (this.players.Count > 1 && this.colors.Count > 0)
        {
            int r = Random.Range(0, this.colors.Count);
            player.GetComponent<SpriteRenderer>().color = this.colors[r];
            this.colors.RemoveAt(r);
        }
    }

    public void Travel()
    {
        foreach (Player player in this.players)
        {
            player.ResetPosition();
            player.ResetGrapple();
        }
    }

    public void IncreaseGrappleLength(float amount)
    {
        foreach (Player player in this.players)
        {
            player.IncreaseGrappleMaxLength(amount);
        }
    }

    public void ChangeGrappleShootSpeed(float amount)
    {
        foreach (Player player in this.players)
        {
            player.ChangeGrappleShootSpeed(amount);
        }
    }

    public void IncreaseLaserSightLength(float amount)
    {
        foreach (Player player in this.players)
        {
            player.IncreaseLaserSightLength(amount);
        }
    }

    public override void OnLevelStart()
    {
        this.levelGold = 0;
        foreach (Player player in this.players)
        {
            player.ResetGrapple();
        }
    }
    public override void OnLevelEnd()
    {
        // Resetting player
        foreach (Player player in this.players)
        {
            player.ResetGrapple();
            player.ResetPosition();
        }

        // Resetting temporary powerups
        foreach (Powerup powerup in this.temporaryPowerups.Keys)
        {
            int amount = this.temporaryPowerups[powerup];
            for (int i = 0; i < amount; i++)
            {
                powerup.DisablePowerup();
            }
        }
        this.temporaryPowerups.Clear();

        UpdateUI();
    }

    public override void OnMainMenuStart()
    {
        this.levelGold = 0;
        this.totalGold = 0;
        this.dynamite = 0;

        playerUpgrades = new Dictionary<string, int>();

        // Resetting player stats
        foreach (Player player in this.players)
        {
            player.ResetPlayerStats();
        }

        // Resetting temporary powerups
        foreach (Powerup powerup in this.temporaryPowerups.Keys)
        {
            int amount = this.temporaryPowerups[powerup];
            for (int i = 0; i < amount; i++)
            {
                powerup.DisablePowerup();
            }
        }
        this.temporaryPowerups.Clear();

        // Enable joining
        this.playerInputManager.EnableJoining();

        UpdateUI();
    }

    public override void OnMainMenuEnd()
    {
        base.OnMainMenuEnd();
        this.playerInputManager.DisableJoining();
    }
    public void AddGold(int gold)
    {
        this.totalGold += gold;
        this.levelGold += gold;
        UpdateUI();
    }

    public void RemoveGold(int gold)
    {
        this.totalGold -= gold;
        UpdateUI();
    }

    public void AddDynamite()
    {
        this.dynamite += 1;
        UpdateUI();
    }

    public void RemoveDynamite()
    {
        this.dynamite -= 1;
        UpdateUI();
    }

    private void UpdateUI()
    {
        this.uiManager.UpdateDynamite(this.dynamite);
        this.uiManager.UpdateTotalGold(this.totalGold);

        bool diamondPaint = DiamondPaint();
        this.uiManager.UpdateDiamondPaint(diamondPaint);

        bool powerPotion = PowerPotion();
        this.uiManager.UpdatePowerPotion(powerPotion);
    }

    public bool DiamondPaint()
    {
        bool diamondPaint = false;
        foreach (Powerup powerup in this.temporaryPowerups.Keys)
        {
            if (powerup is DiamondPaintPowerup)
            {
                diamondPaint = true;
            }
        }
        return diamondPaint;
    }

    public bool PowerPotion()
    {
        bool powerPotion = false;
        foreach (Powerup powerup in this.temporaryPowerups.Keys)
        {
            if (powerup is PowerPotionPowerup)
            {
                powerPotion = true;
            }
        }
        return powerPotion;
    }

    public List<Vector3> GetAllPlayerPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (Player player in this.players)
        {
            positions.Add(player.transform.position);
        }
        return positions;
    }

    public static PlayerManager GetInstance()
    {
        return GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }
}
