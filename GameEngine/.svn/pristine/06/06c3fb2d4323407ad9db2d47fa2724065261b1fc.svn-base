using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStage : BaseStage
{
    public override void Enter()
    {
        base.Enter();
        GamePublisher.Publish(EventType.STAGE_LOBBY_IN, this);
    }

    public override void Exit()
    {
        base.Exit();
        GamePublisher.Publish(EventType.STAGE_LOBBY_OUT, this);//退出lobby消息
    }

    public override string GetLevelName()
    {
        return "Lobby";
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
}
