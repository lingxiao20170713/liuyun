using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyModal : BaseModal
{
    public LobbyView lobbyView;

    public LobbyModal() : base() { }

    public override void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        base.OnSceneLoaded(scene, loadMode);
        Debug.Log("大厅场景加载完成");
        if (lobbyView == null)
        {
            lobbyView = new LobbyView();
            lobbyView.Show();
            GamePublisher.Publish(EventType.lobby_music_open);
        }
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("大厅界面Update下按下空格键");
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        lobbyView.Destroy();
        lobbyView = null;
    }
}
