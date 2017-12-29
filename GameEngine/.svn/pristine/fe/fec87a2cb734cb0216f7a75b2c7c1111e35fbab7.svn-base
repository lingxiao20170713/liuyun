using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CertificationPanel : SingletonWindow<CertificationPanel> {

    Button btnClose;
    Image imgMask;

    Button btnConfirm;
    InputField inputName;
    InputField inputId;

    public CertificationPanel() : base(LayerType.Layer_1) { }

    public override string Bundle
    {
        get
        {
            return "CertificationPanel";
        }
    }

    protected override void OnStart()
    {
        base.OnStart();

        btnClose = Find("BtnClose").GetComponent<Button>();
        imgMask = Find("Mask").GetComponent<Image>();
        btnConfirm = Find("BtnConfirm").GetComponent<Button>();
        inputName = Find("InputName").GetComponent<InputField>();
        inputId = Find("InputID").GetComponent<InputField>();

        this.Register(btnClose.gameObject).onClick = OnClick;
        this.Register(imgMask.gameObject).onClick = OnClick;
        this.Register(btnConfirm.gameObject).onClick = OnClick;
    }

    void OnClick(GameObject go)
    {
        if (go == btnClose.gameObject || go == imgMask.gameObject)
        {
            Close();
        }
        if (go == btnConfirm.gameObject)
        {
            ValidateInput();
        }

    }

    void ValidateInput()
    {
        string name = inputName.text;
        string id = inputId.text;

        if (name == "")
        {
            //GlobalAPI.ToastManager.showAutoDismissToast("请输入姓名");
            return;
        }

        if (id == "")
        {
            //GlobalAPI.ToastManager.showAutoDismissToast("请输入身份证号");
            return;
        }

        if (!Regex.IsMatch(id, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
        {
            //GlobalAPI.ToastManager.showAutoDismissToast("请输入正确的身份证号");
            return;
        }

        Close();
        inputName.text = null;
        inputId.text = null;

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
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
