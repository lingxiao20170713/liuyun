using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseStage:IEventListener
{
    public virtual void Enter()
    {
        AddEvent();
        OpenDefaultLevel();
    }

    //初始化要加载的场景
    public virtual void Open(string mapLevel) { }

    public virtual void OpenDefaultLevel()
    {
        string levelName = GetLevelName();
        SceneManager.LoadSceneAsync(levelName);
    }

    public virtual string GetLevelName()
    {
        return "";
    }

    public virtual void Exit()
    {
        RemoveEvent();
    }

    public virtual void Update() { }
    public virtual void LateUpdate() { }

    protected virtual void AddEvent() { }

    protected virtual void RemoveEvent() { }

    public void RegistListener(int eventType)
    {
        GamePublisher.Instance.RegisterListener(eventType, this);
    }

    public void RemoveListener(int eventType)
    {
        GamePublisher.Instance.RemoveListener(eventType, this);
    }

    public void ProcessEvent(GameEvent evt) { }
}
