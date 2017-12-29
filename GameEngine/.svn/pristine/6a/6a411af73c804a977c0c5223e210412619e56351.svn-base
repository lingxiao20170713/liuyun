/********************************************************************
 * Copyright (C) 2011 广州网络科技有限公司
 * 版权所有
 * 
 * 文件名：ConfigData.cs
 * 文件功能描述：配置数据表类
 * 
 * 创建者：
 * 创建日期：2014/07/18
 * 
 *******************************************************************/

using System;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;

public class ConfigDataType
{
	public const int UNKNOWN = 0; // 未知的作为string处理
	public const int INT = 1;
	public const int BOOL = 2;
	public const int FLOAT = 3;
	public const int LONG = 4;
	public const int DOUBLE = 5;
	public const int STRING = 6;
	public const int ENUM = 7;
	public const int ARRAY = 8; // 数组，以;分隔
    public const int JSON = 9; // JSON格式的string
	public const int CUSTOM = 10; // 自定义类型
	public const int DATETIME = 11; // 日期类型，格式 yyyy-mm-dd hh:mm:ss
}

[Serializable]
public class ConfigData
{
    public ConfigSheetLine[] data;
    public string[] fieldNames; // 字段名称列表
    public int[] fieldTypes; // 字段类型列表
    //public byte FileType; // 配置表类型：1-制表符分割的csv文件，2-xml文件
    //public string xmlText;

    public ConfigSheetLine GetData(int id)
    {
        for (int i = 0; i < this.data.Length; ++i)
        {
            ConfigSheetLine line = this.data[i];
            if (((int) line.GetData(0)) == id)
            {
                return line;
            }
        }
        return null;
    }
        
    public object GetData(int id, int field)
    {
        ConfigSheetLine data = GetData(id);
        if (data != null)
        {
            return data.GetData(field);
        }
        return null;
    }

    public object GetData(int id, string fieldName)
    {
        int field = GetFieldByName(fieldName);
        if (field >= 0)
        {
            return GetData(id, field);
        }
        return null;
    }

    public ConfigSheetLine GetDataByRow(int row)
    {
        Debug.Assert(row >= 0 && row < this.data.Length);
            
        return this.data[row];
    }

    public object GetDataByRow(int row, int field)
    {
        ConfigSheetLine dataByRow = GetDataByRow(row);
        if (dataByRow != null)
        {
            return dataByRow.GetData(field);
        }
        return null;
    }

    public object GetDataByRow(int row, string fieldName)
    {
        ConfigSheetLine dataByRow = GetDataByRow(row);
        if (dataByRow != null)
        {
            int field = GetFieldByName(fieldName);
            if (field >= 0)
            {
                return dataByRow.GetData(field);
            }
        }
        return null;
    }

    public int GetDataType(int field)
    {
        Debug.Assert(field >= 0 && field < this.fieldTypes.Length);

        return this.fieldTypes[field];
    }

    public int GetFieldByName(string fieldName)
    {
        for (int i = 0; i < this.fieldNames.Length; i++)
        {
            if (this.fieldNames[i] == fieldName)
            {
                return i;
            }
        }
        return -1;
    }

    public int GetFieldCount()
    {
        return this.fieldNames.Length;
    }

    public string GetFieldName(int field)
    {
        Debug.Assert(field >= 0 && field < this.fieldTypes.Length);
            
        return this.fieldNames[field];
    }

    public ConfigSheetLine[] GetLinesByFieldNameValue(string[] fieldNames, object[] values)
    {
        Debug.Assert(((fieldNames != null) && (values != null)) && (fieldNames.Length == values.Length));

        int length = fieldNames.Length;
        int[] fieldIndexs = new int[length];
        for (int i = 0; i < length; i++)
        {
            fieldIndexs[i] = GetFieldByName(fieldNames[i]);
            if (fieldIndexs[i] < 0)
            {
                return null;
            }
        }
        List<ConfigSheetLine> list = null;
        for (int i = 0; i < this.data.Length; ++i)
        {
            ConfigSheetLine line = this.data[i];
            bool flag = true;
            for (int j = 0; j < length; j++)
            {
                if (!line.GetData(fieldIndexs[j]).Equals(values[j]))
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                if (list == null)
                {
                    list = new List<ConfigSheetLine>();
                }
                list.Add(line);
            }
        }
        if (list != null)
        {
            return list.ToArray();
        }
        return null;
    }

    public int GetRecordCount()
    {
        return this.data.Length;
    }
}

