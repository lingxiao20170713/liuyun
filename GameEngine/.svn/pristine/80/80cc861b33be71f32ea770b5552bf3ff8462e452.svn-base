using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Client
{
    private const int MAX_NET_BUFFER_SIZE = 1048560;
    private byte[] mRecvBuff = new byte[MAX_NET_BUFFER_SIZE];

    private Socket mSocket;
    private Queue<ByteArray> mRecvQueue;
    private Queue<ByteArray> mSendQueue;
    private bool isSending = false;//是否正在发送
    private int mRecvPosition = 0;
    public Action<bool> connectedCall;//回调

    private Dictionary<string, Type> p_types = new Dictionary<string, Type>();

    public static Client Instance;

    public Client()
    {
        mRecvQueue = new Queue<ByteArray>();
        mSendQueue = new Queue<ByteArray>();
        Instance = this;
    }

    int handleCount = 0;
    public void Update()
    {
        if(mSocket == null || !mSocket.Connected)
        {
            return;
        }

        handleCount = mRecvQueue.Count;

        if(Time.deltaTime >0.05)//如果游戏帧率减低到20以下，则网络包一次过处理完，防止累积太多
        {
            while (handleCount > 0)
            {
                DecodeByteArray(RecvMsg());
                handleCount--;
            }
        }
        else
        {
            DecodeByteArray(RecvMsg());
        }

        SendMsg();
    }

    private void DecodeByteArray(ByteArray byteArray)//解包
    {
        if (byteArray != null)
        {
            byteArray.Postion = 0;
            byteArray.ReadUShort();//读取length
            var protoId = byteArray.ReadUShort();
            var typeName = Protocol.GetProtocolType(protoId);

            Type clazz = null;
            p_types.TryGetValue(typeName, out clazz);
            if (clazz == null)
            {
                clazz = Type.GetType(typeName);
                p_types[typeName] = clazz;
            }
            Debug.Log("返回协议类型,typeName = " + typeName + " protoId = " + protoId);
            if (clazz == null)
            {
                Debug.LogError("找不到协议类型,typeName = " + typeName + " protoId = " + protoId);
                return;
            }
            var message = clazz.Assembly.CreateInstance(typeName) as NetMsg;

            byteArray.Postion = 0;
            message.Read(byteArray);

            #region 消息处理模块
            switch (message.ProtocolId)
            {
                case (int)MsgType.LOGIN:
                    {
                        GameManager.Instance.SendMessage("LoginAck", message);
                    }
                    break;
                case (int)MsgType.LOBBY:
                    {

                    }
                    break;
                case (int)MsgType.BATTLE:
                    {

                    }
                    break;
                case (int)MsgType.GameHistory:
                    {
                        GameManager.Instance.SendMessage("HistoryAck", message);
                    }
                    break;
                case (int)MsgType.HistoryRecord:
                    {
                        GameManager.Instance.SendMessage("HistoryRecordAck", message);
                    }
                    break;
                default:
                    break;
            }
            #endregion

            //if (HasEventListener(message.Type()))
            //{
            //    byteArray.Postion = 0;
            //    message.Read(byteArray);
            //    Debug.Log("socket receive===消息协议======ID====:" + message.ProtocolId);
            //    try
            //    {
            //        Dispatch<NetMsg>(message.Type(), message);  //分发接收到的网络消息
            //    }
            //    catch (System.Exception ex)
            //    {
            //        Logger.LogErrorFormat("[Error] NetSocketManager dispatch error ex = {0},stack = {1}", ex.Message, ex.StackTrace);
            //    }
            //}

            //this.DispatchEvent(MESSAGE_RECEIVE, this, message);
            //NetConnectionManager.Instance.RemoveEvent(message);
        }
    }

    public bool Connect(string ip,int port)// 同步连接服务器
    {
        if(mSocket!=null)
        {
            DisConnect();
        }
        mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        if (mSocket == null)
        {
            connectedCall(false);
            return false;
        }
        try
        {
            mSocket.Connect(ip, port);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message + "错误异常：" + exception.StackTrace);
            connectedCall(false);
            return false;
        }
        mRecvPosition = 0;
        if (!RecvMessageFromSocket())
        {
            DisConnect();
            connectedCall(false);
            return false;
        }
        connectedCall(true);
        return true;
    }


    public void BeginConnect(string ip,int port)// 异步连接服务器
    {
        isSending = false;

        if (mSocket!=null)
        {
            DisConnect();
        }

        AddressFamily mIpAddressFamily = AddressFamily.InterNetwork;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

            string newServerIp = "";
            string portString = Convert.ToString(port);
            getIPType(ip, portString, out newServerIp, out mIpAddressFamily);
            if (!string.IsNullOrEmpty(newServerIp))
            {
                ip = newServerIp;
            }
        }
        mSocket = new Socket(mIpAddressFamily, SocketType.Stream, ProtocolType.Tcp);
        Debug.Log("-------Socket服务器地址:" + ip + "------");

        IAsyncResult result = null;
        try
        {
            result = mSocket.BeginConnect(ip, port, null, null);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "错误异常：" + e.StackTrace);
            connectedCall(false);
            return;
        }

        //异步只能用这种方式，才能在主线程里面继续执行，因为connectedCall函数的回调可能会有对GameObject的操作
        bool success = result.AsyncWaitHandle.WaitOne(3500, true);
        if (!success || !mSocket.Connected)
        {
            mSocket.Close();
            connectedCall(false);
            return;
        }

        mSocket.EndConnect(result);
        mRecvPosition = 0;
        if (!RecvMessageFromSocket())
        {
            DisConnect();
            connectedCall(false);
        }
        connectedCall(true);
    }

    public void DisConnect()
    {
        isSending = false;

        try
        {
            mSocket.Disconnect(true);
        }
        catch (Exception exception)
        {
            Debug.LogError("断开网络连接发生错误：" + exception.StackTrace);
        }
        mSocket = null;
        mRecvPosition = 0;
    }

    public bool isConnected()
    {
        return mSocket != null && mSocket.Connected;
    }

    private bool RecvMessageFromSocket()
    {
        if (mSocket != null)
        {
            try
            {
                SocketError socketError;
                mSocket.BeginReceive(
                    mRecvBuff,
                    mRecvPosition,
                    this.mRecvBuff.Length - this.mRecvPosition,
                    SocketFlags.None,
                    out socketError,
                    new AsyncCallback(RecvCallBack), this);
            }
            catch (Exception exception)
            {
                Debug.LogError("接受Socket数据发生错误：" + exception.StackTrace);
                return false;
            }
            return true;
        }
        return false;
    }

    private void RecvCallBack(IAsyncResult ar)
    {
        int num = 0;
        try
        {
            /*
                * EndReceive 方法将一直阻止到有数据可用为止。 如果您使用的是无连接协议，
                * 则 EndReceive 将读取传入网络缓冲区中第一个排队的可用数据报。 
                * 如果您使用的是面向连接的协议，则 EndReceive 方法将读取所有可用的数据，
                * 直到达到 BeginReceive 方法的 size 参数所指定的字节数为止。 
                * 如果远程主机使用 Shutdown 方法关闭了 Socket 连接，并且所有可用数据均已收到，
                * 则 EndReceive 方法将立即完成并返回零字节。
                */
            SocketError socketError;
            num = mSocket.EndReceive(ar, out socketError);

            Debug.Log("接收到协议返回 长度为： " + num.ToString());
        }
        catch (Exception exception)
        {

            Debug.LogError(exception.Message + "接受Socket数据回调时发生错误：" + exception.StackTrace);
            return;
        }
        if (num == 0)
        {
            Debug.Log("接受Socket数据num=0：DisConnect()");
            DisConnect();
        }
        else
        {
            mRecvPosition += num;
            while (true)
            {
                if (mSocket==null || !mSocket.Connected)
                {
                    break;
                }
                ByteArray byteArray = new ByteArray();
                if (!this.ParseMessage(ref this.mRecvBuff, ref this.mRecvPosition, ref byteArray))
                {
                    break;
                }
                lock (mRecvQueue)
                {
                    mRecvQueue.Enqueue(byteArray);
                }
            }
            RecvMessageFromSocket();
        }
    }

    private bool ParseMessage(ref byte[] buff, ref int len, ref ByteArray byteArray)
    {
        //内容长度2个字节 协议号 2个字节 如果当前BUFF总长度不够4个字节，就等待下次再读取
        if (len < 4)
        {
            return false;
        }

        byte[] byteTemp = new byte[2];
        Array.Copy(buff, 0, byteTemp, 0, 2);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(byteTemp);
        }
        int size = BitConverter.ToUInt16(byteTemp, 0) + 2; //一条消息协议数据包的长度

        if (size < 2 || size > ushort.MaxValue)
        {
            DisConnect();
            Debug.LogError("数据包大小过长或者较短导致断开连接size=" + size);
            return false;
        }
        if (len < size) //出现粘包情况了，也就是还有一部分数据没有读取到，需要等下次一起读取
        {
            return false;
        }
        bool result = byteArray.CreateFromSocketBuff(buff, size);
        RemoveRecvBuff(ref buff, ref len, size);
        return result;
    }

    private void RemoveRecvBuff(ref byte[] buff, ref int nLen, int nSize)
    {
        if (nSize <= nLen)
        {
            byte[] destinationArray = new byte[nLen - nSize];
            Array.Copy(buff, nSize, destinationArray, 0, nLen - nSize);
            Array.Clear(buff, 0, buff.Length);
            Array.Copy(destinationArray, 0, buff, 0, nLen - nSize);
            nLen -= nSize;
        }
    }

    private void getIPType(string serverIp, string serverPorts, out string newServerIp, out AddressFamily mIPType)
    {
        mIPType = AddressFamily.InterNetwork;
        newServerIp = serverIp;
        try
        {
            string mIpv6 = IOSBridgeManager.GetIPv6(serverIp, serverPorts);

            if (!string.IsNullOrEmpty(mIpv6))
            {
                string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIpv6, "&&");
                if (m_StrTemp != null && m_StrTemp.Length >= 2)
                {
                    string IPType = m_StrTemp[1];
                    if (IPType == "ipv6")
                    {
                        newServerIp = m_StrTemp[0];
                        mIPType = AddressFamily.InterNetworkV6;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("ipv6: " + e.Message);
        }
    }

    public bool Send(NetMsg message)
    {
        if (mSocket == null || !mSocket.Connected || message == null)
        {
            return false;
        }
        ByteArray body = new ByteArray();
        message.Write(body);
        ushort len = (ushort)body.Length;
        byte[] lenBytes = BitConverter.GetBytes((ushort)(len - 2));
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(lenBytes);//倒序
        }
        //由于后端把本身的长度2个字节在基类中已经写入，需要获取新的长度覆盖，NetMsg里面的len被覆盖。
        Array.Copy(lenBytes, body.Buff, 2);

        lock (mSendQueue)
        {
            mSendQueue.Enqueue(body);
        }

        return mSocket.Connected;
    }

    public ByteArray RecvMsg()
    {
        lock (mRecvQueue)
        {
            if (mSocket == null)
            {
                return null;
            }
            if (mRecvQueue.Count == 0)
            {
                return null;
            }
            ByteArray msg = mRecvQueue.Dequeue();
            return msg;
        }
    }

    public void SendMsg()
    {
        if (this.isSending || mSendQueue.Count == 0)
            return;

        ByteArray body = null;
        lock (mSendQueue)
        {
            if (mSendQueue.Count > 0)
                body = mSendQueue.Dequeue();

            if (body == null)
                return;

            try
            {
                SocketError error;
                isSending = true;
                mSocket.BeginSend(body.Buff, 0, body.Length, SocketFlags.None, out error, new AsyncCallback(SendCallback), this);
            }
            catch (Exception exception)
            {
                isSending = false;
                Debug.LogError("发送Socket数据发生错误：" + exception.StackTrace);
            }
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        isSending = false;
        try
        {
            SocketError error;
            mSocket.EndSend(ar, out error);
        }
        catch (Exception exception)
        {
            Debug.LogError("发送Socket数据回调发生错误：" + exception.StackTrace);
        }
    }

    public void SendMessage(NetMsg msg)
    {
        Debug.Log("发送消息：" + msg.Type());
        Send(msg);
    }
}
