using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Manager
{
    private SceneManager sceneManager;
    private GameStateManager gameStateManager;
    private UIManager uiManager;
    private PlayerManager playerManager;
    private CameraManager cameraManager;

    public Transform debugParent;
    public bool debug = false;
    public GameObject debugLineRendererPrefab;
    public List<GameObject> debugLineRenderers;

    public GameObject ZeroValueRock;

    public Transform parent;

    public List<GameObject> levelObjects = new List<GameObject>();

    public int currentQuota = 0;
    public int day = 1;

    public int initialLevelDuration = 10;
    public int levelDuration = 10;
    private float startLevelTime;

    public int currentMinLevelValue = 10;
    public int currentMaxLevelValue = 100;
    public int initialMinLevelValue = 10;
    public int initialMaxLevelValue = 100;

    public NormalGrapple grapple;

    public List<GrabbableObject> levelPrefabs = new List<GrabbableObject>();
    private Dictionary<string, GrabbableObject> levelPrefabDict = new Dictionary<string, GrabbableObject>();

    public Template fillerTemplate;
    public List<Template> firstLayerSmallTemplates = new List<Template>();
    public List<Template> firstLayerMediumTemplates = new List<Template>();
    public List<Template> firstLayerLargeTemplates = new List<Template>();

    public List<Template> secondLayerSmallTemplates = new List<Template>();
    public List<Template> secondLayerMediumTemplates = new List<Template>();
    public List<Template> secondLayerLargeTemplates = new List<Template>();

    public List<Template> thirdLayerSmallTemplates = new List<Template>();
    public List<Template> thirdLayerMediumTemplates = new List<Template>();
    public List<Template> thirdLayerLargeTemplates = new List<Template>();

    public override void OnInitialStart()
    {
        base.OnInitialStart();

        foreach (GrabbableObject grabbableObject in this.levelPrefabs)
        {
            this.levelPrefabDict.Add(grabbableObject.name, grabbableObject);
        }

        this.sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        this.gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        this.uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        this.cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();

        SetupTemplates();
        ResetLevelParameters();
    }

    private void ResetLevelParameters()
    {
        this.day = 1;
        this.currentMinLevelValue = this.initialMinLevelValue;
        this.currentMaxLevelValue = this.initialMaxLevelValue;
        this.currentQuota = 0;
        this.levelDuration = this.initialLevelDuration;

        this.uiManager.UpdateQuota(this.currentQuota);
    }

    // From MaxInt -> -5
    private void SetupFirstLayerTemplates()
    {
        int minDepth = int.MaxValue;
        int maxDepth = -10;

        string[,] templateString =
            {
                {"Rock"}
            };
        Template template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.firstLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Gold"}
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.firstLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Large Gold"}
            };
        template = StringToTemplate(templateString, 2, minDepth, maxDepth);
        this.firstLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Small Gold", "Small Gold"}
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Small Gold"}
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Small Gold", "Rock"}
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Empty"},
                {"Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Gold", "Empty"},
                {"Empty", "Rock"},
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Small Gold", "Empty"},
                {"Empty", "Small Gold"},
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Empty", "Rock"},
                {"Empty", "Empty", "Empty"},
                {"Empty", "Gold", "Empty"},
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerLargeTemplates.Add(template);

        templateString = new string[,]
            {
                {"Gold", "Empty", "Rock"},
                {"Empty", "Empty", "Empty"},
                {"Empty", "Empty", "Rock"},
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerLargeTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Empty", "Rock"},
                {"Empty", "Empty", "Empty"},
                {"Gold", "Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 5, minDepth, maxDepth);
        this.firstLayerLargeTemplates.Add(template);
    }

    // From -5 -> -10
    private void SetupSecondLayerTemplates()
    {
        int minDepth = -10;
        int maxDepth = -15;

        string[,] templateString =
            {
                {"Rock"}
            };
        Template template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Gold"}
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"TNT Barrel"}
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Diamond"}
            };
        template = StringToTemplate(templateString, 2, minDepth, maxDepth);
        this.secondLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"TNT Barrel", "Empty", "Empty", "TNT Barrel"},
                {"Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Diamond", "Empty", "Empty"},
            };
        template = StringToTemplate(templateString, 4, minDepth, maxDepth);
        this.secondLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Empty", "Empty", "Rock"},
                {"Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Gold", "Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Empty", "Empty", "Small Gold", "Small Gold"},
                {"Small Gold", "Empty", "Empty", "Empty"},
                {"Empty", "Gold", "Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Empty"},
                {"Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Small Gold", "Empty"},
                {"Empty", "Small Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Small Gold", "TNT Barrel", "Small Gold"},
                {"Empty", "Empty", "Empty"},
                {"Small Gold", "Gold", "Small Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Empty", "TNT Barrel", "Empty"},
                {"Gold", "Empty", "Empty"},
                {"Empty", "Empty", "TNT Barrel"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Empty", "Empty", "Empty"},
                {"TNT Barrel", "Empty", "Large Gold"},
                {"Empty", "Empty", "Empty"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.secondLayerMediumTemplates.Add(template);
    }

    // From -10 -> MinInt
    private void SetupThirdLayerTemplates()
    {
        int minDepth = -15;
        int maxDepth = int.MinValue;

        string[,] templateString =
            {
                {"Rock"}
            };
        Template template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Diamond"}
            };
        template = StringToTemplate(templateString, 6, minDepth, maxDepth);
        this.thirdLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Gold"}
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"TNT Barrel"}
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerSmallTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Empty", "Empty", "Rock"},
                {"Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Gold", "Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Gold","Rock", "Empty", "Empty", "Gold"},
                {"Empty","Empty", "Large Rock", "Empty", "Empty"},
                {"Gold","Empty", "Gold", "Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"TNT Barrel", "Empty", "Empty", "TNT Barrel"},
                {"Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Diamond", "Diamond", "Empty"},
            };
        template = StringToTemplate(templateString, 4, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Empty", "Empty", "Small Gold", "Small Gold"},
                {"Small Gold", "Empty", "Empty", "Empty"},
                {"Empty", "Gold", "Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Empty"},
                {"Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Small Gold", "Empty"},
                {"Empty", "Small Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Small Gold", "TNT Barrel", "Small Gold"},
                {"Empty", "Empty", "Empty"},
                {"Small Gold", "Gold", "Small Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Empty", "TNT Barrel", "Empty"},
                {"Gold", "Empty", "Empty"},
                {"Empty", "Empty", "TNT Barrel"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Empty", "Empty", "Empty"},
                {"TNT Barrel", "Empty", "Large Gold"},
                {"Empty", "Empty", "Empty"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerMediumTemplates.Add(template);

        templateString = new string[,]
            {
                {"Rock", "Empty", "Empty", "Rock", "Empty", "Empty", "Rock"},
                {"Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                {"Rock", "Empty", "Empty", "Large Gold", "Empty", "Empty", "Rock"},
                {"Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                {"Rock", "Empty", "Empty", "Rock", "Empty", "Empty", "Rock"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerLargeTemplates.Add(template);

        templateString = new string[,]
            {
                {"Empty", "Empty", "Empty", "Rock", "TNT Barrel", "Empty", "Empty"},
                {"Empty", "TNT Barrel", "Empty", "Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Large Gold"},
                {"Empty", "Empty", "Empty", "Diamond", "Empty", "Empty", "Empty"},
                {"Large Gold", "Empty", "Empty", "Empty", "TNT Barrel", "Empty", "Empty"},
                {"Empty", "Empty", "Empty", "Empty", "Empty", "Diamond", "Empty"},
                {"Empty", "Empty", "Empty", "Diamond", "Empty", "Empty", "Empty"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerLargeTemplates.Add(template);

        templateString = new string[,]
            {
                {"Small Gold", "Empty", "Empty", "Rock", "Empty", "Rock", "Empty"},
                {"Empty", "Empty", "Small Gold", "Empty", "Empty", "Small Gold", "Empty"},
                {"Empty", "Rock", "Empty", "Empty", "Small Gold", "Empty", "Empty"},
                {"Rock", "Empty", "Empty", "Rock", "Empty", "Empty", "Rock"},
                {"Empty", "Empty", "Empty", "Empty", "Small Gold", "Empty", "Empty"},
                {"Empty", "Rock", "Empty", "Empty", "Rock", "Empty", "Empty"},
                {"Empty", "Empty", "Empty", "Rock", "Small Gold", "Empty", "Small Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerLargeTemplates.Add(template);

        templateString = new string[,]
            {
                {"Empty", "Rock", "Empty", "Rock", "Empty", "Rock", "Empty"},
                {"Rock", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Empty", "Rock", "Empty", "Empty", "TNT Barrel", "Empty"},
                {"TNT Barrel", "Empty", "Empty", "TNT Barrel", "Empty", "Empty", "Empty"},
                {"Empty", "Empty", "Rock", "Empty", "Empty", "Rock", "Empty"},
                {"Rock", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                {"Empty", "Empty", "Empty", "TNT Barrel", "Empty", "Rock", "Empty"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerLargeTemplates.Add(template);

        templateString = new string[,]
            {
                { "Gold", "Empty", "Empty", "Empty", "Empty", "Empty", "Gold"},
                { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                { "Empty", "Empty", "Empty", "Small Gold", "Empty", "Empty", "Empty"},
                { "Empty", "Empty", "Small Gold", "Small Gold", "Small Gold", "Empty", "Empty"},
                { "Empty", "Empty", "Empty", "Small Gold", "Empty", "Empty", "Empty"},
                { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                { "Gold", "Empty", "Empty", "Empty", "Empty", "Empty", "Gold"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerLargeTemplates.Add(template);

        templateString = new string[,]
            {
                { "Empty", "Rock", "Empty", "Empty", "Rock", "Empty", "Empty"},
                { "Empty", "Empty", "Empty", "Empty", "Empty", "Rock", "Empty"},
                { "Rock", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                { "Empty", "Empty", "Empty", "Large Gold", "Empty", "Empty", "Rock"},
                { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
                { "Empty", "Rock", "Empty", "Empty", "Empty", "Rock", "Empty"},
                { "Empty", "Empty", "Empty", "Rock", "Empty", "Empty", "Empty"},
            };
        template = StringToTemplate(templateString, 10, minDepth, maxDepth);
        this.thirdLayerLargeTemplates.Add(template);
    }
    private void SetupTemplates()
    {
        /*{ "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
        { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
        { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
        { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
        { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
        { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},
        { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty"},*/
        this.fillerTemplate = new Template(1, int.MaxValue, int.MinValue, new List<(Vector3, GrabbableObject)>());
        this.SetupFirstLayerTemplates();
        this.SetupSecondLayerTemplates();
        this.SetupThirdLayerTemplates();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStateManager.InLevel())
        {
            if (CheckTime())
            {
                EndDay();
            }
        }
    }

    public override void OnMainMenuStart()
    {
        base.OnMainMenuStart();
        ResetLevelParameters();
        this.uiManager.UpdateHighScore(GetHighScore());
    }
    public override void OnInitialEnd()
    {
        CleanInitialLevel();
    }
    public override void OnLevelStart()
    {
        // Destroy all gameobjects
        foreach (GameObject gameObject in this.levelObjects)
        {
            Destroy(gameObject);
        }

        (Vector3, Vector3) boundsTuple = this.cameraManager.CurrentBounds();
        Vector3Int bottomLeftBound = Vector3Int.FloorToInt(boundsTuple.Item1) - new Vector3Int(1, 0, 0);
        Vector3Int topRightBound = Vector3Int.FloorToInt(boundsTuple.Item2) + new Vector3Int(1, 0, 0);
        topRightBound.y = 1;
        GenerateAndSpawnLevel(bottomLeftBound, topRightBound);
        this.startLevelTime = Time.time;
    }

    public override void OnLevelEnd()
    {
        // Destroy all gameobjects
        foreach (GameObject gameObject in this.levelObjects)
        {
            Destroy(gameObject);
        }

        // If debug, destroy all linerenderers
        foreach (GameObject lineRenderer in this.debugLineRenderers)
        {
            Destroy(lineRenderer);
        }

        // Update current quota
        IncreaseQuota();

        // Update HighScore
        UpdateHighScore();

        // Update day
        day += 1;
    }

    private void UpdateHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            if (this.day > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", this.day);
                PlayerPrefs.Save();
                this.uiManager.UpdateHighScore(this.day);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", this.day);
            PlayerPrefs.Save();
            this.uiManager.UpdateHighScore(this.day);
        }
    }

    private int GetHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            return PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
            return 0;
        }
    }

    public override void OnBetweenLevelStart()
    {
        // Spawn some zero value rocks
        SpawnZeroValueRocks();
    }

    public void ChangeRockValue(int rockValueChange)
    {
        this.levelPrefabDict["Rock"].GetComponent<Pickup>().value += rockValueChange;
    }

    private bool CheckTime()
    {
        float timeLeft = levelDuration - (Time.time - startLevelTime);
        uiManager.UpdateTimeRemaining(Mathf.FloorToInt(timeLeft));

        return (Time.time - startLevelTime > levelDuration);
    }
    private void EndDay()
    {
        // Update game state
        if (playerManager.totalGold >= currentQuota)
        {
            this.gameStateManager.EnterGameState(GameStateManager.GameState.BetweenLevel);
        }
        else
        {
            this.gameStateManager.EnterGameState(GameStateManager.GameState.GameOver);
        }
    }
    private void IncreaseQuota()
    {
        // Scale with the number of players
        float playerFactor = 1 + ((this.playerManager.players.Count - 1) * 0.5f);

        // This means that around day 5, one person should get around 25 gold each day
        // Around day 10, one person should get around 50 gold each day
        float increaseAmount = playerFactor * 5 * this.day;
        this.currentQuota += Mathf.FloorToInt(increaseAmount);

        this.uiManager.UpdateQuota(this.currentQuota);
    }
    private Template PickTemplate(List<Template> possibleTemplates, int currentValue, int currentDepth)
    {
        // Filter out bad depth
        List<Template> filteredTemplates = new List<Template>();
        foreach (Template template in possibleTemplates)
        {
            if (currentDepth >= template.maxDepth && currentDepth <= template.minDepth)
            {
                filteredTemplates.Add(template);
            }
        }

        float valueFactor = 0;
        if (currentValue < this.currentMinLevelValue)
        {
            valueFactor = 0.5f;
        }

        float totalWeight = 0;
        foreach (Template temp in filteredTemplates)
        {
            float currentWeight = CalculateTemplateWeight(temp, valueFactor);
            totalWeight += currentWeight;
        }

        float r = UnityEngine.Random.Range(0, totalWeight);
        foreach (Template temp in filteredTemplates)
        {
            float currentWeight = CalculateTemplateWeight(temp, valueFactor);
            if (r <= currentWeight)
            {
                possibleTemplates.Remove(temp);
                return temp;
            }
            else
            {
                r -= currentWeight;
            }
        }

        return null;
    }

    private float CalculateTemplateWeight(Template temp, float valueFactor)
    {
        // To avoid a level with only few templates
        if (temp.value > 0.3f * this.currentMaxLevelValue) return 0;

        return temp.weight + Mathf.Clamp(valueFactor * temp.value, 0, 50);
    }
    private void GenerateAndSpawnLevel(Vector3Int bottomLeftBound, Vector3Int topRightBound)
    {
        int overflowCounter = 0;
        Level level = null;
        while (level == null)
        {
            overflowCounter++;
            level = GenerateLevel(bottomLeftBound, topRightBound);
            if (overflowCounter > 10)
            {
                Debug.Log("Generating a level took over " + overflowCounter.ToString() + " tries.");
                Debug.Log("Last generated level is spawned...");
            }
        }
        SpawnLevel(level);

        if (debug)
        {
            Debug.Log("Spawned Level");
            Debug.Log("Current Min Value: " + this.currentMinLevelValue);
            Debug.Log("Current Max Value: " + this.currentMaxLevelValue);
            Debug.Log("Level Value: " + level.levelValue);
            Debug.Log("Tries: " + overflowCounter);
        }
    }
    private Level GenerateLevel(Vector3Int bottomLeftBound, Vector3Int topRightBound)
    {
        // Creating a dict with possible positions for templates
        Dictionary<Vector3Int, Template> templateDict = new Dictionary<Vector3Int, Template>();
        for (int y = bottomLeftBound.y; y < topRightBound.y; y++)
        {
            for (int x = bottomLeftBound.x; x < topRightBound.x; x++)
            {
                templateDict.Add(new Vector3Int(x, y, 0), null);
            }
        }

        // Creating empty level
        Level level = new();

        // Randomly ordering possible positions
        List<Vector3Int> positions = templateDict.Keys.ToList();
        System.Random rnd = new System.Random();
        positions = positions.OrderBy(x => rnd.Next()).ToList();

        // For each position, if not occupied, find template for it
        foreach (Vector3Int position in positions)
        {
            if (templateDict[position] == null)
            {
                // Determine depth
                List<Template> possibleLargeTemplates = new List<Template>();
                List<Template> possibleMediumTemplates = new List<Template>();
                List<Template> possibleSmallTemplates = new List<Template>();
                if (position.y > -10)
                {
                    possibleLargeTemplates = new List<Template>(this.firstLayerLargeTemplates);
                    possibleMediumTemplates = new List<Template>(this.firstLayerMediumTemplates);
                    possibleSmallTemplates = new List<Template>(this.firstLayerSmallTemplates);
                }
                else if (position.y <= -10 && position.y > -15)
                {
                    possibleLargeTemplates = new List<Template>(this.secondLayerLargeTemplates);
                    possibleMediumTemplates = new List<Template>(this.secondLayerMediumTemplates);
                    possibleSmallTemplates = new List<Template>(this.secondLayerSmallTemplates);
                }
                else if (position.y <= -15)
                {
                    possibleLargeTemplates = new List<Template>(this.thirdLayerLargeTemplates);
                    possibleMediumTemplates = new List<Template>(this.thirdLayerMediumTemplates);
                    possibleSmallTemplates = new List<Template>(this.thirdLayerSmallTemplates);
                }

                Template chosenTemplate = null;
                List<Vector3Int> takenPositions = new List<Vector3Int>();
                while (possibleLargeTemplates.Count > 0 || possibleMediumTemplates.Count > 0 || possibleSmallTemplates.Count > 0)
                {
                    // Select random template
                    Template template = PickTemplate(possibleLargeTemplates, level.levelValue, position.y);
                    if (template == null)
                    {
                        template = PickTemplate(possibleMediumTemplates, level.levelValue, position.y);
                        if (template == null)
                        {
                            template = PickTemplate(possibleSmallTemplates, level.levelValue, position.y);
                            if (template == null)
                            {
                                break;
                            }
                        }
                    }

                    // Check if value keeps level value below treshold
                    if (template.value + level.levelValue < this.currentMaxLevelValue)
                    {
                        // Check whether template can be placed
                        takenPositions = template.TakenPositionsWithoutMargins(position);
                        bool valid = true;
                        foreach (Vector3Int takenPosition in takenPositions)
                        {
                            if (!CheckInBounds(bottomLeftBound, topRightBound, takenPosition) || templateDict[takenPosition] != null)
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (valid)
                        {
                            chosenTemplate = template;
                            break;
                        }
                    }
                }

                // If no template fits, use filler template
                if (chosenTemplate == null)
                {
                    chosenTemplate = this.fillerTemplate;
                }

                // Add template to level
                level.AddTemplate(position, chosenTemplate);

                // Mark positions as filled
                if (chosenTemplate != this.fillerTemplate)
                {
                    foreach (Vector3Int takenPosition in takenPositions)
                    {
                        templateDict[takenPosition] = chosenTemplate;
                    }
                }

                // If current value is above max value, restart.
                if (level.levelValue > this.currentMaxLevelValue)
                {
                    if (debug)
                    {
                        Debug.Log("Level generation aborted...");
                        Debug.Log("Level value: " + level.levelValue);
                    }
                    return null;
                }
            }
        }

        // Spawn templates if current value is between the bounds else restart
        if (level.levelValue >= this.currentMinLevelValue && level.levelValue <= this.currentMaxLevelValue) return level;
        else
        {
            if (debug)
            {
                Debug.Log("Level generation aborted...");
                Debug.Log("Level value: " + level.levelValue);
            }
            return null;
        }
    }
    private void SpawnLevel(Level level)
    {
        foreach ((Vector3, Template) tuple in level.templates)
        {
            Vector3 position = tuple.Item1;
            Template template = tuple.Item2;
            (List<GameObject>, List<GameObject>) t = SpawnTemplate(position, template);
            List<GameObject> spawnedObjects = t.Item1;
            if (debug)
            {
                foreach (GameObject lineRenderer in t.Item2)
                {
                    this.debugLineRenderers.Add(lineRenderer);
                }
            }
            foreach (GameObject spawnedObject in spawnedObjects)
            {
                this.levelObjects.Add(spawnedObject);
            }
        }
    }
    private bool CheckInBounds(Vector3Int bottomLeftBound, Vector3Int topRightBound, Vector3Int pos)
    {
        if (pos.x >= bottomLeftBound.x && pos.x < topRightBound.x && pos.y >= bottomLeftBound.y && pos.y < topRightBound.y) return true;
        return false;
    }
    private void SpawnZeroValueRocks()
    {
        int amount = UnityEngine.Random.Range(1 * day, 2 * day);
        for (int i = 0; i < amount; i++)
        {
            (Vector3, Vector3) boundsTuple = this.cameraManager.CurrentBounds();
            Vector3Int bottomLeftBound = Vector3Int.FloorToInt(boundsTuple.Item1);
            Vector3Int topRightBound = Vector3Int.FloorToInt(boundsTuple.Item2);
            topRightBound.y = 1;
            Vector3 position = GenerateRandomPoint(bottomLeftBound, topRightBound);
            GameObject g = Instantiate(this.ZeroValueRock, position, Quaternion.identity, parent);
            this.levelObjects.Add(g);
        }
    }
    private Vector3 GenerateRandomPoint(Vector3Int bottomLeftBound, Vector3Int topRightBound)
    {
        float x = UnityEngine.Random.Range(bottomLeftBound.x, topRightBound.x);
        float y = UnityEngine.Random.Range(bottomLeftBound.y, topRightBound.y);
        return new Vector3(x, y, 0);
    }
    private void CleanInitialLevel()
    {
        GameObject parent = GameObject.Find("InitialGameObjects");
        foreach (Transform child in parent.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
    }
    private (List<GameObject>, List<GameObject>) SpawnTemplate(Vector3 zeroPoint, Template template)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        foreach ((Vector3, string) tuple in template.objectList)
        {
            Vector3 objectPosition = tuple.Item1;
            string objectName = tuple.Item2;

            Vector3 position = zeroPoint + objectPosition;
            GrabbableObject grabbableObject = levelPrefabDict[objectName];

            GameObject gameObject = Instantiate(grabbableObject.gameObject, position, Quaternion.identity, parent);
            gameObjects.Add(gameObject);
        }

        // If debug, instantiate line renderer
        List<GameObject> lineRenderers = new List<GameObject>();
        if (this.debug && template.topRight != new Vector3Int(1, 1, 0))
        {
            GameObject outsideLineRenderer = Instantiate(debugLineRendererPrefab, this.debugParent);
            lineRenderers.Add(outsideLineRenderer);
            LineRenderer l = outsideLineRenderer.GetComponent<LineRenderer>();

            l.positionCount = 9;
            l.SetPosition(0, zeroPoint);
            l.SetPosition(1, new Vector3(zeroPoint.x + (template.topRight.x / 2), zeroPoint.y, 0));
            l.SetPosition(2, new Vector3(zeroPoint.x + template.topRight.x, zeroPoint.y, 0));
            l.SetPosition(3, new Vector3(zeroPoint.x + template.topRight.x, zeroPoint.y + (template.topRight.y / 2), 0));
            l.SetPosition(4, new Vector3(zeroPoint.x + template.topRight.x, zeroPoint.y + template.topRight.y, 0));
            l.SetPosition(5, new Vector3(zeroPoint.x + (template.topRight.x / 2), zeroPoint.y + template.topRight.y, 0));
            l.SetPosition(6, new Vector3(zeroPoint.x, zeroPoint.y + template.topRight.y, 0));
            l.SetPosition(7, new Vector3(zeroPoint.x, zeroPoint.y + (template.topRight.y / 2), 0));
            l.SetPosition(8, zeroPoint);

            GameObject insideLineRenderer = Instantiate(debugLineRendererPrefab, this.debugParent);
            lineRenderers.Add(insideLineRenderer);
            l = insideLineRenderer.GetComponent<LineRenderer>();
            l.startColor = Color.blue;
            l.endColor = Color.blue;
            l.startWidth = 0.05f;

            Vector3 realBottomLeftNoMargin = new Vector3(zeroPoint.x + template.margin, zeroPoint.y + template.margin) + new Vector3(0.1f, 0.1f, 0);
            Vector3 realTopRightNoMargin = new Vector3(zeroPoint.x + template.topRight.x - template.margin, zeroPoint.y + template.topRight.y - template.margin) - new Vector3(0.1f, 0.1f, 0);
            float middleX = (realBottomLeftNoMargin.x + realTopRightNoMargin.x) / 2;
            float middleY = (realBottomLeftNoMargin.y + realTopRightNoMargin.y) / 2;

            l.positionCount = 9;
            l.SetPosition(0, realBottomLeftNoMargin);
            l.SetPosition(1, new Vector3(middleX, realBottomLeftNoMargin.y, 0));
            l.SetPosition(2, new Vector3(realTopRightNoMargin.x, realBottomLeftNoMargin.y, 0));
            l.SetPosition(3, new Vector3(realTopRightNoMargin.x, middleY, 0));
            l.SetPosition(4, new Vector3(realTopRightNoMargin.x, realTopRightNoMargin.y, 0));
            l.SetPosition(5, new Vector3(middleX, realTopRightNoMargin.y, 0));
            l.SetPosition(6, new Vector3(realBottomLeftNoMargin.x, realTopRightNoMargin.y, 0));
            l.SetPosition(7, new Vector3(realBottomLeftNoMargin.x, middleY, 0));
            l.SetPosition(8, realBottomLeftNoMargin);
        }

        return (gameObjects, lineRenderers);
    }
    private Template StringToTemplate(string[,] templateString, int weight, int minDepth, int maxDepth)
    {
        // Example
        // [[Rock, Empty, Rock],
        // [Gold, Empty, Gold]]
        List<(Vector3, GrabbableObject)> objectTuples = new List<(Vector3, GrabbableObject)>();
        int y = templateString.Length - 1;
        int x = 0;
        for (int i = 0; i < templateString.GetLength(0); i++)
        {
            for (int j = 0; j < templateString.GetLength(1); j++)
            {
                x = j;
                y = templateString.Length - 1 - i;
                string s = templateString[i, j];
                if (s != "Empty")
                {
                    GrabbableObject grabbableObject = this.levelPrefabDict[s];
                    objectTuples.Add((new Vector3(x, y, 0), grabbableObject));
                }
            }
        }

        Template template = new Template(weight, minDepth, maxDepth, objectTuples);
        return template;
    }

    public static LevelManager GetInstance()
    {
        return GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
}
