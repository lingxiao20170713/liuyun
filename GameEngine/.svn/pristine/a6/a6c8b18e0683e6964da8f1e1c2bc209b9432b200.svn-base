//using System;
//using System.IO;
//using System.Collections.Generic;
//using ProtoBuf;

////using BaseShare.Net.Sockets;
////using BaseShare.Common.Log;
////using C2LMsgProto;

//namespace MsgProto.Net.Sockets
//{
//    using MSGID = System.UInt16;

//    public interface IMsgPaser
//    {
//        MsgDispatcher MsgDispatcher { get; }
//        bool ParseBuffer(MSGID msgId, byte[] buffer, int offset, int length, out Object msg, out Object preRtnObj);
//        byte[] ToBuffer(MSGID msgId, Object msg);
//        T DeserializeProtobuf<T>(byte[] buffer, int offset, int length) where T : class;
//        object DeserializeProtobuf(byte[] buffer, int offset, int length, System.Type type);
//        byte[] GetByteFromProtoBuf(object obj);
//        bool ParseMemoryStream(MSGID msgId, MemoryStream ms, out Object msg, out Object preRtnObj);
//    }

//    public class ProtobufPaser : IMsgPaser
//    {
//        private MsgDispatcher m_msgDispatcher;
//        private ILogger m_logger;

//        public ProtobufPaser(MsgDispatcher msgDispatcher, ILogger logger)
//        {
//            m_msgDispatcher = msgDispatcher;
//            m_logger = logger;
//        }

//        public MsgDispatcher MsgDispatcher { get { return m_msgDispatcher; } }

//        public bool ParseMemoryStream(MSGID msgId, MemoryStream ms, out Object msg, out Object preRtnObj)
//        {
//            preRtnObj = null;
//            msg = null;
//            MsgDispatcherInfo mdi = m_msgDispatcher.GetMsgDispatcherInfo(msgId);

//            if (mdi == null || mdi.m_protoType == null)
//                return false;

//            msg = DeserializeProtobufByMemoryStream(ms, mdi.m_protoType);

//            if (mdi.m_preHandler != null && msg != null)
//            {
//                preRtnObj = mdi.m_preHandler(msg, mdi.m_preObj);
//            }

//            return (msg != null);
//        }

//        public bool ParseBuffer(MSGID msgId, byte[] buffer, int offset, int length, out Object msg, out Object preRtnObj)
//        {
//            preRtnObj = null;
//            msg = null;
//            MsgDispatcherInfo mdi = m_msgDispatcher.GetMsgDispatcherInfo(msgId);

//            if (mdi == null || mdi.m_protoType == null)
//                return false;

//            //if (m_logger != null)
//            //    m_logger.Debug("ParseBuffer before Now:{0}", DateTime.Now);

//            msg = DeserializeProtobuf(buffer, offset, length, mdi.m_protoType);

//            if (mdi.m_preHandler != null && msg != null)
//            {
//                preRtnObj = mdi.m_preHandler(msg, mdi.m_preObj);
//            }

//            //if (m_logger != null)
//            //     m_logger.Debug("ParseBuffer after Now:{0}", DateTime.Now);

//            return (msg != null);
//        }

//        public byte[] ToBuffer(MSGID msgId, Object msg)
//        {
//            //if (m_logger != null)
//            //    m_logger.Debug("ToBuffer before Now:{0}", DateTime.Now);

//            if (msg == null)
//                return null;

//            byte[] rtn = GetByteFromProtoBuf(msg);

//            //if (m_logger != null)
//            //    m_logger.Debug("ToBuffer after Now:{0}", DateTime.Now);

//            return rtn;
//        }

//        public object DeserializeProtobufByMemoryStream(MemoryStream ms, System.Type type)
//        {
//            try
//            {
//#if IOS
//                ProtobufSerializer serializer = new ProtobufSerializer();
//                return serializer.Deserialize(ms, null, type);
//#else
//                return Serializer.NonGeneric.Deserialize(type, ms);
//#endif
//            }
//            catch (Exception ex)
//            {
//                if (m_logger != null)
//                    m_logger.Fatal("DeserializeProtobufByMemoryStream except occoured.Message:{0}, InnerException:{1}", ex.Message, ex.InnerException);
//                return null;
//            }
//        }

//        public T DeserializeProtobuf<T>(byte[] buffer, int offset, int length) where T : class
//        {
//            try
//            {
//                using (MemoryStream m = new MemoryStream(buffer, offset, length))
//                {
//#if IOS
//                    ProtobufSerializer serializer = new ProtobufSerializer();
//                    return serializer.Deserialize(m, null, typeof(T)) as T;
//#else
//                    return Serializer.NonGeneric.Deserialize(typeof(T), m) as T;
//#endif
//                }
//            }
//            catch (Exception ex)
//            {
//                if (m_logger != null)
//                    m_logger.Fatal("DeserializeProtobuf<T> except occoured.Message:{0}, InnerException:{1}", ex.Message, ex.InnerException);
//                return null;
//            }
//        }

//        public object DeserializeProtobuf(byte[] buffer, int offset, int length, System.Type type)
//        {
//            try
//            {
//                using (MemoryStream m = new MemoryStream(buffer, offset, length))
//                {
//#if IOS
//                    ProtobufSerializer serializer = new ProtobufSerializer();
//                    return serializer.Deserialize(m, null, type);
//#else
//                    return Serializer.NonGeneric.Deserialize(type, m);
//#endif
//                }
//            }
//            catch (Exception ex)
//            {
//                if (m_logger != null)
//                    m_logger.Fatal("DeserializeProtobuf except occoured.Message:{0}, InnerException:{1}", ex.Message, ex.InnerException);
//                return null;
//            }
//        }

//        public byte[] GetByteFromProtoBuf(object obj)
//        {
//            byte[] buffer = null;

//            try
//            {
//                using (MemoryStream m = new MemoryStream())
//                {
//#if IOS
//                    ProtobufSerializer serializer = new ProtobufSerializer();
//                    serializer.Serialize(m, obj);
//                    //m.Position = 0;
//                    //int length = (int)m.Length;
//                    //buffer = new byte[length];
//                    //m.Read(buffer, 0, length);
//#else
//                    Serializer.NonGeneric.Serialize(m, obj);
//#endif
//                    buffer = m.ToArray();
//                }
//            }
//            catch (Exception ex)
//            {
//                if (m_logger != null)
//                    m_logger.Fatal("GetByteFromProtoBuf except occoured.Message:{0}, InnerException:{1}", ex.Message, ex.InnerException);
//                return null;
//            }

//            return buffer;
//        }
//    }
//}
