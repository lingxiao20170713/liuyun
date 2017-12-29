using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePublisher
{
    private Dictionary<int, List<IEventListener>> _eventListenerMap = new Dictionary<int, List<IEventListener>>();

    private static GamePublisher _instance;
    public static GamePublisher Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GamePublisher();
            }
            return _instance;
        }
    }

    public void RegisterListener(int eventType, IEventListener listener)
    {
        List<IEventListener> listenerList;

        if (_eventListenerMap.TryGetValue(eventType, out listenerList))
        {
            if (!listenerList.Contains(listener))
            {
                listenerList.Add(listener);
            }
        }
        else
        {
            listenerList = new List<IEventListener>();
            listenerList.Add(listener);
            _eventListenerMap.Add(eventType, listenerList);
        }
    }

    public void RemoveListener(int eventType)
    {
        if (_eventListenerMap.ContainsKey(eventType))
        {
            _eventListenerMap.Remove(eventType);
        }
    }

    public void RemoveListener(int eventType, IEventListener listener)
    {
        List<IEventListener> listenerList;

        if (_eventListenerMap.TryGetValue(eventType, out listenerList))
        {
            if (listenerList.Contains(listener))
            {
                listenerList.Remove(listener);
            }
        }
    }

    public bool HasEventListener(int eventType, IEventListener listener)
    {
        List<IEventListener> listenerList;

        if (this._eventListenerMap.TryGetValue(eventType, out listenerList))
        {
            if (listenerList.Contains(listener))
            {
                return true;
            }
        }
        return false;
    }

    public void PublishEvent(int eventType, GameEvent evt)
    {
        List<IEventListener> listenerList;
        if (_eventListenerMap.TryGetValue(eventType, out listenerList))
        {
            for (int i = 0; i < listenerList.Count; i++)
            {
                IEventListener listener = listenerList[i];
                listener.ProcessEvent(evt);
            }
        }
    }

    public static void Publish(int eventType, object source, object data, GameEvent.CallBack callBack, params object[] args)
    {
        GameEvent evt = new GameEvent(eventType,source, data);
        evt.CallBackFunc = callBack;
        evt.CallBackArgs = args;
        GamePublisher.Instance.PublishEvent(eventType, evt);
    }

    public static void Publish(int eventType, object source, object data)
    {
        GameEvent evt = new GameEvent(eventType,source, data);
        GamePublisher.Instance.PublishEvent(eventType, evt);
    }

    public static void Publish(int eventType, object source)
    {
        GameEvent evt = new GameEvent(eventType,source);
        GamePublisher.Instance.PublishEvent(eventType, evt);
    }

    public static void Publish(int eventType, object source, Dictionary<string, object> paramDic)
    {
        GameEvent evt = new GameEvent(eventType,source);
        evt.Params = paramDic;

        GamePublisher.Instance.PublishEvent(eventType, evt);
    }

    public static void Publish(int eventType, GameEvent evt)
    {
        GamePublisher.Instance.PublishEvent(eventType, evt);
    }

    public static void Publish(int eventType)
    {
        GameEvent evt = new GameEvent(eventType,null);
        GamePublisher.Instance.PublishEvent(eventType, evt);
    }
}
