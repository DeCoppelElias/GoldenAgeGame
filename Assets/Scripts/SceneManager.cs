using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneManager : Manager
{
    public int grassMaxDistance = 3;
    public int grassMinDistance = 1;
    public int CloudsMaxDistance = 3;
    public int CloudsMinDistance = 1;
    public float CloudsMinSpeed = 0.5f;
    public float CloudsMaxSpeed = 1;
    public Transform sceneParent;
    public Transform grassParent;
    public Transform cloudParent;

    public GameObject cloudPrefab;
    public GameObject grassPrefab;

    public Tile airTile;
    public Tile grassTile;
    public Tile groundTile;
    public Tilemap backGroundTilemap;
    public Tilemap groundTilemap;

    private List<GameObject> sceneProps = new List<GameObject>();

    public bool firstTimeSpawnShopSign = true;
    public bool firstTimeSpawnUpgradeSign = true;

    public GameObject shopSignPrefab;
    public GameObject mainMenuSignPrefab;
    public GameObject upgradeSignPrefab;

    public GameObject sunPrefab;
    private Sun sun;

    private LevelManager levelManager;
    private CameraManager cameraManager;

    private void Start()
    {
        this.levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        this.cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }
    public override void OnLevelStart()
    {
        ResetScene();
        sun.StartSinking(4, this.levelManager.levelDuration);
    }

    public override void OnLevelEnd()
    {
        // Reset Camera
        Camera.main.transform.position = new Vector3(0, -3, -10);
    }
    public override void OnMainMenuStart()
    {
        ResetScene();
        this.firstTimeSpawnShopSign = true;
        firstTimeSpawnUpgradeSign = true;
    }
    public override void OnBetweenLevelStart()
    {
        ResetScene();

        // Spawn Shop
        SpawnShopSign();
        SpawnMainMenuSign(false, new Vector3(50, 3, 0));

        if ((levelManager.day - 1) % 3 == 0)
        {
            // Spawn Upgrades
            SpawnUpgradeSign();
            SpawnMainMenuSign(true, new Vector3(-50, 3, 0));
        }
    }

    public void Travel()
    {
        ResetSceneTravel();
    }

    private void SpawnSun(Vector3 bottomLeftBound, Vector3 topRightBound)
    {
        float middleSceneX = (bottomLeftBound.x + topRightBound.x + 2) / 2;
        float middleSceneY = (topRightBound.y / 2) + 2;
        this.sun = Instantiate(sunPrefab, new Vector3(middleSceneX, middleSceneY, 0), Quaternion.identity, this.sceneParent).GetComponent<Sun>();
        this.sceneProps.Add(this.sun.gameObject);
    }

    private void SpawnShopSign()
    {
        GameObject shopSign = Instantiate(shopSignPrefab, new Vector3(10, 3, 0), Quaternion.identity, this.sceneParent);
        TravelSign travelSign = shopSign.GetComponent<TravelSign>();
        travelSign.SetDirection(true);
        if (firstTimeSpawnShopSign)
        {
            shopSign.transform.Find("Explanation").gameObject.SetActive(true);
            firstTimeSpawnShopSign = false;
        }
        else
        {
            shopSign.transform.Find("Explanation").gameObject.SetActive(false);
        }
        this.sceneProps.Add(shopSign);
    }

    private void SpawnMainMenuSign(bool right, Vector3 position)
    {
        GameObject shopSign = Instantiate(mainMenuSignPrefab, position, Quaternion.identity, this.sceneParent);
        TravelSign travelSign = shopSign.GetComponent<TravelSign>();
        travelSign.SetDirection(right);
        this.sceneProps.Add(shopSign);
    }

    private void SpawnUpgradeSign()
    {
        GameObject upgradeSign = Instantiate(upgradeSignPrefab, new Vector3(-10, 3, 0), Quaternion.identity, this.sceneParent);
        TravelSign travelSign = upgradeSign.GetComponent<TravelSign>();
        travelSign.SetDirection(false);
        if (firstTimeSpawnUpgradeSign)
        {
            upgradeSign.transform.Find("Explanation").gameObject.SetActive(true);
            firstTimeSpawnUpgradeSign = false;
        }
        else
        {
            upgradeSign.transform.Find("Explanation").gameObject.SetActive(false);
        }
        this.sceneProps.Add(upgradeSign);
    }
    private void GenerateScene(Vector3Int bottomLeftBound, Vector3Int topRightBound)
    {
        GenerateBackGround(bottomLeftBound, topRightBound);
        GenerateSceneProps(bottomLeftBound, topRightBound);
    }
    private void GenerateSceneProps(Vector3Int bottomLeftBound, Vector3Int topRightBound)
    {
        // Grass
        float x = bottomLeftBound.x;
        int r = Random.Range(CloudsMinDistance, CloudsMaxDistance);
        x += r;
        while (x < topRightBound.x)
        {
            GameObject sceneProp = Instantiate(grassPrefab, new Vector3(x, 3, 0), Quaternion.identity, grassParent);
            this.sceneProps.Add(sceneProp);

            r = Random.Range(grassMinDistance, grassMaxDistance);
            x += r;
        }

        // Clouds
        x = bottomLeftBound.x;
        r = Random.Range(CloudsMinDistance, CloudsMaxDistance);
        x += r;
        while (x < topRightBound.x)
        {
            float y = Random.Range(3f, topRightBound.y - 1);
            GameObject gameObject = Instantiate(cloudPrefab, new Vector3(x, y, 0), Quaternion.identity, cloudParent);
            Cloud c = gameObject.GetComponent<Cloud>();
            float speed = Random.Range(CloudsMinSpeed, CloudsMaxSpeed);
            c.Initialise(bottomLeftBound.x, topRightBound.x, speed);
            this.sceneProps.Add(gameObject);

            r = Random.Range(CloudsMinDistance, CloudsMaxDistance);
            x += r;
        }

        SpawnSun(bottomLeftBound, topRightBound);
    }
    private void GenerateBackGround(Vector3Int bottomLeftSceneBound, Vector3Int topRightSceneBound)
    {
        // Air
        for (int x = Mathf.FloorToInt(bottomLeftSceneBound.x / 2) - 1; x <= Mathf.FloorToInt(topRightSceneBound.x / 2) + 1; x++)
        {
            for (int y = 1; y < topRightSceneBound.y; y++)
            {
                backGroundTilemap.SetTile(new Vector3Int(x, y, 0), airTile);
            }
        }

        // Grass
        for (int x = Mathf.FloorToInt(bottomLeftSceneBound.x / 2) - 1; x <= Mathf.FloorToInt(topRightSceneBound.x / 2) + 1; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
            }
        }

        // Ground
        for (int x = Mathf.FloorToInt(bottomLeftSceneBound.x / 2) - 1; x <= Mathf.FloorToInt(topRightSceneBound.x / 2) + 1; x++)
        {
            for (int y = Mathf.FloorToInt(bottomLeftSceneBound.y / 2); y < 0; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
            }
        }
    }
    private void ResetScene()
    {
        this.backGroundTilemap.ClearAllTiles();
        this.groundTilemap.ClearAllTiles();
        DeleteSceneProps();

        (Vector3, Vector3) boundsTuple = this.cameraManager.CurrentBounds();
        Vector3Int bottomLeftBound = Vector3Int.FloorToInt(boundsTuple.Item1) - new Vector3Int(1, 0, 0);
        Vector3Int topRightBound = Vector3Int.FloorToInt(boundsTuple.Item2);
        GenerateScene(bottomLeftBound, topRightBound);
    }

    private void ResetSceneTravel()
    {
        this.backGroundTilemap.ClearAllTiles();
        this.groundTilemap.ClearAllTiles();

        (Vector3, Vector3) boundsTuple = this.cameraManager.CurrentBounds();
        Vector3Int bottomLeftBound = Vector3Int.FloorToInt(boundsTuple.Item1) - new Vector3Int(1, 0, 0);
        Vector3Int topRightBound = Vector3Int.FloorToInt(boundsTuple.Item2);
        GenerateScene(bottomLeftBound, topRightBound);
    }
    private void DeleteSceneProps()
    {
        foreach (GameObject sceneProp in this.sceneProps)
        {
            Destroy(sceneProp);
        }

        this.sceneProps = new List<GameObject>();
    }
}
