using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlerManager:IEventListener
{
    private LoginControler loginControler;
    private LobbyControler lobbyControler;
    private BattleControler battleControler;

    private static ControlerManager _instance;
    public static ControlerManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new ControlerManager();
                _instance.Init();
            }
            return _instance;
        }
    }

    public void Start()
    {
        GamePublisher.Publish(EventType.START_LOAD_DATA);//加载数据
    }


    private void Init()
    {
        loginControler = LoginControler.Instance;
        lobbyControler = LobbyControler.Instance;
        battleControler = BattleControler.Instance;

        GamePublisher.Instance.RegisterListener(EventType.START_LOAD_DATA, this);
    }

    public void ProcessEvent(GameEvent evt)
    {
        switch(evt.Type)
        {
            case EventType.START_LOAD_DATA:
                break;
            default:
                break;
        }
    }
}
