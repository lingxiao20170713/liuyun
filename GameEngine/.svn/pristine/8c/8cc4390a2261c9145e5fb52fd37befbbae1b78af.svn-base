using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager
{
    List<IView> views { get; set; }
    List<IView> act_views { get; set; }
    private static ViewManager _instance = null;
    public static ViewManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new ViewManager();
                _instance.Init();
            }
            return _instance;
        }
    }

    public ViewManager(){}

    void Init()
    {
        views = new List<IView>();
        act_views = new List<IView>();
    }

    public void Show(IView ui)
    {
        //TODO
        if(!views.Contains(ui))
        {
            views.Add(ui);
            act_views.Add(ui);
        }

        foreach (var item in views)
        {
            if (item.GetModuleType() == ui.GetModuleType())
            {
                item.Show();
            }
        }

    }

    // 显示一个之前出现过的、缓存在了UIManage里的BaseUI界面。
    public void Show(ModuleType pShowedViewID)
    {
        bool ishave = false;
        for (int i = 0; i < views.Count; i++)
        {
            if (views[i].GetModuleType() == (int)pShowedViewID)
            {
                ishave = true;
                views[i].Show();
            }
        }
        if (!ishave)
        {
            Debug.LogError("界面不存在!");
        }
    }

    // 隐藏一个之前出现过的、缓存在了UIManage里的BaseUI界面。
    public void Hide(IView ui)
    {
        //TODO
        if(!views.Contains(ui))
        {
            return;
        }
        else
        {
            foreach (IView iz in views)
            {
                if (iz.GetModuleType()== ui.GetModuleType())
                {
                    iz.Hide();
                }
            }
        }

        if (act_views.Contains(ui))
        {
            act_views.Remove(ui);
        }

    }

    // 根据模块ID获取当前正在显示的界面
    public BaseView GetView(ModuleType moduleType)
    {
        //TODO
        return null;
    }

    public void Update ()
    {
		foreach(IView ui in act_views)
        {
            ui.Update();
        }
	}

    // 销毁ui
    public void Destroy(IView ui)
    {
        if (!views.Contains(ui))
        {
            return;
        }
        else
        {
            foreach (IView iz in views)
            {
                if (iz.GetModuleType() == ui.GetModuleType())
                {
                    iz.Destroy();
                }
            }
        }

        if (!act_views.Contains(ui))
        {
            return;
        }
        else
        {
            act_views.Remove(ui);
        }
    }
}
