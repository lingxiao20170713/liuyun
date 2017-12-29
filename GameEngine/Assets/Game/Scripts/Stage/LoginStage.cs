using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginStage : BaseStage
{
    public override void Enter()
    {
        base.Enter();
        GamePublisher.Publish(EventType.STAGE_LOGIN_IN, this);
    }

    public override void Exit()
    {
        base.Exit();
        GamePublisher.Publish(EventType.STAGE_LOGIN_OUT, this);//退出login消息
    }

    public override string GetLevelName()
    {
        return "Login";
    }

    public override void Open(string mapLevel)
    {
        base.Open(mapLevel);
    }

    public override void OpenDefaultLevel()
    {
        base.OpenDefaultLevel();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
    }
}
