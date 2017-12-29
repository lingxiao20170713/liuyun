using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseView:IEventListener,IView
{
    public bool bHaveChildPanel { get; set; }
    protected bool beShow = false;//界面是否显示

    // 界面显示前传入的数据
    private object _viewParams;
    public object ViewParams { get { return _viewParams; } set { _viewParams = value; } }

    public BaseView(string assetPath = "", bool haveChildPanel = false)
    {
        LoadPanel(assetPath, null);
        Init();
        AddEvent();
        bHaveChildPanel = haveChildPanel;
    }

    public BaseView(string assertPath, Transform parent)
    {
        LoadPanel(assertPath, parent);
        Init();
        AddEvent();
    }

    // 加载资源
    protected virtual void LoadPanel(string assetPath, Transform parent)
    {

    }

    public virtual int GetModuleType()
    {
        return (int)ModuleType.NONE;
    }

    public virtual void Update()
    {

    }
    public virtual GameObject ViewObject()
    {
        return null;
    }
    public virtual void Show(object pData)
    {
        _viewParams = pData;
        Show();
    }
    public virtual void Show()
    {
        beShow = true;

        CheckModuleLockedWidget();

        //UIManager.Instance.Show(this);                                              //显示出来
        OnShowUINotify();
        OnShowDefaultOperation();
    }

    // 在显示界面之前，检查某些模块是否解锁，某些组件能否显示
    protected virtual void CheckModuleLockedWidget() { }
    // 打开界面后广播
    protected virtual void OnShowUINotify()
    {
        //GamePublisher.Publish(EventConst.UI_SHOW_NOTIFY, this, GetModuleType());
    }
    // 打开界面的初始化操作等
    protected virtual void OnShowDefaultOperation()
    {
        iTween.ScaleFrom(ViewObject(), iTween.Hash("scale", new Vector3(1.5f, 1.5f, 1f), "islocal", true, "time", 1f));
    }

    public virtual void Hide()
    {
        if (beShow == false)
            return;
        beShow = false;

        OnTweenHideFinished();
    }

    // 关闭界面后广播
    protected virtual void OnHideUINotify()
    {
        //GamePublisher.Publish(EventConst.UI_HIDE_NOTIFY, this, GetModuleType());
    }
    //隐藏界面后的数据重置
    protected virtual void OnHideResetData() { }
    // 关闭界面后的重置数据
    protected virtual void OnDestroyResetData() { }
    private void OnTweenHideFinished()
    {
        //UIManager.Instance.Hide(this);

        OnHideUINotify();
        OnHideResetData();

        //Dispatch(UIEventConst.UI_DESTORY);
    }

    public virtual void Init() { }

    public virtual void AddEvent() { }

    public virtual void RemoveEvent() { }

    public virtual void ProcessEvent(GameEvent evt) { }

    public virtual void Destroy()
    {
        OnDestoryFinished();
    }
    private void OnDestoryFinished()
    {
        OnDestroyResetData();
        RemoveEvent();

        //UIManager.Instance.Destroy(this);
        OnHideUINotify();
    }
}


public enum ModuleType
{
    NONE = 0,
    LOGIN_MAIN =5,
    LOBBY_MAIN = 10,
    BATTLE_MAIN = 100
}