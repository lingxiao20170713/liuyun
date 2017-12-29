using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxPanel : SingletonWindow<MailBoxPanel>
{
    private Button confirmBtn;
    private Button cancelBtn;

    public MailBoxPanel() : base(LayerType.Layer_3) { }

    public override string Bundle
    {
        get
        {
            return "MailBoxPanel";
        }
    }
    protected override void OnStart()
    {
        base.OnStart();

        confirmBtn = Find("Button_Ready").GetComponent<Button>();
        cancelBtn = Find("Button_Cancel").GetComponent<Button>();

        this.Register(confirmBtn.gameObject).onClick = OnClick;
        this.Register(cancelBtn.gameObject).onClick = OnClick;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    void OnClick(GameObject go)
    {
        if(go == confirmBtn.gameObject)
        {
            Debug.Log("确定");
        }
        if(go == cancelBtn.gameObject)
        {
            Close();
        }
    }

    protected override void OnActive(bool active)
    {
        base.OnActive(active);
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
}
