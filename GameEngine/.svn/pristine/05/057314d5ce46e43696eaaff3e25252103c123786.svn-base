using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocol
{
    private static Dictionary<int, string> protocols = Init();

    private static Dictionary<int, string> Init()
    {
        protocols = new Dictionary<int, string>();

        protocols[(int)MsgType.LOGIN] = typeof(MsgLogin10001).Name;
        protocols[(int)MsgType.GameHistory] = typeof(MsgHistory11110).Name;
        protocols[(int)MsgType.HistoryRecord] = typeof(MsgRecordPlay11111).Name;

        return protocols;
    }

    public static string GetProtocolType(int protocolId)
    {
        if (protocols.ContainsKey(protocolId))
        {
            return protocols[protocolId];
        }
        return "无定义协议号不处理";
    }
}

public enum MsgType
{
    None=0,
    LOGIN = 10001,
    LOBBY = 10002,
    BATTLE = 10003,

    GameHistory = 11110,
    HistoryRecord = 11111
}