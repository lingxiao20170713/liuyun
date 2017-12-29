using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControler : BaseControler
{
    public BattleModal battleModal;

    private static BattleControler _instance;
    public static BattleControler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BattleControler();
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
        if(battleModal == null)
        {
            battleModal = ModalManager.Instance.LoadModal(typeof(BattleModal)) as BattleModal;
        }
    }

    public void OnExitModal()
    {
        if (battleModal != null)
        {
            ModalManager.Instance.RemoveModal(battleModal);
            battleModal.Destroy();
            battleModal = null;
        }
    }

    public override void AddEvent()
    {
        base.AddEvent();
        GamePublisher.Instance.RegisterListener(EventType.STAGE_BATTLE_IN,this);
        GamePublisher.Instance.RegisterListener(EventType.STAGE_BATTLE_OUT,this);
    }

    public override void RemoveEvent()
    {
        base.RemoveEvent();
        GamePublisher.Instance.RemoveListener(EventType.STAGE_BATTLE_IN, this);
        GamePublisher.Instance.RemoveListener(EventType.STAGE_BATTLE_OUT, this);
    }

    public override void ProcessEvent(GameEvent evt)
    {
        base.ProcessEvent(evt);
        switch(evt.Type)
        {
            case EventType.STAGE_BATTLE_IN:
                OnEnterModal();
                break;
            case EventType.STAGE_BATTLE_OUT:
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
