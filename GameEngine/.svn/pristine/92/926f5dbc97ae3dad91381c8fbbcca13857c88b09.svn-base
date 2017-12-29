using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MessageBox : SingletonWindow<MessageBox>
{
    public enum Mode
    {
        None,
        Ok,                //1个按钮 
        OkCancel,          //2个按钮 
        OkCancelMiddle,    //3个按钮 
    };

    public enum Click
    {
        Ok,
        Cancel,
        Middle
    }

    private string m_text = string.Empty;
    private string m_title = string.Empty;
    private string m_sure = string.Empty;
    private string m_cancel = string.Empty;
    private string m_ok = string.Empty;

    private Action<object[]> m_handle_ok = null;
    private object[] m_handle_ok_args = null;
    private Action<object[]> m_handle_middle = null;
    private object[] m_handle_middle_args = null;
    private Action<object[]> m_handle_cancel = null;
    private object[] m_handle_cancel_args = null;
    private Action<object[]> m_handle_close = null;
    private object[] m_handle_close_args = null;

    private Mode m_mode = Mode.OkCancel;
    private int type = 0;

    private GameObject m_go_cancel = null;
    private GameObject m_go_ok = null;
    private GameObject m_go_ok_middle = null;
    private GameObject m_close = null;
    private Text m_lab_title = null;
    private Text m_lab_text = null;
    private Text m_lb_sure = null;
    private Text m_lb_cancel = null;
    private Text m_lb_ok = null;
    private Text m_lab_price = null;
    private GameObject m_disable_middle = null;

    public override string Bundle
    {
        get { return "MessageBox"; }
    }

    public Action<object[]> handleOk
    {
        set { m_handle_ok = value; }
    }

    public Action<object[]> handleMiddle
    {
        set { m_handle_middle = value; }
    }

    public Action<object[]> handleCancel
    {
        set { m_handle_cancel = value; }
    }

    public Action<object[]> handleClose
    {
        set { m_handle_close = value; }
    }

    public Mode mode
    {
        set { m_mode = value; //if (Done) Refresh();
            }
        get { return m_mode; }
    }

    public System.Action<Click> closeActive;

    public MessageBox() : base(LayerType.OverLayer) { }

    protected override void OnStart()
    {
        base.OnStart();

        Register("btn_sure").onClick = OnClickOk;
        Register("btn_cancel").onClick = OnClickCancel;
        Register("btn_sure2").onClick = OnClickMiddle;
        Register("btn_close").onClick = OnClickClose;

        m_go_cancel = Find("btn_cancel");
        m_go_ok = Find("btn_sure");
        m_go_ok_middle = Find("btn_sure2");
        m_close = Find("btn_close");
        m_disable_middle = Find("btn_sure3");
        m_lab_title = Find<Text>("lab_title");
        m_lab_text = Find<Text>("lab_text");
        m_lb_sure = Find<Text>("btn_sure/Text");
        m_lb_cancel = Find<Text>("btn_cancel/Text");
        m_lb_ok = Find<Text>("btn_sure2/Text");
        m_lab_price = Find<Text>("btn_sure3/Text");

        m_go_cancel.SetActive(false);
        m_go_ok.SetActive(false);
        m_go_ok_middle.SetActive(false);
        m_close.SetActive(false);
    }

    protected override void OnActive(bool active)
    {
        base.OnActive(active);
        if (active)
        {
            Refresh();
        }
    }

    public void Close(Click click)
    {
        base.Close();
        if(m_handle_close!=null)
        {
            m_handle_close(m_handle_close_args);
        }
        if (closeActive != null)
        {
            closeActive(click);
            closeActive = null;
        }
    }

    void OnClickClose(GameObject go)
    {
        //SoundManager.PlayBtnClickSnd();

        Close(Click.Cancel);

        if (m_handle_close != null)
        {
            m_handle_close(m_handle_close_args);
        }
    }

    void OnClickOk(GameObject go)
    {
        //SoundManager.PlayBtnClickSnd();

        Close(Click.Ok);

        if (m_handle_ok != null)
        {
            m_handle_ok(m_handle_ok_args);
        }
    }

    private void OnClickMiddle(GameObject go)
    {
        //SoundManager.PlayBtnClickSnd();

        Close(Click.Middle);

        if (m_handle_middle != null)
        {
            m_handle_middle(m_handle_middle_args);
        }
    }

    void OnClickCancel(GameObject go)
    {
        //SoundManager.PlayBtnClickSnd();

        Close(Click.Cancel);

        if (m_handle_cancel != null)
        {
            m_handle_cancel(m_handle_cancel_args);
        }
    }

    void Refresh()
    {
        if (m_mode == Mode.Ok)
        {
            m_go_cancel.SetActive(false);
            m_go_ok.SetActive(false);
            m_go_ok_middle.SetActive(true);
            m_close.SetActive(true);
        }
        else if (m_mode == Mode.OkCancel)
        {
            m_go_cancel.SetActive(true);
            m_go_ok.SetActive(true);
            m_go_ok_middle.SetActive(false);
            m_close.SetActive(true);
        }
        else if (m_mode == Mode.OkCancelMiddle)
        {
            m_go_cancel.SetActive(true);
            m_go_ok.SetActive(true);
            m_go_ok_middle.SetActive(true);
            m_close.SetActive(true);
        }
        else
        {
            m_go_cancel.SetActive(false);
            m_go_ok.SetActive(false);
            m_go_ok_middle.SetActive(false);
            m_close.SetActive(true);
        }

        m_lab_title.text = m_title;
        if (m_cancel != null && m_cancel != string.Empty)
            m_lb_cancel.text = m_cancel;
        if (m_ok != null && m_ok != string.Empty)
        {
            m_lab_price.text = m_ok;
            m_lb_ok.text = m_ok;
        }
        if (m_sure != null && m_sure != string.Empty)
            m_lb_sure.text = m_sure;

        m_lab_text.text = m_text;
        //(m_lab_text as TextPicture_UILabel).srcText = m_text;

        if (type != 0)
        {
            m_close.SetActive(false);
            m_disable_middle.SetActive(true);
            m_go_cancel.SetActive(false);
            m_go_ok.SetActive(false);
            m_go_ok_middle.SetActive(false);
        }
        else
        {
            m_disable_middle.SetActive(false);
        }
    }

    public static void Open(string title,string message)
    {
        message = message.Replace('^', '\n');
        Instance.m_title = title;
        Instance.m_text = message;
        Instance.mode = Mode.None;
        Instance.Open();
    }

    public delegate void AutoCloseCallBack(params object[] args);
    /// <summary>
    /// 支持自动关闭及其回调
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    /// <param name="time"></param>
    /// <param name="mode"></param>
    public static void Open(string title, string message, string lb_sure = null, string lb_ok = null, string lb_cancel = null, AutoCloseCallBack callback=null,object[] args = null, float time = 2f, Mode mode = Mode.None)
    {
        message = message.Replace('^', '\n');
        Instance.m_title = title;
        Instance.m_text = message;
        Instance.mode = mode;
        Instance.Open();
        Instance.m_sure = string.IsNullOrEmpty(lb_sure) ? "确定" : lb_sure;
        Instance.m_ok = string.IsNullOrEmpty(lb_sure) ? "忽略" : lb_ok;
        Instance.m_cancel = string.IsNullOrEmpty(lb_cancel) ? "取消" : lb_cancel;
        Util.StartCoroutine(AutoCloseDelayTime(time,callback, args));
    }

    static IEnumerator AutoCloseDelayTime(float time, AutoCloseCallBack callback = null, object[] args = null)
    {
        yield return new WaitForSeconds(time);
        if(callback!=null)
        {
            callback(args);
        }
        Instance.Close();
    }

    /// <summary>
    /// 支持3个按钮及其回调和关闭按钮及其回调
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="lb_sure"></param>
    /// <param name="lb_ok"></param>
    /// <param name="lb_cancel"></param>
    /// <param name="mode"></param>
    /// <param name="handleOk"></param>
    /// <param name="okArgs"></param>
    /// <param name="handleMiddle"></param>
    /// <param name="middleArgs"></param>
    /// <param name="handleCancel"></param>
    /// <param name="cancelArgs"></param>
    /// <param name="handleClose"></param>
    /// <param name="closeArgs"></param>
    public static void Open(string title, string message, string lb_sure = null, string lb_ok = null, string lb_cancel = null, Mode mode = Mode.OkCancel, Action<object[]> handleOk = null, object[] okArgs = null, Action<object[]> handleMiddle = null, object[] middleArgs = null, Action<object[]> handleCancel = null,object[] cancelArgs = null, Action<object[]> handleClose = null,object[] closeArgs = null)
    {
        message = message.Replace('^', '\n');
        Instance.m_title = title;
        Instance.m_text = message;
        Instance.mode = mode;
        Instance.handleOk = handleOk;
        Instance.m_handle_ok_args = okArgs;
        Instance.handleMiddle = handleMiddle;
        Instance.m_handle_middle_args = middleArgs;
        Instance.handleCancel = handleCancel;
        Instance.m_handle_cancel_args = cancelArgs;
        Instance.handleClose = handleClose;
        Instance.m_handle_close_args = closeArgs;

        Instance.m_sure = string.IsNullOrEmpty(lb_sure) ? "确定" : lb_sure;
        Instance.m_ok = string.IsNullOrEmpty(lb_sure) ? "忽略" : lb_ok;
        Instance.m_cancel = string.IsNullOrEmpty(lb_cancel) ? "取消" : lb_cancel;
        Instance.Open();
    }

}
