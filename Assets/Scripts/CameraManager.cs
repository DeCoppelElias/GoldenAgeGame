using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Manager
{
    private GameStateManager gameStateManager;
    private PlayerManager playerManager;
    private enum CameraState { Normal, Level}
    private CameraState cameraState = CameraState.Normal;

    private Vector3 initialCameraPositionLevel = new Vector3(0, -1, -10);
    private Vector3 cameraPositionLevel = new Vector3(0, -1, -10);
    public float initialCameraSizeLevel = 6.5f;
    public float cameraSizeLevel = 6.5f;
    private Vector3 cameraPositionNormal = new Vector3(0, -3, -10);
    private float cameraSizeNormal = 8.5f;

    private void ResetCameraParameters()
    {
        this.cameraPositionLevel = this.initialCameraPositionLevel;
        this.cameraSizeLevel = this.initialCameraSizeLevel;
    }
    private void ToNormalState()
    {
        this.cameraState = CameraState.Normal;
        Camera.main.transform.position = this.cameraPositionNormal;
        Camera.main.orthographicSize = this.cameraSizeNormal;
    }
    private void ToLevelState()
    {
        this.cameraState = CameraState.Level;
        Camera.main.transform.position = this.cameraPositionLevel;
        Camera.main.orthographicSize = this.cameraSizeLevel;
    }

    public override void OnMainMenuEnd()
    {
        base.OnMainMenuEnd();

        if (this.playerManager.players.Count > 1)
        {
            this.ZoomOut(this.playerManager.players.Count - 1);
        }
    }
    public override void OnInitialStart()
    {
        base.OnInitialStart();
        this.gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        ResetCameraParameters();
        ToNormalState();
    }

    public override void OnLevelStart()
    {
        base.OnLevelStart();
        ToLevelState();
    }

    public override void OnLevelEnd()
    {
        base.OnLevelStart();
        ToNormalState();
    }

    public override void OnMainMenuStart()
    {
        base.OnMainMenuStart();
        ResetCameraParameters();
    }

    public void Travel(int offset)
    {
        cameraPositionNormal += new Vector3(offset, 0, 0);
        Camera.main.transform.position += new Vector3(offset, 0, 0);
    }

    private void ToState(CameraState cameraState)
    {
        if (cameraState == CameraState.Normal) ToNormalState();
        else if (cameraState == CameraState.Level) ToLevelState();
    }

    private (Vector3, Vector3) LevelBounds()
    {
        CameraState previousState = this.cameraState;
        ToState(CameraState.Level);
        Vector3 bottomLeftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRightBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        ToState(previousState);
        return (bottomLeftBound, topRightBound);
    }

    private (Vector3, Vector3) NormalBounds()
    {
        CameraState previousState = this.cameraState;
        ToState(CameraState.Normal);
        Vector3 bottomLeftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRightBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        ToState(previousState);
        return (bottomLeftBound, topRightBound);
    }
    public (Vector3, Vector3) CurrentBounds()
    {
        if (gameStateManager.InLevel())
        {
            return LevelBounds();
        }
        else
        {
            return NormalBounds();
        }
    }
    public void ZoomOut(int zoomAmount)
    {
        // Calculate new camera position and size in level
        CameraState previousState = this.cameraState;
        ToState(CameraState.Level);
        float previousTopY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        Camera.main.orthographicSize += zoomAmount;
        float newTopY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float distance = newTopY - previousTopY;
        ToState(previousState);

        this.cameraPositionLevel += new Vector3(0, -distance, 0);
        this.cameraSizeLevel += zoomAmount;
    }
}
