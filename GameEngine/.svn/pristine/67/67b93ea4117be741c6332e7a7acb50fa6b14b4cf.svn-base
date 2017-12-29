using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageManager
{
    private BaseStage[] stages;
    private StageEnum curStage;
    private StageEnum nextStage;
    private string nextMapLevel;

    private static StageManager _instance;
    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new StageManager();
            }
            return _instance;
        }
    }

    public StageManager()
    {
        int count = Enum.GetNames(typeof(StageEnum)).Length;
        stages = new BaseStage[count];
        stages[(int)StageEnum.None] = null;
        stages[(int)StageEnum.LOGIN] = new LoginStage();
        stages[(int)StageEnum.LOBBY] = new LobbyStage();
        stages[(int)StageEnum.BATTLE] = new BattleStage();
    }

    public void Start()
    {
        curStage = StageEnum.None;
        nextStage = StageEnum.None;
    }

    public void Update()
    {
        if(stages[(int)nextStage]!=null)
        {
            if (curStage != nextStage)
            {
                if (stages[(int)curStage] != null)
                { stages[(int)curStage].Exit(); }
                stages[(int)nextStage].Enter();
                curStage = nextStage;
            }
            stages[(int)curStage].Update();
        }

    }

    public void LateUpdate()
    {

    }

    public void SetNextStage(StageEnum stage)
    {
        nextStage = stage;
    }

    public void SetNextStage(StageEnum stage, string mapLevel)
    {
        nextStage = stage;
        nextMapLevel = mapLevel;
    }

    public StageEnum GetCurStage()
    {
        return curStage;
    }
}

public enum StageEnum
{
    None = 0,
    LOGIN,
    LOBBY,
    BATTLE
}