using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMsg : INetMsg
{
    public ushort Len = 0;             //协议长度，这里只是占位，发送的时候要重写
    public ushort ProtocolId = 0;      //协议号

    public virtual string Type()
    {
        return typeof(NetMsg).Name;
    }

    public virtual void Read(ByteArray buf)
    {
        Len = (ushort)buf.ReadShort();
        ProtocolId = (ushort)buf.ReadShort();
    }

    public virtual void Write(ByteArray buf)
    {
        buf.WriteShort((ushort)Len);
        buf.WriteShort((ushort)ProtocolId);
    }
}
