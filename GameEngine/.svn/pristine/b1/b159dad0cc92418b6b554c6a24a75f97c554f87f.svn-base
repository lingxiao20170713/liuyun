using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControler:IEventListener, IControler
{
    public BaseControler()
    {
        Init();
    }

    public virtual void Init()
    {
        AddEvent();
    }

    public virtual void AddEvent() { }

    public virtual void RemoveEvent() { }

    public virtual void ProcessEvent(GameEvent evt) { }

    public virtual void Destroy()
    {
        RemoveEvent();
    }
}
