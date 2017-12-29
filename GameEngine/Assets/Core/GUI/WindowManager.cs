using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class WindowManager : Singleton<WindowManager>
{
    private Transform m_wnd_parent;
    private List<Window> m_wnds = new List<Window>();

    //弹窗层设置
    private int LayerSet = 0;
    public string[] AnchorCenter = new string[5]{ "GUI/Layer_0", "GUI/Layer_1", "GUI/Layer_2", "GUI/Layer_3", "GUI/OverLayer" };

    public Transform WndParent
    {
        get
        {
            m_wnd_parent = GameObject.Find(AnchorCenter[LayerSet-1]).transform;
            LayerSet = 0;
            return m_wnd_parent;
        }
    }

    public void Register(string wnd)
    {
        if (string.IsNullOrEmpty(wnd))
        {
            Debug.Log("Register() - wnd is null.");
            return;
        }

        if (WindowManager.Instance.Find(wnd) != null)
        {
            Debug.LogWarningFormat("Window {0} is registered!", wnd);
            return;
        }

        object window = Activator.CreateInstance(Type.GetType(wnd));
        if (window == null)
        {
            Debug.LogWarningFormat("Register() - Create instance: {0} failed!", wnd);
        }
        Register(window as Window);
    }

    public bool Register(Window wnd)
    {
        if (Find(wnd.Name) != null)
            return false;

        m_wnds.Add(wnd);
        return true;
    }

    public bool Unregister(string name)
    {
        Window wnd = Find(name);
        if (wnd == null)
            return false;

        m_wnds.Remove(wnd);
        return true;
    }

    public void OnExit()
    {
        for (int i = 0; i < m_wnds.Count; ++i)
        {
            UnregisterAndDestroy(m_wnds[i].Name);
        }
    }
    public void UnregisterAndDestroy(string wnd)
    {
        Destroy(wnd);
        Unregister(wnd);
    }

    public Window Find(string name)
    {
        return m_wnds.Find((Window wnd) =>
        {
            return wnd.Name == name;
        });
    }

    public bool Active(string name, bool active = true)
    {
        Window wnd = Find(name);
        if (wnd == null)
            return false;
 
        if (wnd.Wnd == null) // 用到时才加载
        {
            Debug.LogFormat("Start {0} loading...", wnd.Bundle);
            AssetManager.Instance.LoadUI(wnd.Bundle, (GameObject prefab) =>
                {
                    if (wnd.Wnd == null)
                    {
                        LayerSet = wnd.GetLayer();
                        wnd.Wnd = Util.Instantiate(prefab, WndParent);
                        wnd.Wnd.SetActive(false);
                        wnd.Start();
                        wnd.Active(active);
                    }
                });

            return true;
        }
        else if (!wnd.Done) // TODO: 临时这样写
        {
            wnd.Start();
        }

        wnd.Active(active);
        return true;
    }

    public void Update()
    {
        for (int i = 0; i < m_wnds.Count; ++i)
            m_wnds[i].Update();
    }

    public void Destroy(string name)
    {
        Window wnd = Find(name);
        if (wnd == null)
            return;

        wnd.Destroy();
    }
}
