using UnityEngine;
using System.Collections;

public class DialogBase
{
    protected GameObject m_panel = null;

    public GameObject panel
    {
        get { return m_panel; }
    }

    public bool opened
    {
        get { return m_panel.activeSelf; }
    }

    public DialogBase(GameObject go)
    {
        m_panel = go;
    }

    public virtual void Open()
    {
        m_panel.SetActive(true);
    }

    public virtual void Close()
    {
        m_panel.SetActive(false);
        Dialog.Instance.Close();
    }

    // 设置触摸关闭
    public void SetTapClose(string path)
    {
        Register(path).onClick = (GameObject go) =>
        {
            Close();
        };
    }

    protected GameObject Find(string path)
    {
        return m_panel.transform.Find(path).gameObject;
    }

    protected T Find<T>(string path) where T : MonoBehaviour
    {
        return m_panel.transform.Find(path).GetComponent<T>();
    }

    protected UIEventListener Register(string path)
    {
        GameObject go = m_panel.transform.Find(path).gameObject;
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

    protected virtual void Refresh()
    {

    }
}
