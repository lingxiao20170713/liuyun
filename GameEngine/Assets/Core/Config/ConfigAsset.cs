/********************************************************************
 * Copyright (C) 2011 广州网络科技有限公司
 * 版权所有
 * 
 * 文件名：ConfigAsset.cs
 * 文件功能描述：配置表序列化
 * 
 * 创建者：
 * 创建日期：2014/07/23
 * 
 *******************************************************************/
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ConfigAsset : ScriptableObject
{
    [SerializeField]
    private byte[] stream;

    public void Serialize(ConfigData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream serializationStream = new MemoryStream();
        formatter.Serialize(serializationStream, data);
        this.stream = serializationStream.ToArray();
    }

    public ConfigData Deserialize()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream serializationStream = new MemoryStream(this.stream);
        return formatter.Deserialize(serializationStream) as ConfigData;
    }
}


