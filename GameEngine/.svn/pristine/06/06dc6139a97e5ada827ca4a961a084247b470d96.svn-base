using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleModal : BaseModal
{
    public BattleView battleView;
    public BattleModal():base()
    {

    }

    public override void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        base.OnSceneLoaded(scene, loadMode);
        Debug.Log("战斗场景加载完成");
        if (battleView == null)
        {
            battleView = new BattleView();
            battleView.Show();
            GamePublisher.Publish(EventType.battle_music_open);
        }

    }



    public override void Update()
    {
        base.Update();
        if (battleView == null) return;
        if (battleView!=null && !battleView.people.moving) return;

        //Debug.Log(Vector3.Distance(battleView.people.transform.localPosition, battleView.gameOjbet_tran[0].gameObject.transform.localPosition));

        //for (int i = 0; i < battleView.gameOjbet_tran.Length; i++)
        //{
        //    GameObject go = battleView.gameOjbet_tran[i].gameObject;
        //    Debug.Log(Vector3.Distance(battleView.people.transform.localPosition, go.transform.localPosition));
        //    if(Vector3.Distance(battleView.people.transform.localPosition,go.transform.localPosition)<5)
        //    {
        //        go.SetActive(false);
        //    }
        //    //else
        //    //{
        //    //    go.SetActive(true);
        //    //}
        //}
    }

    public override void Destroy()
    {
        base.Destroy();
        battleView.Destroy();
        battleView = null;
    }
}
