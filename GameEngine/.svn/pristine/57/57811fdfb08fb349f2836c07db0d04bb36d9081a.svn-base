using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginView : BaseView
{
    private GameObject LoginTrans;
    private InputField account;
    private InputField password;
    private InputField IP;

    private Button okBtn;
    private Button cancelBtn;
    public LoginView(string assetpath ="") : base(assetpath) { }

    public override int GetModuleType()
    {
        return (int)ModuleType.LOGIN_MAIN;
    }

    public override void Init()
    {
        base.Init();

        LoginTrans = GameObject.Find("Canvas/WndLogin");
        account = LoginTrans.transform.Find("Account").GetComponent<InputField>();
        password = LoginTrans.transform.Find("Password").GetComponent<InputField>();
        IP = LoginTrans.transform.Find("IP").GetComponent<InputField>();
        okBtn = LoginTrans.transform.Find("Button_Ready").GetComponent<Button>();
        cancelBtn = LoginTrans.transform.Find("Button_Cancel").GetComponent<Button>();

        UIEventListener.Get(okBtn.gameObject).onClick = OnClick;
        UIEventListener.Get(cancelBtn.gameObject).onClick = OnClick;

        string accoutSaved = CacheManager.GetString(CacheManager.ACCOUNT);
        string passwordSaved = CacheManager.GetString(CacheManager.PASSWORD);

        account.text = accoutSaved;
        password.text = passwordSaved;
    }

    protected override void LoadPanel(string assetPath, Transform parent)
    {
        base.LoadPanel(assetPath, parent);
    }

    void OnClick(GameObject go)
    {
        if(go == okBtn.gameObject)
        {
            Debug.Log("账号：" + account.text + "\n密码：" + password.text);

            CacheManager.SetString(CacheManager.ACCOUNT, account.text);
            CacheManager.SetString(CacheManager.PASSWORD, password.text);
            CacheManager.Save();

            //CMDAskHandle.Instance.LoginAsk(account.text, password.text);
            StageManager.Instance.SetNextStage(StageEnum.LOBBY);
        }
        if(go == cancelBtn.gameObject)
        {
            //Application.Quit();
            CacheManager.Clear();
        }
    }

    public override GameObject ViewObject()
    {
        return LoginTrans;
    }
    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
