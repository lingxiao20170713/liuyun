
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MsgHistory11110 : NetMsg
{
    //发送参数

    //接收参数
    public byte Result { get; set; }
    
    public List<Record> recordList { get; set; }//解析步骤1

    public MsgHistory11110()
    {
        ProtocolId = (int)MsgType.GameHistory;
    }

    public override string Type()
    {
        return typeof(MsgHistory11110).Name;
    }

    public override void Read(ByteArray buf)
    {
        base.Read(buf);

        Result = buf.ReadByte();

        string str = buf.ReadString();
        Debug.Log(str);

        recordList = new List<Record>();
        recordList = fastJSON.JSON.ToObject<List<Record>>(str);
    }

    public override void Write(ByteArray buf)
    {
        base.Write(buf);
    }

}

public interface HisItem { }
public class Record
{
    public int gameEnum;
    public string record;
}

#region 南昌麻将解析方案
/// <summary>
/// 存放战绩项的外层包裹，为了使JSON字符串能够成功解析
/// </summary>
public class OuterItem: HisItem
{
    public List<HisPlayerInfo> player_info = new List<HisPlayerInfo>();
    public roomInfo room_info = new roomInfo();
    public List<RoundPlayRecordScore> round_play_record_scores = new List<RoundPlayRecordScore>();
    public int play_process_id;
}

public class HisPlayerInfo
{
    public string img_url;
    public string name;
    public long player_id;
    public int score;
}

public class roomInfo
{
    public int room_id;// 房间号
    public byte game_type;//0-经典玩法，1-德国玩法
    public byte round_type;//0-8局，1－16局
    public byte play_options;// 0-未选择，1-上下翻精，2-埋雷，3-同一首歌
    public byte hui_tou_yi_xiao;//  0-未选择，1-选择
    public byte only_zi_mo;//  0-未选择，1-选择
    public byte dian_pao;//0-一家付，1-三家付
    public long game_end_time;//  游戏结束时间
    public long owner_id;// 房主id
}
#endregion

#region 13水解析方案
public class WaterOuterItem: HisItem
{
    public List<WPlayerInfo> playerInfoList = new List<WPlayerInfo>();//玩家信息列表
    public DeskInfo deskInfo;
    public List<RoundRecordScoreInfo> roundRecordScoreList = new List<RoundRecordScoreInfo>();//每局积分列表
    public long playProcessId;

}

public class RoundRecordScoreInfo
{
    public List<int> playerScoreList = new List<int>();//积分列表
    public long playbackRecordID; // 战绩回放的id

}

public class WPlayerInfo
{
    public long playerId;//玩家id
    public string name;//玩家名字
    public string avatarUrl;//玩家头像 
    public int score;//玩家积分
}

public class DeskInfo
{
    public int deskId; // 房间号
    public int playerSum; //玩家人数
    public byte gunRule; //打枪规则
    public long ownerUserId; // 房主id
    public long gameEndTime; // 游戏结束时间
}
#endregion

#region 红中解析方案
public class HZMJOuterItem: HisItem
{
    public List<HzmjPlayerInfo> player_info = new List<HzmjPlayerInfo>();
    public HzmjRoomInfo room_info;
    public List<RoundPlayRecordScore> round_play_record_scores = new List<RoundPlayRecordScore>();
    public long play_process_id;
}

public class HzmjRoomInfo
{
    public int room_id; // 房间号
    public long owner_id;  // 房主id
    public byte round_type;  //  // 0: 0-8局， 1: 1－16局
    public byte mapaiType;  //  // 马牌类型  0 -> 不奖马 、  1 -> 奖1马 、  2 -> 奖2马  、 3 -> 奖4马  、4 -> 奖6马
    public long game_end_time;  //  游戏结束时间

}

public class HzmjPlayerInfo
{
    public long player_id;
    public string name;
    public string img_url;
    public int score;

}

public class RoundPlayRecordScore
{
    public List<int> playerScores = new List<int>();//积分列表
    public long playbackRecordID;  // 战绩回放的id
}
#endregion

#region 牛牛解析方案
public class NiuNiuOuterItem:HisItem
{
    public List<NnPlayerInfo> playerInfoList = new List<NnPlayerInfo>();
    public NnDeskInfo deskInfo;
    public List<RoundRecordScoreInfo> roundRecordScoreList = new List<RoundRecordScoreInfo>();
    public long playProcessId;
}

public class NnDeskInfo
{
    public int deskId;         // 房间号
    public int playerSum;      //玩家人数
    public long ownerUserId;   // 房主id
    public long gameEndTime;   //  游戏结束时间
}

public class NnPlayerInfo
{
    public long playerId;
    public string name;
    public string avatarUrl;
    public int score;
}

#endregion

#region 跑胡子解析方案
public class PhzOuterItem : HisItem
{
    public List<HzmjPlayerInfo> player_info = new List<HzmjPlayerInfo>();
    public HhRoomInfoBean room_info;
    public List<RoundPlayRecordScore> round_play_record_scores = new List<RoundPlayRecordScore>();
    private long play_process_id;
}

public class HhRoomInfoBean
{
    public int room_id; // 房间号
    public long owner_id;  // 房主id
    public byte round_type;  //回合数，0－6局2钻石，1－10局3钻石，2－16局4钻石
    public byte play_type;          //玩法，0－无醒，1-跟醒，2－翻醒
    public long game_end_time;  //  游戏结束时间
}
#endregion