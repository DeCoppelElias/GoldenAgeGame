using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelSign : MonoBehaviour
{
    private PlayerManager playerManager;
    private SceneManager sceneManager;
    private CameraManager cameraManager;
    private bool right = true;

    private void Start()
    {
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        this.sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        this.cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }

    public void SetDirection(bool right)
    {
        this.right = right;
        if (!right) RotateSign();
    }

    private void RotateSign()
    {
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        for (int childIndex = 0; childIndex < this.transform.childCount; childIndex++)
        {
            RectTransform child = this.transform.GetChild(childIndex).GetComponent<RectTransform>();
            child.localScale = new Vector3(-child.localScale.x, child.localScale.y, child.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckPlayersNear())
        {
            int offset = 60;
            if (!right)
            {
                offset = -60;
            }
            this.cameraManager.Travel(offset);
            this.playerManager.Travel();
            this.sceneManager.Travel();
        }
    }

    private bool CheckPlayersNear()
    {
        List<Vector3> playerPositions = this.playerManager.GetAllPlayerPositions();
        if (playerPositions.Count == 0) return false;

        foreach (Vector3 playerPosition in playerPositions)
        {
            if (Vector3.Distance(playerPosition, this.transform.position) > 2)
            {
                return false;
            }
        }

        return true;
    }
}
