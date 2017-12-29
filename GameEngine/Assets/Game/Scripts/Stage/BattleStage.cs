using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStage : BaseStage
{
    public override void Enter()
    {
        base.Enter();
        GamePublisher.Publish(EventType.STAGE_BATTLE_IN, this);
    }

    public override void Exit()
    {
        base.Exit();
        GamePublisher.Publish(EventType.STAGE_BATTLE_OUT, this);
    }

    public override string GetLevelName()
    {
        return "Battle";
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
