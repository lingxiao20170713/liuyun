using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    private int eventType;
    private object source;
    private object param;

    public delegate void CallBack(params object[] args);
    private CallBack callBackFunc;
    private object[] callBackArgs;

    private Dictionary<string, object> _params = new Dictionary<string, object>();

    public GameEvent() { }

    public GameEvent(int type,object source)
    {
        eventType = type;
        this.source = source;
    }

    public GameEvent(int type,object source,object param)
    {
        eventType = type;
        this.source = source;
        this.param = param;
    }

    public int Type { get{ return eventType; } }
    public object Source { get { return source; } }
    public object Param { get { return param; } }

    public CallBack CallBackFunc
    {
        get { return callBackFunc; }
        set { callBackFunc = value; }
    }

    public object[] CallBackArgs
    {
        get { return callBackArgs; }
        set { callBackArgs = value; }
    }

    public Dictionary<string, object> Params
    {
        get { return this._params; }
        set { this._params = value; }
    }
}
