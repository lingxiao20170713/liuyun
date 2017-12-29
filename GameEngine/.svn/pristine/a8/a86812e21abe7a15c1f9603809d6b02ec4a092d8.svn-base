using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : SingletonWindow<Dialog>
{
    ReceiveDialog m_receiveDlg = null;
    PauseDialog m_pauseDlg = null;

    public Dialog() : base(LayerType.Layer_3) { }

    public override string Bundle
    {
        get
        {
            return "Dialog";
        }
    }

    public ReceiveDialog reviveDlg
    {
        get { return m_receiveDlg; }
        set { m_receiveDlg = value; }
    }

    public PauseDialog pauseDlg
    {
        get { return m_pauseDlg; }
        set { m_pauseDlg = value; }
    }

    protected override void OnStart()
    {
        base.OnStart();

        Debug.Log("dialog init...");
        m_receiveDlg = new ReceiveDialog(Find("ReceiveDialog"));
        m_pauseDlg = new PauseDialog(Find("PauseDialog"));
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public class ReceiveDialog : DialogBase
    {
        Text btn_title;
        Image[] items = new Image[4];
        public ReceiveDialog(GameObject go) : base(go)
        {
            Register("GetBtn").onClick = OnGet;
            Register("CloseBtn").onClick = OnClose;

            btn_title = Find("GetBtn/Text").GetComponent<Text>();
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = Find("ItemsList/Item" + (i + 1)).GetComponent<Image>();
            }
        }

        void OnGet(GameObject go)
        {
            Debug.Log("点击");
        }

        void OnClose(GameObject go)
        {
            Close();
        }

        protected override void Refresh()
        {
            base.Refresh();
        }

        public override void Open()
        {
            base.Open();

            string[] titles = new string[] { "领  取","购  买","确  定"};
            btn_title.text = titles[UnityEngine.Random.Range(0, 3)];
            int item_count = UnityEngine.Random.Range(1,5);
            for (int i = 0; i < items.Length; i++)
            {
                items[i].gameObject.SetActive(i < item_count);
                items[i].sprite = Resources.Load<Sprite>("Item/icon_item_"+UnityEngine.Random.Range(231,259));
                items[i].transform.Find("Text").GetComponent<Text>().text = string.Format("x{0}", UnityEngine.Random.Range(1, 100));
            }
        }

        public override void Close()
        {
            base.Close();
        }
    }

    public class PauseDialog : DialogBase
    {
        public PauseDialog(GameObject go) : base(go)
        {
            Register("CloseBtn").onClick = OnClose;
        }

        void OnClose(GameObject go)
        {
            Close();
        }

        public override void Open()
        {
            base.Open();
        }

        protected override void Refresh()
        {
            base.Refresh();
        }

        public override void Close()
        {
            base.Close();
        }
    }
}
