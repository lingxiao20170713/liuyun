
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MsgLogin10001 : NetMsg
{
    public static bool guestLogin = false;

    //发送参数
    /// 1--uid/token登录 2--微信授权登录 3--游客登录
    public int TypeSend { get; set; }//	   
    public string WXCodeSend { get; set; }
    public string UidSend { get; set; }
    public string TokenSend { get; set; }
    public byte platform; //            1--Android 2--iOS
    public int cityCode { get; set; }//麻将区域编码 游戏类型
    public int game_channel_id { get; set; }//渠道id+产品id

    //接收参数
    public int Code { get; set; }//0成功 1账户不存在 2 参数错误 3  登录时间失效  4 登录token失效
    public string Msg { get; set; }

    public string Uid { get; set; }
    public string Token { get; set; }
    public string AvatarURL { get; set; }
    public string NickName { get; set; }
    public int Sex { get; set; }
    public int RoomCards { get; set; }
    public string IPAddress { get; set; }
	public string IMToken {get;set;}
    public int gameChannelid { get; set; }

    public MsgLogin10001()
    {
        ProtocolId = (int)MsgType.LOGIN;
        WXCodeSend = @"";
        TokenSend = @"";
        UidSend = @"";
    }

    public override string Type()
    {
        return typeof(MsgLogin10001).Name;
    }

    public override void Read(ByteArray buf)
    {
        base.Read(buf);
        Code = buf.ReadInt();
        Msg = buf.ReadString();
        Debug.Log(Msg);

        Uid = buf.ReadString();
        Token = buf.ReadString();
        AvatarURL = buf.ReadString();
        NickName = buf.ReadString();
        Sex = buf.ReadInt();
        RoomCards = buf.ReadInt();
        IPAddress = buf.ReadString();

		IMToken = buf.ReadString ();
        gameChannelid = buf.ReadInt();
        PlayerPrefs.SetInt("GameChannelId", gameChannelid);
    }

    public override void Write(ByteArray buf)
    {
        base.Write(buf);
        buf.WriteInt(TypeSend);
        buf.WriteString(WXCodeSend);
        buf.WriteString(UidSend);
        buf.WriteString(TokenSend);
        if (Application.platform == RuntimePlatform.Android)
        {

            platform = 1;
        }
        else
        {

            platform = 2;
        }
        buf.WriteByte(platform);
		cityCode = (int)Game_City.FuJian;
        PlayerPrefs.SetInt("CityCode", cityCode);//保存城市编号
        buf.WriteInt(cityCode);
        if(game_channel_id == 0)
        {
            game_channel_id = PlayerPrefs.GetInt("GameChannelId");
        }
        buf.WriteInt(game_channel_id);
    }


}
///城市分类
///登录协议参数
public enum Game_City
{
    None = 0,
    NanChang = 10100101,
    FuZhou = 3402,
	FuJian = 20200101,
    RedCenter = 10300101,
	CowCow = 20200102,


    #region Paohuzi
    //永州扯胡子  //3人
    Hhchz = 30300101, 
    //衡阳六胡抢   //四人
    Hhlhq = 30300201
    #endregion
}