using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginModal : BaseModal
{
    private LoginView loginView = null;
    public LoginModal() : base() { }

    public override void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        base.OnSceneLoaded(scene, loadMode);
        if (loginView == null)
        {
            loginView = new LoginView();
            loginView.Show();
        }
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("登录界面Update下按下空格键");
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        loginView.Destroy();
        loginView = null;
    }
}
