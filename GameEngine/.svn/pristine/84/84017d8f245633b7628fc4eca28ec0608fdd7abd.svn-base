using System;
using UnityEngine;

public class Singleton<T> where T : new()
{
    private static readonly object _lock = new object();
    private static T _instance;

    protected Singleton()
    {
    }

    public static bool Exists
    {
        get
        {
            return _instance != null;
        }
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = Activator.CreateInstance<T>();
                    }
                }
            }
            return _instance;
        }
    }

    public static void DeleteInstance()
    {
        lock (_lock)
        {
            _instance = default(T);
        }
    }
}


public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T uniqueInstance;

    protected S AddComponent<S>() where S : Component
    {
        S component = GetComponent<S>();
        if (component == null)
        {
            component = gameObject.AddComponent<S>();
        }
        return component;
    }

    protected virtual void Awake()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = (T)this;
            Exists = true;
        }
        else if (uniqueInstance != this)
        {
            throw new InvalidOperationException("Cannot have two instances of a SingletonMonoBehaviour : " + typeof(T).ToString() + ".");
        }
    }

    protected virtual void OnDestroy()
    {
        if (uniqueInstance == this)
        {
            Exists = false;
            uniqueInstance = null;
        }
    }

    public static bool Exists
    {
        get;
        private set;
    }

    public static T Instance
    {
        get
        {
            return uniqueInstance;
        }
    }
}


public class SingletonSpawningMonoBehaviour<T> : MonoBehaviour where T : SingletonSpawningMonoBehaviour<T>
{
    protected static bool applicationQuitting;
    private static T uniqueInstance;

    protected virtual void Awake()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = (T)this;
        }
        else if (uniqueInstance != this)
        {
            throw new InvalidOperationException("Cannot have two instances of a SingletonMonoBehaviour : " + typeof(T).ToString() + ".");
        }
    }

    protected virtual void OnApplicationQuit()
    {
        applicationQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (uniqueInstance == this)
        {
            Exists = false;
            uniqueInstance = null;
        }
    }

    public static bool Exists
    {
        get;
        private set;
    }

    public static T Instance
    {
        get
        {
            if (!Exists)
            {
                if (applicationQuitting || !Application.isPlaying)
                {
                    return null;
                }
                Type[] components = new Type[] { typeof(T) };
                GameObject target = new GameObject("Singleton " + typeof(T).ToString(), components);
                uniqueInstance = target.GetComponent<T>();
                UnityEngine.Object.DontDestroyOnLoad(target);
                Exists = true;
            }
            return uniqueInstance;
        }
    }
}
