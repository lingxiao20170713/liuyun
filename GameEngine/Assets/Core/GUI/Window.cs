using UnityEngine;
using System.Collections;

public class Window
{
    protected GameObject m_wnd = null;
    private Canvas m_panel = null;
    protected string m_name = string.Empty;
    private bool m_done = false;
    private LayerType m_layer = LayerType.None;

    public GameObject Wnd
    {
        set { m_wnd = value; }
        get { return m_wnd; }
    }

    public string Name
    {
        get { return m_name; }
    }

    public bool Done
    {
        get { return m_done; }
    }

    public virtual bool IsOpened
    {
        get { return m_wnd != null && m_wnd.activeSelf; }
    }

    public virtual string Bundle
    {
        get { return string.Empty; }
    }

    public Window(LayerType layer)
    {        
        m_name = GetType().Name;
        m_layer = layer;
    }

    protected virtual void OnStart()
    {
        GetLayer();
    }

    protected virtual void OnActive(bool active)
    {

    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnDestroy()
    {

    }

    public void Start()
    {
        if (m_wnd == null)
            return;

        m_panel = m_wnd.GetComponent<Canvas>();
        if (m_panel == null)
            Debug.LogErrorFormat("Window \'{0}\' not have UIPanel component", m_name);

        m_done = true;
        OnStart();
    }

    public void DestroyWnd()
    {
        if (m_wnd != null)
        {
            GameObject.Destroy(m_wnd);
            m_wnd = null;

            Resources.UnloadUnusedAssets();
        }
    }

    public void Destroy()
    {
        OnDestroy();
        DestroyWnd();
        m_name = string.Empty;
        m_done = false;
        m_layer = LayerType.None;
    }

    public void Active(bool active)
    {
        if (m_wnd == null)
            return;

        if (m_wnd.activeSelf == active || !m_done)
            return;

        m_wnd.SetActive(active);
        OnActive(active);
    }

    public void Update()
    {
        if (m_wnd == null)
            return;

        if (!m_wnd.activeSelf || !m_done)
            return;

        OnUpdate();
    }

    public int GetLayer()
    {
        return ((int)m_layer);
    }

    public void ChangeToLayer(LayerType layer)
    {
        m_layer = layer;
    }

    protected GameObject Find(string path)
    {
        return m_wnd.transform.Find(path).gameObject;
    }

    protected T Find<T>(string path) where T : MonoBehaviour
    {
        return m_wnd.transform.Find(path).GetComponent<T>();
    }

    protected UIEventListener Register(string path)
    {
        GameObject go = m_wnd.transform.Find(path).gameObject;
        if (go == null)
            return null;

        return UIEventListener.Get(go);
    }

    protected UIEventListener Register(GameObject go)
    {
        if (go == null)
            return null;

        return UIEventListener.Get(go);
    }
}

public class SingletonWindow<T> : Window where T : Window
{
    protected static T ms_instance = null;

    protected SingletonWindow(LayerType layer) :
        base(layer)
    {
        ms_instance = this as T;
    }

    public static T Instance
    {
        get { return ms_instance; }
    }

    protected override void OnDestroy()
    {
        ms_instance = null;
        base.OnDestroy();
    }

    public virtual void Open()
    {
        WindowManager.Instance.Active(m_name);
    }

    public virtual void Close()
    {
        WindowManager.Instance.Active(m_name, false);
    }
}

public enum LayerType
{
    None,
    Layer_0,
    Layer_1,
    Layer_2,
    Layer_3,
    OverLayer
}