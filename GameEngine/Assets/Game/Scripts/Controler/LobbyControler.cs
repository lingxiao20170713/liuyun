using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyControler : BaseControler
{
    public LobbyModal lobbyModal;

    private static LobbyControler _instance;
    public static LobbyControler Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new LobbyControler();
            }
            return _instance;
        }
    }

    public override void Init()
    {
        base.Init();
    }

    public void OnEnterModal()
    {
        if(lobbyModal == null)
        {
            lobbyModal = ModalManager.Instance.LoadModal(typeof(LobbyModal)) as LobbyModal;
        }
    }
    public void OnExitModal()
    {
        if (lobbyModal != null)
        {
            ModalManager.Instance.RemoveModal(lobbyModal);
            lobbyModal.Destroy();
            lobbyModal = null;
        }
    }
    public override void AddEvent()
    {
        base.AddEvent();
        GamePublisher.Instance.RegisterListener(EventType.STAGE_LOBBY_IN,this);
        GamePublisher.Instance.RegisterListener(EventType.STAGE_LOBBY_OUT,this);
    }

    public override void RemoveEvent()
    {
        base.RemoveEvent();
        GamePublisher.Instance.RemoveListener(EventType.STAGE_LOBBY_IN, this);
        GamePublisher.Instance.RemoveListener(EventType.STAGE_LOBBY_OUT, this);
    }

    public override void ProcessEvent(GameEvent evt)
    {
        base.ProcessEvent(evt);
        switch(evt.Type)
        {
            case EventType.STAGE_LOBBY_IN:
                OnEnterModal();
                break;
            case EventType.STAGE_LOBBY_OUT:
                OnExitModal();
                break;
            default:
                break;
        }
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
