using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHook : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoginAck(NetMsg msg)
    {
        Debug.Log("MsgLogin10001返回");
        MsgLogin10001 data = (MsgLogin10001)msg;
        Debug.Log("登录结果："+data.Code);
        if (data.Code == 0)
        {
            Debug.Log("登录成功:"+ data.Uid + "||" + data.NickName + "||" + data.cityCode);
            Player player = new Player();
            player.nickName = data.NickName;
            long out_id = 0;
            if(long.TryParse(data.Uid, out out_id))
            {
                player.Id = out_id;
            }
            //player.sex = data.Sex;
            //player.score = data.game_channel_id;
            //player.icon = data.AvatarURL;
            //PlayerManager.Instance.player = player;

            StageManager.Instance.SetNextStage(StageEnum.LOBBY);
        }
        else if(data.Code == 1)
        {
            MessageBox.Open("错误","账户或者密码错误");
        }
        else if(data.Code == 2)
        {
            Debug.Log("参数错误");
        }
        else if(data.Code == 3)
        {
            Debug.Log("登录时间失效");
        }
        else if(data.Code == 4)
        {
            Debug.Log("登录token失效");
        }
        else
        {
            Debug.Log("登录失败");
        }
    }

    void HistoryAck(NetMsg msg)
    {
        Debug.Log("MsgHistory11110返回");
        MsgHistory11110 data = (MsgHistory11110)msg;
        Debug.Log(data.Result);
        if (data.Result == 0)
        {
            BattleControler.Instance.battleModal.battleView.gameHistoryPanel.gameObject.SetActive(true);
            GameHistory.Instance.InitList(data.recordList);
        }
    }

    void HistoryRecordAck(NetMsg msg)
    {
        Debug.Log("MsgRecordPlay11111返回");
        MsgRecordPlay11111 data = (MsgRecordPlay11111)msg;
        Debug.Log(data.Result);
        if (data.Result == 0)
        {
            BattleControler.Instance.battleModal.battleView.gameHistoryPanel.gameObject.SetActive(true);
            GameHistory.Instance.GetRecordDetailAck(data);
        }
    }
}
