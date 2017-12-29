using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgRecordPlay11111 : NetMsg {

    //C->S
    public long RecordID { get; set; }

    //S->C
    public byte Result { get; set; }
	public string RecordDetail{ get; set; }

    public MsgRecordPlay11111()
    {
        ProtocolId = (int)MsgType.HistoryRecord;
    }

    public override string Type()
    {
        return typeof(MsgRecordPlay11111).Name;
    }

    public override void Read(ByteArray buf)
    {
        base.Read(buf);
        Result = buf.ReadByte();

		RecordDetail = buf.ReadString();
		Debug.Log(RecordDetail);
        

    }

    public override void Write(ByteArray buf)
    {
        base.Write(buf);

        buf.WriteLong(RecordID);
    }
}
