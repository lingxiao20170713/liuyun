using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModalManager
{
    public List<IModal> modals { get; private set; }

    private static ModalManager _instance;
    public static ModalManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new ModalManager();
                _instance.Init();
            }
            return _instance;
        }
    }

    private void Init()
    {
        modals = new List<IModal>();
    }

    public IModal LoadModal(Type modalType)
    {
        IModal modal = Activator.CreateInstance(modalType) as IModal;
        modals.Add(modal);
        return modal;
    }

    public void RemoveModal(IModal modal)
    {
        modals.Remove(modal);
    }

    public void Update()
    {
        foreach(IModal modal in modals)
        {
            modal.Update();
        }
    }
}
