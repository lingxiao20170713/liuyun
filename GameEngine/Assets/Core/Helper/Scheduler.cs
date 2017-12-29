using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FunctionPointer();
public delegate void ParameterizedFunctionPointer(params object[] paramiters);

public static class Scheduler
{
    private static List<ScheduledEvent> _events = new List<ScheduledEvent>();

    public static void RegisterEvent(/*double*/float scheduleTime, FunctionPointer functionPointer)
    {
        RegisterNamedEvent(string.Empty, scheduleTime, functionPointer);
    }

    public static void RegisterEvent(/*double*/float scheduleTime, ParameterizedFunctionPointer functionPointer, params object[] paramiters)
    {
        RegisterNamedEvent(string.Empty, scheduleTime, functionPointer, paramiters);
    }

    public static void RegisterNamedEvent(string name, /*double*/float scheduleTime, FunctionPointer functionPointer)
    {
        Remove(name);

        lock (_events)
        {
            _events.Add(new ScheduledEvent(name, functionPointer, scheduleTime));
        }
    }

    public static void RegisterNamedEvent(string name, /*double*/float scheduleTime, ParameterizedFunctionPointer functionPointer, params object[] paramiters)
    {
        Remove(name);

        lock (_events)
        {
            _events.Add(new ScheduledEvent(name, functionPointer, paramiters, scheduleTime));
        }
    }

    public static void Remove(string name)
    {
        if (string.IsNullOrEmpty(name))
            return;

        lock (_events)
        {
            for (int i = _events.Count - 1; i >= 0; --i)
            {
                ScheduledEvent e = _events[i];
                if (e.name == name)
                    _events.RemoveAt(i);
            }
        }
    }

    public static void ExecuteSchedule()
    {
        lock (_events)
        {
            for (int i = _events.Count - 1; i >= 0; --i)
            {
                ScheduledEvent e = _events[i];
                //if (System.DateTime.Now >= e.targetTime)
                if (Time.time >= e.targetTime)
                {
                    _events.RemoveAt(i);
                    if (e.funcPointer != null)
                    {
                        e.funcPointer();
                    }
                    else if (e.paramFuncPointer != null)
                    {
                        e.paramFuncPointer(e.paramiters);
                    }
                }
            }
        } // end lock
    }
}

public class ScheduledEvent
{
    public string name; // 事件名
    public FunctionPointer funcPointer;
    public ParameterizedFunctionPointer paramFuncPointer;
    //public DateTime targetTime;    
    public float targetTime;
    public object[] paramiters;

    public ScheduledEvent(string name, FunctionPointer pointer, float time/*double time*/)
    {
        this.name = name;
        funcPointer = pointer;
        //targetTime = DateTime.Now.AddMilliseconds(time);
        targetTime = Time.time + time * 0.001f;
    }

    public ScheduledEvent(string name, ParameterizedFunctionPointer pointer, object[] parmaiters, float time/*double time*/)
    {
        this.name = name;
        this.paramFuncPointer = pointer;
        //this.targetTime = DateTime.Now.AddMilliseconds(time);
        targetTime = Time.time + time * 0.001f;
        this.paramiters = parmaiters;
    }
}
