using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginControler : BaseControler
{
    private LoginModal loginModal = null;

    private static LoginControler _instance;
    public static LoginControler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LoginControler();
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
        if(loginModal == null)
        {
            loginModal = ModalManager.Instance.LoadModal(typeof(LoginModal)) as LoginModal;
        }
    }

    public void OnExitModal()
    {
        if(loginModal!=null)
        {
            ModalManager.Instance.RemoveModal(loginModal);
            loginModal.Destroy();
            loginModal = null;
        }
    }

    public override void AddEvent()
    {
        base.AddEvent();
        GamePublisher.Instance.RegisterListener(EventType.STAGE_LOGIN_IN, this);
        GamePublisher.Instance.RegisterListener(EventType.STAGE_LOGIN_OUT, this);
    }

    public override void RemoveEvent()
    {
        base.RemoveEvent();
        GamePublisher.Instance.RemoveListener(EventType.STAGE_LOGIN_IN, this);
        GamePublisher.Instance.RemoveListener(EventType.STAGE_LOGIN_OUT, this);
    }

    public override void ProcessEvent(GameEvent evt)
    {
        base.ProcessEvent(evt);
        switch(evt.Type)
        {
            case EventType.STAGE_LOGIN_IN:
                OnEnterModal();
                break;
            case EventType.STAGE_LOGIN_OUT:
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
