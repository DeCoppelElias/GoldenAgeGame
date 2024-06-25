using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Manager
{
    public LevelManager levelManager;
    public PlayerManager playerManager;

    public GameObject introMenu;
    public GameObject introGameTitle;
    public GameObject introNameReveal;
    public GameObject introGameBy;
    public GameObject levelCompletedMenu;
    public GameObject tutorialMenu;
    public GameObject shopMenu;
    public GameObject quotaInfo;
    public GameObject totalGoldInfo;

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI levelGoldText;
    public TextMeshProUGUI newQuotaText;
    public TextMeshProUGUI totalGold;
    public TextMeshProUGUI quota;
    public TextMeshProUGUI timeRemaining;
    public TextMeshProUGUI highScore;

    public List<TextMeshProUGUI> dynamiteTexts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> powerPotionTexts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> diamondPaintTexts = new List<TextMeshProUGUI>();

    public List<GameObject> shopPrefabs = new List<GameObject>();
    public List<GameObject> upgradePrefabs = new List<GameObject>();
    public List<GameObject> betweenLevelObjects = new List<GameObject>();
    public Transform shopButtonParent;
    public Transform upgradeButtonParent;

    public override void OnInitialStart()
    {
        this.introMenu.SetActive(true);
        this.introNameReveal.SetActive(false);
        this.introGameBy.SetActive(false);
        this.introGameTitle.SetActive(true);
        Invoke("NameRevealPart1", 2);

        this.levelCompletedMenu.SetActive(false);
        this.tutorialMenu.SetActive(false);
        this.quotaInfo.SetActive(false);
        this.totalGoldInfo.SetActive(false);
    }

    public override void OnInitialEnd()
    {
        this.introMenu.SetActive(false);
        this.totalGoldInfo.SetActive(true);
    }

    private void NameRevealPart1()
    {
        this.introGameTitle.SetActive(false);
        this.introGameBy.SetActive(true);
        Invoke("NameRevealPart2", 1);
    }
    private void NameRevealPart2()
    {
        this.introNameReveal.SetActive(true);
    }

    public override void OnGameOver()
    {
        this.UpdateQuota(0);
    }
    public override void OnLevelStart()
    {
        this.EnableQuotaInfo(true);
    }

    public override void OnLevelEnd()
    {
        this.EnableQuotaInfo(false);
    }

    public override void OnBetweenLevelStart()
    {
        this.EnableLevelCompletedMenu(levelManager.day-1, playerManager.levelGold, levelManager.currentQuota);
        InstantiateRandomShop();
        InstantiateRandomUpgrades();
    }

    public override void OnBetweenLevelEnd()
    {
        this.DisableLevelCompleteMenu();
        RemoveObjects();
    }

    private void RemoveObjects()
    {
        foreach (GameObject o in this.betweenLevelObjects)
        {
            Destroy(o);
        }
        this.betweenLevelObjects = new List<GameObject>();
    }

    private List<GameObject> SelectFromList(List<GameObject> list, int amount)
    {
        List<GameObject> copy = new List<GameObject>(list);
        List<GameObject> chosen = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            int r = Random.Range(0, copy.Count);
            chosen.Add(copy[r]);
            copy.RemoveAt(r);
        }
        return chosen;
    }

    private void InstantiateRandomShop()
    {
        List<GameObject> chosenPrefabs = SelectFromList(this.shopPrefabs, 3);

        GameObject o1 = Instantiate(chosenPrefabs[0], shopButtonParent);
        o1.transform.localPosition = new Vector3(-100, -10, 0);
        this.betweenLevelObjects.Add(o1);

        GameObject o2 = Instantiate(chosenPrefabs[1], shopButtonParent);
        o2.transform.localPosition = new Vector3(0, -10, 0);
        this.betweenLevelObjects.Add(o2);

        GameObject o3 = Instantiate(chosenPrefabs[2], shopButtonParent);
        o3.transform.localPosition = new Vector3(100, -10, 0);
        this.betweenLevelObjects.Add(o3);
    }

    private void InstantiateRandomUpgrades()
    {
        List<GameObject> chosenPrefabs = SelectFromList(this.upgradePrefabs, 3);

        GameObject o1 = Instantiate(chosenPrefabs[0], upgradeButtonParent);
        o1.transform.localPosition = new Vector3(-100, -10, 0);
        this.betweenLevelObjects.Add(o1);

        GameObject o2 = Instantiate(chosenPrefabs[1], upgradeButtonParent);
        o2.transform.localPosition = new Vector3(0, -10, 0);
        this.betweenLevelObjects.Add(o2);

        GameObject o3 = Instantiate(chosenPrefabs[2], upgradeButtonParent);
        o3.transform.localPosition = new Vector3(100, -10, 0);
        this.betweenLevelObjects.Add(o3);
    }

    public override void OnMainMenuStart()
    {
        this.EnableTutorialMenu(true);
    }

    public override void OnMainMenuEnd()
    {
        this.EnableTutorialMenu(false);
    }

    public void UpdateTotalGold(int gold)
    {
        this.totalGold.text = gold.ToString();
    }

    public void UpdateQuota(int gold)
    {
        this.quota.text = gold.ToString();
    }

    public void UpdateTimeRemaining(int seconds)
    {
        this.timeRemaining.text = seconds.ToString();
    }

    public void EnableLevelCompletedMenu(int day, int levelGold, int newQuota)
    {
        this.dayText.text = "Completed Day " + day.ToString();
        this.levelGoldText.text = "Gold Earned: " + levelGold.ToString();
        this.newQuotaText.text = "New Quota: " + newQuota.ToString();
        this.levelCompletedMenu.SetActive(true);
    }

    public void DisableLevelCompleteMenu()
    {
        this.levelCompletedMenu.SetActive(false);
    }

    public void EnableQuotaInfo(bool enable)
    {
        quotaInfo.SetActive(enable);
    }

    public void EnableTutorialMenu(bool enable)
    {
        this.tutorialMenu.SetActive(enable);
    }

    public void EnableShopMenu(bool enable)
    {
        this.shopMenu.SetActive(enable);
    }

    public void UpdateDiamondPaint(bool enable)
    {
        foreach (TextMeshProUGUI diamondPaintText in this.diamondPaintTexts)
        {
            if (enable)
            {
                diamondPaintText.transform.parent.gameObject.SetActive(true);
                diamondPaintText.text = "Enabled!";
            }
            else
            {
                diamondPaintText.text = "Disabled!";
                diamondPaintText.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void UpdatePowerPotion(bool enable)
    {
        foreach (TextMeshProUGUI powerPotionText in this.powerPotionTexts)
        {
            if (enable)
            {
                powerPotionText.transform.parent.gameObject.SetActive(true);
                powerPotionText.text = "Enabled!";
            }
            else
            {
                powerPotionText.text = "Disabled!";
                powerPotionText.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateDynamite(int dynamite)
    {
        foreach (TextMeshProUGUI dynamiteText in this.dynamiteTexts)
        {
            if (dynamite > 0)
            {
                dynamiteText.transform.parent.gameObject.SetActive(true);
            }

            dynamiteText.text = dynamite.ToString();

            if (dynamite == 0)
            {
                dynamiteText.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHighScore(int highScore)
    {
        this.highScore.text = highScore.ToString() + " Days";
    }

    public void AddPlayer(Player player)
    {
        TextMeshProUGUI dynamiteText = player.transform.Find("Canvas").transform.Find("Panel").transform.Find("DynamiteInfo").transform.Find("DynamiteText").GetComponent<TextMeshProUGUI>();
        this.dynamiteTexts.Add(dynamiteText);
        TextMeshProUGUI powerPotionText = player.transform.Find("Canvas").transform.Find("Panel").transform.Find("PowerPotionInfo").transform.Find("PowerPotionText").GetComponent<TextMeshProUGUI>();
        this.powerPotionTexts.Add(powerPotionText);
        TextMeshProUGUI diamondPaintText = player.transform.Find("Canvas").transform.Find("Panel").transform.Find("DiamondPaintInfo").transform.Find("DiamondPaintText").GetComponent<TextMeshProUGUI>();
        this.diamondPaintTexts.Add(diamondPaintText);
        TextMeshProUGUI nameText = player.transform.Find("Canvas").transform.Find("Panel").transform.Find("Name").GetComponent<TextMeshProUGUI>();
        nameText.text = "Player " + player.index.ToString();
    }
}
