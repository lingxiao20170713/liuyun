using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMDAskHandle
{
    private static CMDAskHandle _instance;
    public static CMDAskHandle Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CMDAskHandle();
            }
            return _instance;
        }
    }

    public void LoginAsk(string account,string password)
    {
        MsgLogin10001.guestLogin = false;
        MsgLogin10001 msg = new MsgLogin10001();
        msg.UidSend = account;
        msg.TokenSend = password;
        msg.TypeSend = 1;
        msg.game_channel_id = 0;
        Client.Instance.SendMessage(msg);
    }

    public void GetRecordAsk()
    {
        MsgHistory11110 msg = new MsgHistory11110();
        Client.Instance.SendMessage(msg);
    }
    public void GetRecordDetailAsk(long id)
    {
        MsgRecordPlay11111 msg = new MsgRecordPlay11111();
        msg.RecordID = id;
        Client.Instance.SendMessage(msg);
    }
}
