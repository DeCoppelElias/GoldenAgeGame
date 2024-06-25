using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    public virtual void OnInitialStart()
    {
        return;
    }
    public virtual void OnInitialEnd()
    {
        return;
    }
    public virtual void OnLevelStart()
    {
        return;
    }
    public virtual void OnLevelEnd()
    {
        return;
    }
    public virtual void OnBetweenLevelStart()
    {
        return;
    }
    public virtual void OnBetweenLevelEnd()
    {
        return;
    }
    public virtual void OnMainMenuStart()
    {
        return;
    }
    public virtual void OnMainMenuEnd()
    {
        return;
    }
    public virtual void OnGameOver()
    {
        return;
    }
}
